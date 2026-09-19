#!/usr/bin/env python3
"""Inventory the pinned CTP SDK and the bridge's managed/native surfaces.

Clang's JSON AST is preferred when available. The fallback parser intentionally
only covers declarations needed for the inventory, so CI can still validate the
checked-in manifest on Windows runners without requiring Clang.
"""

from __future__ import annotations

import argparse
import fnmatch
import json
import re
import shutil
import subprocess
import sys
from pathlib import Path
from typing import Any, Iterable


SDK_VERSION = "v6.7.13_20260225"
GENERATOR_VERSION = 2

MANUAL_INTEROP_SOURCES = (
    "Ctp.Net/Bridge/Common.fs",
    "Ctp.Net/Bridge/MdBridge.fs",
    "Ctp.Net/Bridge/TraderBridge.fs",
)
GENERATED_INTEROP_SOURCE = "Ctp.Net/Bridge/GeneratedNativeStructs.fs"
MANAGED_FIELD_ALIASES = {
    # The public records historically used this descriptive name while the
    # native SDK and C ABI call the field ExchangeRate.
    "exchangerate": "exchangerateresponse",
}


def snake_case(value: str) -> str:
    value = re.sub(r"([A-Z]+)([A-Z][a-z])", r"\1_\2", value)
    value = re.sub(r"([a-z0-9])([A-Z])", r"\1_\2", value)
    return value.lower()


def compact_name(value: str) -> str:
    return re.sub(r"[^a-z0-9]", "", value.lower())


def official_c_struct_name(official_name: str) -> str:
    body = official_name.removeprefix("CThostFtdc").removesuffix("Field")
    special = {
        "QrySecAgentACIDMap": "qry_sec_agent_ac_id_map",
        "SecAgentACIDMap": "sec_agent_ac_id_map",
    }
    return "ctp_" + special.get(body, snake_case(body))


def official_c_field_name(official_name: str) -> str:
    return snake_case(official_name)


def native_type_name(c_name: str) -> str:
    value = c_name.removeprefix("ctp_")
    if value == "cfmmc_trading_account_token":
        return "NativeCFMMCTradingAccountToken"
    return "Native" + "".join(part[:1].upper() + part[1:] for part in value.split("_"))


def walk(node: Any) -> Iterable[dict[str, Any]]:
    if isinstance(node, dict):
        yield node
        for child in node.get("inner", []):
            yield from walk(child)


def run_ast(header: Path, include_dir: Path, cxx: bool) -> dict[str, Any] | None:
    compiler = shutil.which("clang++" if cxx else "clang")
    if compiler is None:
        return None

    command = [
        compiler,
        "-fsyntax-only",
        "-Xclang",
        "-ast-dump=json",
        "-x",
        "c++" if cxx else "c",
        str(header),
        "-I",
        str(include_dir),
        "-o",
        "-",
    ]
    completed = subprocess.run(command, capture_output=True, text=True, check=False)
    if completed.returncode != 0:
        return None

    try:
        return json.loads(completed.stdout)
    except json.JSONDecodeError:
        return None


def method_from_ast(node: dict[str, Any]) -> dict[str, Any]:
    parameters = []
    for child in node.get("inner", []):
        if child.get("kind") == "ParmVarDecl":
            parameters.append(
                {
                    "name": child.get("name", ""),
                    "type": child.get("type", {}).get("qualType", ""),
                }
            )

    qualified_type = node.get("type", {}).get("qualType", "")
    return_type = qualified_type.split("(", 1)[0].strip()
    return {
        "name": node.get("name", ""),
        "return_type": return_type,
        "parameters": parameters,
    }


def parameter_from_text(parameter: str) -> dict[str, str] | None:
    value = parameter.strip()
    if not value or value == "void":
        return None

    value = re.sub(r"\s*=\s*[^,]+$", "", value).strip()
    name_match = re.search(r"([A-Za-z_]\w*)\s*(?:\[[^]]*\])?$", value)
    return {
        "name": name_match.group(1) if name_match else "",
        "type": value,
    }


def find_record(ast: dict[str, Any], name: str) -> dict[str, Any] | None:
    records = [
        node
        for node in walk(ast)
        if node.get("kind") in {"RecordDecl", "CXXRecordDecl"}
        and node.get("name") == name
        and any(child.get("kind") in {"FieldDecl", "CXXMethodDecl"} for child in node.get("inner", []))
    ]
    return max(records, key=lambda node: len(node.get("inner", [])), default=None)


def ast_api(header: Path, include_dir: Path, api_class: str, spi_class: str) -> tuple[list[dict[str, Any]], list[dict[str, Any]]]:
    ast = run_ast(header, include_dir, True)
    if ast is None:
        return [], []

    api_record = find_record(ast, api_class)
    spi_record = find_record(ast, spi_class)
    api_methods = []
    spi_methods = []
    if api_record:
        api_methods = [
            method_from_ast(node)
            for node in api_record.get("inner", [])
            if node.get("kind") == "CXXMethodDecl"
            and not node.get("isImplicit")
            and node.get("name")
        ]
    if spi_record:
        spi_methods = [
            method_from_ast(node)
            for node in spi_record.get("inner", [])
            if node.get("kind") == "CXXMethodDecl"
            and not node.get("isImplicit")
            and node.get("name")
        ]
    return api_methods, spi_methods


def text_api(header: Path) -> tuple[list[dict[str, Any]], list[dict[str, Any]]]:
    source = header.read_text(encoding="utf-8", errors="replace")
    methods = []
    callbacks = []
    for return_type, name, parameters in re.findall(
        r"^\s*(?:virtual|static)\s+([^;(){}\n]+?)\s+[*&]?\s*(\w+)\s*\(([^)]*)\)\s*(?:=\s*0)?\s*(?:\{\s*\}|;)",
        source,
        re.M,
    ):
        parsed_parameters = [parameter_from_text(parameter) for parameter in parameters.split(",")]
        item = {
            "name": name,
            "return_type": " ".join(return_type.split()),
            "parameters": [parameter for parameter in parsed_parameters if parameter is not None],
        }
        if name.startswith("On"):
            callbacks.append(item)
        else:
            methods.append(item)
    return methods, callbacks


