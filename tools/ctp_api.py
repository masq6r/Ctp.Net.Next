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
GENERATOR_VERSION = 1


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


def check(repo_root: Path, sdk_root: Path, version: str, output_dir: Path) -> None:
    manifest_path = output_dir / "ctp-api-manifest.json"
    if not manifest_path.exists():
        raise SystemExit(f"Missing generated manifest: {manifest_path}")

    expected = build_manifest(repo_root, sdk_root, version)
    actual = json.loads(manifest_path.read_text(encoding="utf-8"))
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