def official_api(sdk_dir: Path) -> dict[str, Any]:
    md_dir = sdk_dir / "mduserapi" / "linux-x64"
    trader_dir = sdk_dir / "traderapi" / "linux-x64"
    md_header = md_dir / "ThostFtdcMdApi.h"
    trader_header = trader_dir / "ThostFtdcTraderApi.h"

    md_methods, md_callbacks = ast_api(md_header, md_dir, "CThostFtdcMdApi", "CThostFtdcMdSpi")
    trader_methods, trader_callbacks = ast_api(
        trader_header, trader_dir, "CThostFtdcTraderApi", "CThostFtdcTraderSpi"
    )

    if not md_methods:
        md_methods, md_callbacks = text_api(md_header)
    if not trader_methods:
        trader_methods, trader_callbacks = text_api(trader_header)

    return {
        "md": {
            "methods": md_methods,
            "callbacks": md_callbacks,
        },
        "trader": {
            "methods": trader_methods,
            "callbacks": trader_callbacks,
        },
    }


def field_from_ast(node: dict[str, Any]) -> dict[str, Any]:
    field_type = node.get("type", {})
    desugared = field_type.get("desugaredQualType", "")
    qualified = field_type.get("qualType", "")
    array_length = None
    match = re.search(r"\[(\d+)\]$", desugared or qualified)
    if match:
        array_length = int(match.group(1))
    return {
        "name": node.get("name", ""),
        "type": field_type.get("qualType", ""),
        "desugared_type": desugared or None,
        "array_length": array_length,
    }


def official_structs(sdk_dir: Path) -> list[dict[str, Any]]:
    header = sdk_dir / "reference" / "ThostFtdcUserApiStruct.h"
    include_dir = header.parent
    ast = run_ast(header, include_dir, True)
    result = []
    if ast:
        seen = set()
        for node in walk(ast):
            name = node.get("name", "")
            if (
                node.get("kind") not in {"RecordDecl", "CXXRecordDecl"}
                or not name.startswith("CThostFtdc")
                or not name.endswith("Field")
                or name in seen
            ):
                continue
            fields = [
                field_from_ast(child)
                for child in node.get("inner", [])
                if child.get("kind") == "FieldDecl" and child.get("name")
            ]
            if fields:
                seen.add(name)
                result.append({"name": name, "fields": fields})
    if result:
        return sorted(result, key=lambda item: item["name"])

    source = header.read_text(encoding="utf-8", errors="replace")
    data_types = header.parent / "ThostFtdcUserApiDataType.h"
    array_aliases = {}
    if data_types.exists():
        data_type_source = data_types.read_text(encoding="utf-8", errors="replace")
        array_aliases = {
            name: int(length)
            for _, name, length in re.findall(
                r"typedef\s+([^;\[]+?)\s+(\w+)\s*\[(\d+)\]\s*;", data_type_source
            )
        }

    fallback = []
    for name, body in re.findall(r"struct\s+(CThostFtdc\w+Field)\s*\{(.*?)\};", source, re.S):
        fields = []
        for declaration in body.split(";"):
            declaration = re.sub(r"//.*", "", declaration).strip()
            match = re.match(r"(.+?)\s+(\w+)(?:\[(\d+)\])?$", declaration)
            if match:
                field_type = " ".join(match.group(1).split())
                array_length = int(match.group(3)) if match.group(3) else array_aliases.get(field_type.split()[-1])
                fields.append(
                    {
                        "name": match.group(2),
                        "type": field_type,
                        "desugared_type": None,
                        "array_length": array_length,
                    }
                )
        fallback.append({"name": name, "fields": fields})
    return sorted(fallback, key=lambda item: item["name"])


def official_field_by_c_name(official_struct: dict[str, Any]) -> dict[str, dict[str, Any]]:
    return {
        compact_name(official_c_field_name(field["name"])): field
        for field in official_struct["fields"]
    }


def c_decl_for_official_field(field: dict[str, Any]) -> str:
    array_length = field.get("array_length")
    if array_length is not None:
        return f"char[{array_length}]"

    desugared = field.get("desugared_type") or field.get("type", "")
    desugared = re.sub(r"\s+", " ", desugared).strip()
    if desugared == "double":
        return "double"
    if desugared in {"short", "short int"}:
        return "int16_t"
    if desugared in {"int", "signed int"}:
        return "int32_t"
    if desugared == "char":
        return "char"
    raise ValueError(
        f"Unsupported native field type {desugared!r} for {field.get('name', '')}."
    )


def merge_official_abi_structs(
    official_struct_list: list[dict[str, Any]], current_abi: dict[str, Any]
) -> list[dict[str, Any]]:
    current_by_name = {item["name"]: item for item in current_abi["structs"]}
    merged = []

    for official in official_struct_list:
        c_name = official_c_struct_name(official["name"])
        current = current_by_name.get(c_name)
        current_fields = current.get("fields", []) if current else []
        official_by_name = official_field_by_c_name(official)
        current_matches: dict[str, dict[str, Any]] = {}
        for current_field in current_fields:
            key = compact_name(current_field["name"])
            if key not in official_by_name:
                raise ValueError(
                    f"Current C ABI field {c_name}.{current_field['name']} is not present in "
                    f"{official['name']}."
                )
            if key in current_matches:
                raise ValueError(f"Duplicate C ABI field mapping in {c_name}: {current_field['name']}.")
            current_matches[key] = current_field

        fields = []
        for current_field in current_fields:
            official_field = official_by_name[compact_name(current_field["name"])]
            fields.append(
                {
                    "name": current_field["name"],
                    "type": c_decl_for_official_field(official_field),
                    "desugared_type": c_decl_for_official_field(official_field),
                    "array_length": official_field.get("array_length"),
                }
            )

        for official_field in official["fields"]:
            key = compact_name(official_c_field_name(official_field["name"]))
            if key in current_matches:
                continue
            c_decl = c_decl_for_official_field(official_field)
            fields.append(
                {
                    "name": official_c_field_name(official_field["name"]),
                    "type": c_decl,
                    "desugared_type": c_decl,
                    "array_length": official_field.get("array_length"),
                }
            )

        merged.append({"name": c_name, "fields": fields})

    return sorted(merged, key=lambda item: item["name"])


def render_c_abi_structs(structs: list[dict[str, Any]]) -> str:
    lines = [
        "/* Generated from the pinned CTP SDK. Do not hand-edit this block. */",
        "",
    ]
    for item in structs:
        lines.append(f"typedef struct {item['name']} {{")
        for field in item["fields"]:
            field_type = field["type"]
            if field.get("array_length") is not None:
                lines.append(f"  char {field['name']}[{field['array_length']}];")
            else:
                lines.append(f"  {field_type} {field['name']};")
        lines.extend([f"}} {item['name']};", ""])
    return "\n".join(lines)


def rewrite_c_abi_header(repo_root: Path, official_struct_list: list[dict[str, Any]]) -> None:
    header_path = repo_root / "NativeBridge" / "include" / "ctp_bridge.h"
    source = header_path.read_text(encoding="utf-8")
    current_abi = c_abi_inventory(repo_root)
    merged = merge_official_abi_structs(official_struct_list, current_abi)

    pattern = re.compile(
        r"typedef\s+struct\s+(ctp_\w+)\s*\{.*?\}\s*\1\s*;\s*",
        re.S,
    )

    def keep_spi(match: re.Match[str]) -> str:
        return match.group(0) if match.group(1).endswith("_spi") else ""

    source = pattern.sub(keep_spi, source)
    source = re.sub(r"\nstruct\s+ctp_for_quote_rsp\s*;\s*\n", "\n", source)
    marker = '#ifdef __cplusplus\nextern "C" {\n#endif\n'
    if marker not in source:
        raise ValueError(f"Could not find C ABI insertion marker in {header_path}.")
    source = source.replace(marker, marker + "\n" + render_c_abi_structs(merged), 1)
    header_path.write_text(source, encoding="utf-8")


def fsharp_decl_for_c_field(field: dict[str, Any]) -> str:
    if field.get("array_length") is not None:
        return "byte array"
    field_type = field.get("type", "")
    if field_type == "double":
        return "float"
    if field_type == "int16_t":
        return "int16"
    if field_type == "int32_t":
        return "int"
    if field_type == "char":
        return "byte"
    raise ValueError(f"Unsupported C ABI field type {field_type!r} for {field['name']}.")


def render_fsharp_field(field: dict[str, Any], name: str | None = None) -> list[str]:
    field_name = name or "".join(part[:1].upper() + part[1:] for part in field["name"].split("_"))
    lines = []
    if field.get("array_length") is not None:
        lines.extend(
            [
                f"    [<MarshalAs(UnmanagedType.ByValArray, SizeConst = {field['array_length']})>]",
                "    [<DefaultValue>]",
            ]
        )
    else:
        lines.append("    [<DefaultValue>]")
    lines.append(f"    val mutable {field_name}: {fsharp_decl_for_c_field(field)}")
    return lines


def manual_native_type_names(repo_root: Path) -> set[str]:
    result = set()
    for relative in MANUAL_INTEROP_SOURCES:
        source = (repo_root / relative).read_text(encoding="utf-8")
        result.update(re.findall(r"^type\s+(?:private|internal)\s+(Native\w+)\s*=", source, re.M))
    return result


def render_generated_fsharp_structs(
    repo_root: Path, abi_structs: list[dict[str, Any]]
) -> str:
    manual_names = manual_native_type_names(repo_root)
    lines = [
        "namespace Ctp.Net.Bridge",
        "",
        "open System.Runtime.InteropServices",
        "",
        "// Generated from NativeBridge/include/ctp_bridge.h and the pinned CTP SDK.",
        "// Do not hand-edit this file; run tools/ctp_api.py generate.",
        "",
    ]
    for item in abi_structs:
        if item["name"].endswith("_spi"):
            continue
        name = native_type_name(item["name"])
        if name in manual_names:
            continue
        lines.extend(["[<Struct; StructLayout(LayoutKind.Sequential)>]", f"type private {name} ="])
        for field in item["fields"]:
            lines.extend(render_fsharp_field(field))
            lines.append("")
        if lines[-1] == "":
            lines.pop()
        lines.append("")
    return "\n".join(lines)


def replace_struct_body(source: str, type_name: str, body: str) -> str:
    pattern = re.compile(
        rf"(?ms)^(?P<prefix>\[<Struct; StructLayout\(LayoutKind\.Sequential\)>\]\n"
        rf"type\s+(?:private|internal)\s+{re.escape(type_name)}\s*=\n)"
        rf"(?P<body>.*?)(?=^\[<|^type\s|\Z)"
    )
    match = pattern.search(source)
    if not match:
        raise ValueError(f"Could not find managed native struct {type_name}.")
    return source[: match.start()] + match.group("prefix") + body + source[match.end() :]


def update_existing_fsharp_structs(
    repo_root: Path, abi_structs: list[dict[str, Any]]
) -> None:
    abi_by_native_name = {
        native_type_name(item["name"]): item
        for item in abi_structs
        if not item["name"].endswith("_spi")
    }
    # The MD and trader bridge keep separate native declarations for the same
    # callback payload. Both must track the one C ABI layout.
    if "ctp_depth_market_data" in {item["name"] for item in abi_structs}:
        depth = next(item for item in abi_structs if item["name"] == "ctp_depth_market_data")
        abi_by_native_name["NativeTraderDepthMarketData"] = depth
    for relative in MANUAL_INTEROP_SOURCES:
        path = repo_root / relative
        source = path.read_text(encoding="utf-8")
        for native_name, abi in abi_by_native_name.items():
            pattern = re.compile(
                rf"(?ms)^(?P<prefix>\[<Struct; StructLayout\(LayoutKind\.Sequential\)>\]\n"
                rf"type\s+(?:private|internal)\s+{re.escape(native_name)}\s*=\n)"
                rf"(?P<body>.*?)(?=^\[<|^type\s|\Z)"
            )
            match = pattern.search(source)
            if not match:
                continue
            body = match.group("body")
            existing_names = set(re.findall(r"val mutable\s+(\w+)\s*:", body))
            existing_by_compact = {compact_name(name): name for name in existing_names}

            for field in abi["fields"]:
                expected_name = "".join(part[:1].upper() + part[1:] for part in field["name"].split("_"))
                current_name = existing_by_compact.get(compact_name(field["name"]))
                if current_name is None:
                    current_name = existing_by_compact.get(
                        MANAGED_FIELD_ALIASES.get(compact_name(field["name"]), "")
                    )
                if current_name is not None and field.get("array_length") is not None:
                    field_pattern = re.compile(
                        rf"(?P<attrs>(?:^[ \t]*\[<[^\n]+>\][ \t]*\n)*)"
                        rf"^[ \t]*val mutable {re.escape(current_name)}\s*:\s*byte array",
                        re.M,
                    )
                    field_match = field_pattern.search(body)
                    if field_match:
                        attrs = re.sub(
                            r"SizeConst\s*=\s*\d+",
                            f"SizeConst = {field['array_length']}",
                            field_match.group("attrs"),
                        )
                        body = body[: field_match.start("attrs")] + attrs + body[field_match.end("attrs") :]
                    continue
                if current_name is not None:
                    continue
                addition = "\n" + "\n".join(render_fsharp_field(field, expected_name)) + "\n"
                body = body.rstrip() + addition
                existing_by_compact[compact_name(field["name"])] = expected_name

            source = source[: match.start()] + match.group("prefix") + body + source[match.end() :]
        path.write_text(source, encoding="utf-8")


def render_cpp_copy_body(
    official_struct: dict[str, Any], c_abi_struct: dict[str, Any], direction: str
) -> str:
    c_fields = {compact_name(field["name"]): field for field in c_abi_struct["fields"]}
    lines = ["{", "  std::memset(&dest, 0, sizeof(dest));"]
    if direction == "from_native":
        lines.append("  if (src == nullptr) {")
        lines.append("    return;")
        lines.append("  }")

    for official_field in official_struct["fields"]:
        key = compact_name(official_c_field_name(official_field["name"]))
        c_field = c_fields[key]
        if official_field.get("array_length") is not None:
            if direction == "from_native":
                lines.append(f"  copy_field(dest.{c_field['name']}, src->{official_field['name']});")
            else:
                lines.append(f"  copy_field(dest.{official_field['name']}, src.{c_field['name']});")
        elif direction == "from_native":
            lines.append(f"  dest.{c_field['name']} = src->{official_field['name']};")
        else:
            lines.append(f"  dest.{official_field['name']} = src.{c_field['name']};")
    lines.append("}")
    return "\n".join(lines)


def replace_cpp_function_body(
    source: str, function_name: str, body: str, occurrence: int = 0
) -> str:
    signatures = list(re.finditer(rf"\bvoid\s+{re.escape(function_name)}\s*\(", source))
    if occurrence >= len(signatures):
        raise ValueError(
            f"Could not find C++ function {function_name} occurrence {occurrence}."
        )
    signature = signatures[occurrence]
    if signature is None:
        raise ValueError(f"Could not find C++ function {function_name}.")
    opening = source.find("{", signature.end())
    if opening < 0:
        raise ValueError(f"Could not find C++ body for {function_name}.")
    depth = 0
    closing = None
    for index in range(opening, len(source)):
        if source[index] == "{":
            depth += 1
        elif source[index] == "}":
            depth -= 1
            if depth == 0:
                closing = index
                break
    if closing is None:
        raise ValueError(f"Could not balance C++ body for {function_name}.")
    return source[:opening] + body + source[closing + 1 :]


def update_cpp_copy_functions(repo_root: Path, sdk_dir: Path, abi_structs: list[dict[str, Any]]) -> None:
    official = {item["name"]: item for item in official_structs(sdk_dir)}
    abi = {item["name"]: item for item in abi_structs}
    md_specs = [
        ("fill_specific_instrument", "CThostFtdcSpecificInstrumentField", "ctp_specific_instrument", "from_native", 0),
        ("fill_multicast_instrument", "CThostFtdcMulticastInstrumentField", "ctp_multicast_instrument", "from_native", 0),
        ("fill_depth_market_data", "CThostFtdcDepthMarketDataField", "ctp_depth_market_data", "from_native", 0),
        ("fill_req_user_login", "CThostFtdcReqUserLoginField", "ctp_req_user_login", "to_native", 0),
    ]
    trader_specs = [
        ("fill_trading_account", "CThostFtdcTradingAccountField", "ctp_trading_account", "from_native", 0),
        ("fill_investor_position", "CThostFtdcInvestorPositionField", "ctp_investor_position", "from_native", 0),
        ("fill_input_order", "CThostFtdcInputOrderField", "ctp_input_order", "from_native", 0),
        ("fill_input_order_action", "CThostFtdcInputOrderActionField", "ctp_input_order_action", "from_native", 0),
        ("fill_order", "CThostFtdcOrderField", "ctp_order", "from_native", 0),
        ("fill_trade", "CThostFtdcTradeField", "ctp_trade", "from_native", 0),
        ("fill_depth_market_data", "CThostFtdcDepthMarketDataField", "ctp_depth_market_data", "from_native", 0),
        ("fill_req_user_login", "CThostFtdcReqUserLoginField", "ctp_req_user_login", "to_native", 0),
        ("fill_qry_investor_position", "CThostFtdcQryInvestorPositionField", "ctp_qry_investor_position", "to_native", 0),
        ("fill_qry_instrument_margin_rate", "CThostFtdcQryInstrumentMarginRateField", "ctp_qry_instrument_margin_rate", "to_native", 0),
        ("fill_qry_exchange_margin_rate", "CThostFtdcQryExchangeMarginRateField", "ctp_qry_exchange_margin_rate", "to_native", 0),
        ("fill_qry_instrument_commission_rate", "CThostFtdcQryInstrumentCommissionRateField", "ctp_qry_instrument_commission_rate", "to_native", 0),
        ("fill_input_order", "CThostFtdcInputOrderField", "ctp_input_order", "to_native", 1),
        ("fill_input_order_action", "CThostFtdcInputOrderActionField", "ctp_input_order_action", "to_native", 1),
    ]

    for relative, specs in (
        ("NativeBridge/src/md_bridge.cpp", md_specs),
        ("NativeBridge/src/trader_bridge.cpp", trader_specs),
    ):
        path = repo_root / relative
        source = path.read_text(encoding="utf-8")
        for function_name, official_name, c_name, direction, occurrence in specs:
            source = replace_cpp_function_body(
                source,
                function_name,
                render_cpp_copy_body(official[official_name], abi[c_name], direction),
                occurrence,
            )
        path.write_text(source, encoding="utf-8")


def validate_cpp_copy_coverage(
    repo_root: Path, sdk_dir: Path, abi_structs: list[dict[str, Any]]
) -> None:
    """Ensure every field of each implemented C++ copy helper is transferred."""

    official_by_c_name = {
        official_c_struct_name(item["name"]): item for item in official_structs(sdk_dir)
    }
    abi_by_name = {item["name"]: item for item in abi_structs}
    errors = []

    for relative in ("NativeBridge/src/md_bridge.cpp", "NativeBridge/src/trader_bridge.cpp"):
        source = (repo_root / relative).read_text(encoding="utf-8")
        for match in re.finditer(r"\bvoid\s+(fill_\w+)\s*\(", source):
            opening = source.find("{", match.end())
            if opening < 0:
                continue

            depth = 0
            closing = None
            for index in range(opening, len(source)):
                if source[index] == "{":
                    depth += 1
                elif source[index] == "}":
                    depth -= 1
                    if depth == 0:
                        closing = index
                        break

            if closing is None:
                continue

            signature = source[match.start() : opening]
            body = source[opening : closing + 1]
            c_match = re.search(r"\b(ctp_\w+)\s*[*&]?\s*(dest|src)\b", signature)
            official_match = re.search(r"\bCThostFtdc(\w+)Field\b", signature)
            if c_match is None or official_match is None:
                continue

            c_name = c_match.group(1)
            c_variable = c_match.group(2)
            official_name = f"CThostFtdc{official_match.group(1)}Field"
            if c_name not in official_by_c_name:
                errors.append(f"{relative}:{match.group(1)} uses unknown C ABI structure {c_name}")
                continue
            if c_name not in abi_by_name:
                errors.append(f"{relative}:{match.group(1)} has no C ABI structure {c_name}")
                continue

            expected_c_name = official_c_struct_name(official_name)
            if expected_c_name != c_name:
                errors.append(
                    f"{relative}:{match.group(1)} maps {official_name} to {c_name}, "
                    f"expected {expected_c_name}"
                )
                continue

            native_variable = "src" if c_variable == "dest" else "dest"
            native_operator = "->" if c_variable == "dest" else "."
            c_operator = "." if c_variable == "dest" else "."
            official_fields = {
                compact_name(official_c_field_name(field["name"])): field
                for field in official_by_c_name[c_name]["fields"]
            }
            for field in abi_by_name[c_name]["fields"]:
                field_name = field["name"]
                official_field = official_fields[compact_name(field_name)]
                c_reference = rf"\b{c_variable}{c_operator}{re.escape(field_name)}\b"
                native_reference = rf"\b{native_variable}{native_operator}{re.escape(official_field['name'])}\b"
                if not re.search(c_reference, body):
                    errors.append(
                        f"{relative}:{match.group(1)} does not copy {c_name}.{field_name}"
                    )
                if not re.search(native_reference, body):
                    errors.append(
                        f"{relative}:{match.group(1)} does not read {official_name}.{official_field['name']}"
                    )

    if errors:
        raise SystemExit("C++ field-copy coverage drift:\n" + "\n".join(errors))


def complete_generated_surfaces(repo_root: Path, sdk_dir: Path) -> None:
    official_struct_list = official_structs(sdk_dir)
    current_abi = c_abi_inventory(repo_root)
    merged = merge_official_abi_structs(official_struct_list, current_abi)
    rewrite_c_abi_header(repo_root, official_struct_list)
    update_existing_fsharp_structs(repo_root, merged)
    generated_path = repo_root / GENERATED_INTEROP_SOURCE
    generated_path.write_text(render_generated_fsharp_structs(repo_root, merged), encoding="utf-8")
    update_cpp_copy_functions(repo_root, sdk_dir, merged)


def ctp_constants(sdk_dir: Path) -> list[dict[str, str]]:
    header = sdk_dir / "reference" / "ThostFtdcUserApiDataType.h"
    source = header.read_text(encoding="utf-8", errors="replace")
    constants = []
    for name, value in re.findall(r"^#define\s+(THOST_FTDC_\w+)\s+'(.)'", source, re.M):
        constants.append({"name": name, "value": value})
    return sorted(constants, key=lambda item: item["name"])


def ctp_enums(sdk_dir: Path) -> list[dict[str, Any]]:
    header = sdk_dir / "reference" / "ThostFtdcUserApiDataType.h"
    source = header.read_text(encoding="utf-8", errors="replace")
    enums = []
    for name, body in re.findall(r"\benum\s+(\w+)\s*\{(.*?)\};", source, re.S):
        members = []
        for declaration in body.split(","):
            declaration = re.sub(r"/\*.*?\*/|//.*", "", declaration, flags=re.S).strip()
            if not declaration:
                continue
            match = re.match(r"(\w+)(?:\s*=\s*(.*))?$", declaration)
            if match:
                members.append({"name": match.group(1), "value": match.group(2)})
        enums.append({"name": name, "members": members})
    return sorted(enums, key=lambda item: item["name"])


def c_abi_inventory(repo_root: Path) -> dict[str, Any]:
    header = repo_root / "NativeBridge" / "include" / "ctp_bridge.h"
    ast = run_ast(header, header.parent, False)
    structs = []
    functions = []
    callbacks = []
    if ast:
        for node in walk(ast):
            kind = node.get("kind")
            name = node.get("name", "")
            if kind == "RecordDecl" and name.startswith("ctp_"):
                fields = [
                    field_from_ast(child)
                    for child in node.get("inner", [])
                    if child.get("kind") == "FieldDecl" and child.get("name")
                ]
                if fields:
                    structs.append({"name": name, "fields": fields})
                if name.endswith("_spi"):
                    callbacks.extend(
                        {
                            "struct": name,
                            "name": child.get("name", ""),
                            "type": child.get("type", {}).get("qualType", ""),
                        }
                        for child in node.get("inner", [])
                        if child.get("kind") == "FieldDecl" and child.get("name")
                    )
            elif kind == "FunctionDecl" and name.startswith("ctp_"):
                functions.append(method_from_ast(node))
    if not ast or not functions:
        source = header.read_text(encoding="utf-8", errors="replace")
        for name, body in re.findall(r"typedef\s+struct\s+(ctp_\w+)\s*\{(.*?)\}\s*\1\s*;", source, re.S):
            fields = []
            for declaration in body.split(";"):
                callback_match = re.search(r"\(\*\s*(\w+)\s*\)\s*\(", declaration)
                if callback_match:
                    fields.append(
                        {
                            "name": callback_match.group(1),
                            "type": " ".join(declaration.split()),
                            "desugared_type": None,
                            "array_length": None,
                        }
                    )
                    continue

                match = re.match(r"\s*(.+?)\s+(\w+)(?:\[(\d+)\])?\s*$", declaration)
                if match:
                    fields.append(
                        {
                            "name": match.group(2),
                            "type": " ".join(match.group(1).split()),
                            "desugared_type": None,
                            "array_length": int(match.group(3)) if match.group(3) else None,
                        }
                    )
            structs.append({"name": name, "fields": fields})
        for name, parameters in re.findall(r"\b(ctp_\w+)\s*\(([^;]*)\)\s*;", source, re.S):
            parsed_parameters = [parameter_from_text(parameter) for parameter in parameters.split(",")]
            functions.append(
                {
                    "name": name,
                    "return_type": "",
                    "parameters": [parameter for parameter in parsed_parameters if parameter is not None],
                }
            )

    source = header.read_text(encoding="utf-8", errors="replace")
    for struct_name, callback_body in re.findall(
        r"typedef\s+struct\s+(ctp_\w+_spi)\s*\{(.*?)\}\s*\1\s*;", source, re.S
    ):
        if any(item.get("struct") == struct_name for item in callbacks):
            continue
        callbacks.extend(
            {"struct": struct_name, "name": name, "type": f"void (*)({params})"}
            for name, params in re.findall(
                r"void\s*\(\*\s*(\w+)\s*\)\s*\(([^;]+)\)\s*;", callback_body
            )
        )

    return {
        "structs": sorted({item["name"]: item for item in structs}.values(), key=lambda item: item["name"]),
        "functions": sorted({item["name"]: item for item in functions}.values(), key=lambda item: item["name"]),
        "callbacks": sorted(
            { (item.get("struct", ""), item["name"]): item for item in callbacks }.values(),
            key=lambda item: (item.get("struct", ""), item["name"]),
        ),
    }


def source_members(source: str) -> list[str]:
    names = []
    for match in re.finditer(r"^\s+member\s+(?:this|_)\.([A-Za-z_]\w*)", source, re.M):
        names.append(match.group(1))
    return sorted(set(names))


def managed_inventory(repo_root: Path) -> dict[str, Any]:
    md = (repo_root / "Ctp.Net" / "Md.fs").read_text(encoding="utf-8")
    trader = (repo_root / "Ctp.Net" / "Trader.fs").read_text(encoding="utf-8")
    csharp_md = (repo_root / "Ctp.Net" / "CSharp" / "MdClient.fs").read_text(encoding="utf-8")
    csharp_trader = (repo_root / "Ctp.Net" / "CSharp" / "TraderClient.fs").read_text(encoding="utf-8")

    interop_sources = {
        relative: (repo_root / relative).read_text(encoding="utf-8")
        for relative in (
            "Ctp.Net/Bridge/Common.fs",
            "Ctp.Net/Bridge/MdBridge.fs",
            "Ctp.Net/Bridge/TraderBridge.fs",
            GENERATED_INTEROP_SOURCE,
        )
    }

    native_types = []
    dll_imports = []
    for relative, source in interop_sources.items():
        library_match = re.search(r'let\s+Library\s*=\s*"([^"]+)"', source)
        library = library_match.group(1) if library_match else None

        for match in re.finditer(
            r"^type\s+(?:private|internal)\s+(Native\w+|\w+Delegate)\s*=\s*(?:delegate of[^\n]+|\n(.*?)(?=^\[<|^type |^module |\Z))",
            source,
            re.M | re.S,
        ):
            name = match.group(1)
            declaration = match.group(0)
            kind = "delegate" if "delegate of" in declaration else "struct"
            fields = []
            if kind == "struct":
                for field in re.finditer(
                    r"(?P<attributes>(?:^[ \t]*\[<[^\n]+>\][ \t]*\n)*)"
                    r"^[ \t]*val mutable (?P<name>\w+)\s*:\s*(?P<type>[^\n]+)",
                    match.group(2) or "",
                    re.M,
                ):
                    size_match = re.search(r"SizeConst\s*=\s*(\d+)", field.group("attributes"))
                    fields.append(
                        {
                            "name": field.group("name"),
                            "type": " ".join(field.group("type").split()),
                            "array_length": int(size_match.group(1)) if size_match else None,
                        }
                    )

            native_types.append(
                {
                    "file": relative,
                    "name": name,
                    "kind": kind,
                    "fields": fields,
                }
            )

        if library:
            for entry_point, name in re.findall(
                r"\[<DllImport\(Library.*?EntryPoint\s*=\s*\"([^\"]+)\"\)>\]\s*"
                r"\n\s*extern\s+[^\n]+?\s+(\w+)\(",
                source,
                re.S,
            ):
                dll_imports.append(
                    {
                        "file": relative,
                        "library": library,
                        "entry_point": entry_point,
                        "name": name,
                    }
                )

    return {
        "fsharp": {
            "md_members": source_members(md),
            "trader_members": source_members(trader),
        },
        "csharp": {
            "md_members": source_members(csharp_md),
            "trader_members": source_members(csharp_trader),
        },
        "interop": {
            "native_types": sorted(native_types, key=lambda item: (item["file"], item["name"])),
            "dll_imports": sorted(
                dll_imports,
                key=lambda item: (item["file"], item["entry_point"], item["name"]),
            ),
        },
    }


def resolve_sdk(root: Path, version: str) -> Path:
    direct = root / "mduserapi"
    if direct.is_dir():
        return root
    candidate = root / version
    if not candidate.is_dir():
        raise SystemExit(f"SDK version directory not found: {candidate}")
    return candidate


def build_manifest(repo_root: Path, sdk_root: Path, version: str) -> dict[str, Any]:
    sdk_dir = resolve_sdk(sdk_root, version)
    official = official_api(sdk_dir)
    return {
        "schema_version": 1,
        "generator_version": GENERATOR_VERSION,
        "sdk_version": sdk_dir.name,
        "sdk_root": "NativeBridge/ctp-sdk",
        "parser": "clang-ast-with-deterministic-fallback",
        "official": official,
        "official_structs": official_structs(sdk_dir),
        "official_constants": ctp_constants(sdk_dir),
        "official_enums": ctp_enums(sdk_dir),
        "c_abi": c_abi_inventory(repo_root),
        "managed": managed_inventory(repo_root),
    }


def baseline_members(repo_root: Path) -> dict[str, list[str]]:
    result = {}
    files = {
        "fsharp_md": "Ctp.Net/Md.fs",
        "fsharp_trader": "Ctp.Net/Trader.fs",
        "csharp_md": "Ctp.Net/CSharp/MdClient.fs",
        "csharp_trader": "Ctp.Net/CSharp/TraderClient.fs",
    }
    for name, relative in files.items():
        completed = subprocess.run(
            ["git", "show", f"HEAD:{relative}"],
            cwd=repo_root,
            capture_output=True,
            text=True,
            check=False,
        )
        result[name] = source_members(completed.stdout) if completed.returncode == 0 else []
    return result


def write_json(path: Path, value: Any) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(json.dumps(value, ensure_ascii=False, indent=2, sort_keys=True) + "\n", encoding="utf-8")


def api_signature(value: dict[str, Any]) -> dict[str, Any]:
    def methods(items: list[dict[str, Any]]) -> list[dict[str, Any]]:
        return [
            {
                "name": item["name"],
                "parameters": [parameter.get("name", "") for parameter in item.get("parameters", [])],
            }
            for item in items
        ]

    return {
        "md": {
            "methods": methods(value["md"]["methods"]),
            "callbacks": methods(value["md"]["callbacks"]),
        },
        "trader": {
            "methods": methods(value["trader"]["methods"]),
            "callbacks": methods(value["trader"]["callbacks"]),
        },
    }


def field_signature(items: list[dict[str, Any]]) -> list[dict[str, Any]]:
    def normalized_type(value: str) -> str:
        if re.search(r"\(\s*\*", value):
            return "function_pointer"
        return re.sub(r"\s+", " ", re.sub(r"\[\d+\]", "", value)).strip()

    return [
        {
            "name": item["name"],
            "fields": [
                {
                    "name": field["name"],
                    "type": normalized_type(field.get("type", "")),
                    "array_length": field.get("array_length"),
                }
                for field in item.get("fields", [])
            ],
        }
        for item in items
    ]


def enum_signature(items: list[dict[str, Any]]) -> list[dict[str, Any]]:
    return [
        {
            "name": item["name"],
            "members": [
                {"name": member["name"], "value": member.get("value")}
                for member in item.get("members", [])
            ],
        }
        for item in items
    ]


def constant_signature(items: list[dict[str, Any]]) -> list[dict[str, str]]:
    return [{"name": item["name"], "value": item["value"]} for item in items]


def c_abi_signature(value: dict[str, Any]) -> dict[str, Any]:
    return {
        "structs": field_signature(value["structs"]),
        "functions": [
            {
                "name": item["name"],
                "parameters": [parameter.get("name", "") for parameter in item.get("parameters", [])],
            }
            for item in value["functions"]
        ],
        "callbacks": [
            {"struct": item.get("struct", ""), "name": item["name"]}
            for item in value["callbacks"]
        ],
    }


def abi_probe_source(manifest: dict[str, Any]) -> str:
    lines = [
        '#include "ctp_bridge.h"',
        "#include <cstddef>",
        "#include <cstdio>",
        "#include <initializer_list>",
        "",
        "struct FieldLayout { const char* name; std::size_t offset; };",
        "static bool first = true;",
        "",
        "template <typename T>",
        "void print_layout(const char* name, std::initializer_list<FieldLayout> fields) {",
        '  if (!first) std::printf(",");',
        "  first = false;",
        '  std::printf("{\\\"name\\\":\\\"%s\\\",\\\"size\\\":%zu,\\\"alignment\\\":%zu,\\\"fields\\\":[", name, sizeof(T), alignof(T));',
        "  bool first_field = true;",
        "  for (const auto& field : fields) {",
        '    if (!first_field) std::printf(",");',
        "    first_field = false;",
        '    std::printf("{\\\"name\\\":\\\"%s\\\",\\\"offset\\\":%zu}", field.name, field.offset);',
        "  }",
        '  std::printf("]}");',
        "}",
        "",
        "int main() {",
        '  std::printf("{\\\"schema_version\\\":1,\\\"structs\\\":[");',
    ]
    for item in manifest["c_abi"]["structs"]:
        type_name = item["name"]
        lines.append(f'  print_layout<{type_name}>("{type_name}", {{')
        for field in item["fields"]:
            field_name = field["name"]
            lines.append(f'    {{"{field_name}", offsetof({type_name}, {field_name})}},')
        lines.append("  });")
    lines.extend(["  std::printf(\"]}\\n\");", "  return 0;", "}", ""])
    return "\n".join(lines)


def generate(repo_root: Path, sdk_root: Path, version: str, output_dir: Path) -> None:
    sdk_dir = resolve_sdk(sdk_root, version)
    complete_generated_surfaces(repo_root, sdk_dir)
    manifest = build_manifest(repo_root, sdk_root, version)
    write_json(output_dir / "ctp-api-manifest.json", manifest)
    write_json(
        output_dir / "field-manifest.json",
        {
            "schema_version": 1,
            "sdk_version": manifest["sdk_version"],
            "structs": manifest["official_structs"],
        },
    )
    write_json(
        output_dir / "compatibility-v0.7.1.json",
        {
            "schema_version": 1,
            "baseline": "ce1c00a",
            "members": baseline_members(repo_root),
        },
    )
    probe_path = repo_root / "NativeBridge" / "generated" / "ctp_bridge_abi_probe.cpp"
    probe_path.parent.mkdir(parents=True, exist_ok=True)
    probe_path.write_text(abi_probe_source(manifest), encoding="utf-8")


def validate_official_abi_coverage(
    official_struct_list: list[dict[str, Any]], c_abi: dict[str, Any]
) -> None:
    official_by_c_name = {
        official_c_struct_name(item["name"]): item for item in official_struct_list
    }
    c_abi_by_name = {item["name"]: item for item in c_abi["structs"]}
    missing_structs = sorted(set(official_by_c_name) - set(c_abi_by_name))
    if missing_structs:
        raise SystemExit(f"Official C ABI structures are missing: {missing_structs}")

    unexpected_structs = sorted(
        set(c_abi_by_name) - set(official_by_c_name) - {"ctp_md_spi", "ctp_trader_spi"}
    )
    if unexpected_structs:
        raise SystemExit(f"C ABI has structures without an official SDK source: {unexpected_structs}")

    field_errors = []
    for c_name, official in official_by_c_name.items():
        actual_fields = c_abi_by_name[c_name]["fields"]
        actual_by_name = {compact_name(field["name"]): field for field in actual_fields}
        expected_keys = {compact_name(official_c_field_name(field["name"])) for field in official["fields"]}
        missing_fields = sorted(expected_keys - set(actual_by_name))
        extra_fields = sorted(set(actual_by_name) - expected_keys)
        if missing_fields or extra_fields:
            field_errors.append(
                f"{c_name}: missing={missing_fields}, extra={extra_fields}"
            )
            continue
        for official_field in official["fields"]:
            key = compact_name(official_c_field_name(official_field["name"]))
            actual_field = actual_by_name[key]
            expected_type = c_decl_for_official_field(official_field)
            if (
                actual_field.get("array_length") != official_field.get("array_length")
                or actual_field.get("type") != expected_type
            ):
                field_errors.append(
                    f"{c_name}.{official_field['name']}: expected={expected_type}[{official_field.get('array_length')}], "
                    f"actual={actual_field.get('type')}[{actual_field.get('array_length')}]"
                )
    if field_errors:
        raise SystemExit("Official C ABI field coverage/type drift:\n" + "\n".join(field_errors))


def check(repo_root: Path, sdk_root: Path, version: str, output_dir: Path) -> None:
    manifest_path = output_dir / "ctp-api-manifest.json"
    if not manifest_path.exists():
        raise SystemExit(f"Missing generated manifest: {manifest_path}")

    expected = build_manifest(repo_root, sdk_root, version)
    actual = json.loads(manifest_path.read_text(encoding="utf-8"))
    validate_official_abi_coverage(expected["official_structs"], expected["c_abi"])
    validate_cpp_copy_coverage(
        repo_root, resolve_sdk(sdk_root, version), expected["c_abi"]["structs"]
    )
    if api_signature(expected["official"]) != api_signature(actual.get("official", {})):
        raise SystemExit("Manifest drift in section 'official'. Run tools/ctp_api.py generate.")

    if c_abi_signature(expected["c_abi"]) != c_abi_signature(actual.get("c_abi", {})):
        raise SystemExit("Manifest drift in section 'c_abi'. Run tools/ctp_api.py generate.")

    if expected["managed"] != actual.get("managed"):
        raise SystemExit("Manifest drift in section 'managed'. Run tools/ctp_api.py generate.")

    probe_path = repo_root / "NativeBridge" / "generated" / "ctp_bridge_abi_probe.cpp"
    if not probe_path.exists() or probe_path.read_text(encoding="utf-8") != abi_probe_source(expected):
        raise SystemExit("Generated ABI probe is out of date. Run tools/ctp_api.py generate.")

    expected_fields = field_signature(expected["official_structs"])
    if expected_fields != field_signature(actual.get("official_structs", [])):
        raise SystemExit("Manifest drift in section 'official_structs'. Run tools/ctp_api.py generate.")

    if enum_signature(expected["official_enums"]) != enum_signature(actual.get("official_enums", [])):
        raise SystemExit("Manifest drift in section 'official_enums'. Run tools/ctp_api.py generate.")

    if constant_signature(expected["official_constants"]) != constant_signature(actual.get("official_constants", [])):
        raise SystemExit("Manifest drift in section 'official_constants'. Run tools/ctp_api.py generate.")

    if actual.get("sdk_version") != version:
        raise SystemExit(f"Manifest SDK version is {actual.get('sdk_version')!r}, expected {version!r}.")

    field_manifest = json.loads((output_dir / "field-manifest.json").read_text(encoding="utf-8"))
    if field_signature(field_manifest.get("structs", [])) != expected_fields:
        raise SystemExit("Field manifest is out of date. Run tools/ctp_api.py generate.")

    compatibility = json.loads((output_dir / "compatibility-v0.7.1.json").read_text(encoding="utf-8"))
    current = expected["managed"]
    surface_map = {
        "fsharp_md": current["fsharp"]["md_members"],
        "fsharp_trader": current["fsharp"]["trader_members"],
        "csharp_md": current["csharp"]["md_members"],
        "csharp_trader": current["csharp"]["trader_members"],
    }
    missing = {
        surface: sorted(set(names) - set(surface_map.get(surface, [])))
        for surface, names in compatibility.get("members", {}).items()
        if set(names) - set(surface_map.get(surface, []))
    }
    if missing:
        raise SystemExit(f"Compatibility members disappeared: {missing}")

    parity_path = output_dir / "csharp-parity.json"
    if parity_path.exists():
        parity = json.loads(parity_path.read_text(encoding="utf-8"))
        mapped_fsharp: dict[str, set[str]] = {}
        for entry in parity.get("entries", []):
            surface = entry.get("surface")
            if surface not in {"MdClient", "TraderClient"}:
                raise SystemExit(f"C# parity entry has an invalid surface: {entry}")
            fsharp_members = set(current["fsharp"]["md_members"] if surface == "MdClient" else current["fsharp"]["trader_members"])
            csharp_members = set(current["csharp"]["md_members"] if surface == "MdClient" else current["csharp"]["trader_members"])
            if entry.get("status") == "mapped":
                if entry.get("fsharp") not in fsharp_members:
                    raise SystemExit(f"C# parity entry is missing from the F# facade: {entry.get('fsharp')}")
                if entry.get("csharp") not in csharp_members:
                    raise SystemExit(f"C# parity entry is missing from the C# facade: {entry.get('csharp')}")
                mapped_fsharp.setdefault(surface, set()).add(entry["fsharp"])
            elif entry.get("status") == "exception":
                if not entry.get("reason"):
                    raise SystemExit(f"C# parity exception has no reason: {entry}")
            else:
                raise SystemExit(f"C# parity entry has an invalid status: {entry}")

        for exception in parity.get("exceptions", []):
            surface = exception.get("surface")
            reason = exception.get("reason")
            patterns = exception.get("patterns")
            if surface not in {"MdClient", "TraderClient"} or not reason or not patterns:
                raise SystemExit(f"C# parity exception is incomplete: {exception}")
            if not isinstance(patterns, list) or not all(isinstance(pattern, str) for pattern in patterns):
                raise SystemExit(f"C# parity exception patterns must be strings: {exception}")

        for surface, fsharp_names, csharp_names in (
            ("MdClient", current["fsharp"]["md_members"], current["csharp"]["md_members"]),
            ("TraderClient", current["fsharp"]["trader_members"], current["csharp"]["trader_members"]),
        ):
            renamed = mapped_fsharp.get(surface, set())
            missing = set(fsharp_names) - set(csharp_names) - renamed
            covered = set()
            for exception in parity.get("exceptions", []):
                if exception.get("surface") != surface:
                    continue
                for name in missing:
                    if any(fnmatch.fnmatchcase(name, pattern) for pattern in exception["patterns"]):
                        covered.add(name)
            unexplained = sorted(missing - covered)
            if unexplained:
                raise SystemExit(f"Unexplained C# parity omissions for {surface}: {unexplained}")

    print(
        "manifest ok: "
        f"Trader Req={sum(item['name'].startswith('Req') for item in expected['official']['trader']['methods'])}, "
        f"Trader SPI={len(expected['official']['trader']['callbacks'])}, "
        f"MD methods={len(expected['official']['md']['methods'])}, "
        f"MD SPI={len(expected['official']['md']['callbacks'])}, "
        f"official structs={len(expected['official_structs'])}"
    )


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("command", choices=["generate", "check"], nargs="?")
    parser.add_argument(
        "--check",
        action="store_true",
        dest="check_only",
        help="Check generated files without rewriting them; supports 'generate --check'.",
    )
    parser.add_argument("--repo-root", type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument("--sdk-root", type=Path)
    parser.add_argument("--version", default=SDK_VERSION)
    parser.add_argument("--output-dir", type=Path)
    args = parser.parse_args()

    repo_root = args.repo_root.resolve()
    sdk_root = (args.sdk_root or repo_root / "NativeBridge" / "ctp-sdk").resolve()
    output_dir = (args.output_dir or repo_root / "api-manifest").resolve()
    if args.check_only:
        check(repo_root, sdk_root, args.version, output_dir)
    elif args.command == "generate":
        generate(repo_root, sdk_root, args.version, output_dir)
    elif args.command == "check":
        check(repo_root, sdk_root, args.version, output_dir)
    else:
        parser.error("a command is required unless --check is supplied")
    return 0


if __name__ == "__main__":
    sys.exit(main())
