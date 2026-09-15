#!/usr/bin/env python3
"""Compare C ABI probe layouts with the compiled F# interop structs."""

from __future__ import annotations

import argparse
import json
import re
import subprocess
import tempfile
from pathlib import Path


def pascal_case(value: str) -> str:
    return "".join(part[:1].upper() + part[1:] for part in value.split("_"))


def native_type_name(c_name: str) -> str:
    value = c_name.removeprefix("ctp_")
    if value == "cfmmc_trading_account_token":
        return "NativeCFMMCTradingAccountToken"
    return "Native" + pascal_case(value)


def compact_name(value: str) -> str:
    return re.sub(r"[^a-z0-9]", "", value.lower())


def managed_field_key(value: str) -> str:
    aliases = {"exchangerateresponse": "exchangerate"}
    return aliases.get(compact_name(value), compact_name(value))


MANAGED_TYPE_ALIASES = {
    # These payloads have separate declarations in the MD and Trader bridge
    # files but share one C ABI structure and therefore one required layout.
    "ctp_depth_market_data": ["NativeTraderDepthMarketData"],
    "ctp_fens_user_info": ["NativeMdFensUserInfo"],
    "ctp_for_quote_rsp": ["NativeMdForQuoteRsp"],
}


def read_probe(probe_path: Path) -> dict:
    completed = subprocess.run([str(probe_path)], capture_output=True, text=True, check=False)
    if completed.returncode != 0:
        raise SystemExit(f"ABI probe failed with exit code {completed.returncode}: {completed.stderr}")
    return json.loads(completed.stdout)


def compiled_layouts(assembly_path: Path, type_names: list[str]) -> dict[str, dict]:
    # The temporary script is deliberately generated outside the repository. It only
    # reflects the already-built assembly and emits a stable, dependency-free format.
    type_literals = "; ".join(f'"{name}"' for name in type_names)
    script = f'''#r @"{assembly_path}"
open System
open System.Reflection
open System.Runtime.InteropServices

let assembly = Assembly.LoadFrom(@"{assembly_path}")
let flags = BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic
let typeNames = [ {type_literals} ]

for name in typeNames do
    let typeInfo = assembly.GetType("Ctp.Net.Next.Bridge." + name, true)
    let fields = typeInfo.GetFields(flags)
    let offsets = fields |> Seq.map (fun field -> Marshal.OffsetOf(typeInfo, field.Name).ToInt64())
    let descriptors =
        fields
        |> Seq.map (fun field ->
            let marshal =
                field.GetCustomAttributes(typeof<MarshalAsAttribute>, false)
                |> Array.tryHead
                |> Option.map (fun value -> (value :?> MarshalAsAttribute).SizeConst)
                |> Option.defaultValue -1
            String.Format("{{0}}:{{1}}:{{2}}", field.Name, marshal, field.FieldType.FullName))
    printfn "%s\\t%d\\t%s\\t%s" name (Marshal.SizeOf(typeInfo)) (String.Join(",", offsets)) (String.Join("|", descriptors))
'''
    with tempfile.NamedTemporaryFile("w", suffix=".fsx", encoding="utf-8") as script_file:
        script_file.write(script)
        script_file.flush()
        completed = subprocess.run(
            ["dotnet", "fsi", "--nologo", script_file.name],
            capture_output=True,
            text=True,
            check=False,
        )
    if completed.returncode != 0:
        raise SystemExit(f"Managed ABI reflection failed: {completed.stderr or completed.stdout}")

    result = {}
    for line in completed.stdout.splitlines():
        parts = line.split("\t")
        if len(parts) != 4:
            continue
        offsets = [] if not parts[2] else [int(value) for value in parts[2].split(",")]
        fields = []
        if parts[3]:
            for descriptor in parts[3].split("|"):
                field_name, array_length, field_type = descriptor.split(":", 2)
                fields.append(
                    {
                        "name": field_name,
                        "array_length": int(array_length),
                        "type": field_type,
                    }
                )
        result[parts[0]] = {
            "size": int(parts[1]),
            "offsets": offsets,
            "fields": fields,
        }
    return result


def find_assembly(repo_root: Path) -> Path:
    candidates = [
        repo_root / "Ctp.Net" / "bin" / "Debug" / "net10.0" / "Ctp.Net.Next.dll",
        repo_root / "Ctp.Net" / "bin" / "Release" / "net10.0" / "Ctp.Net.Next.dll",
    ]
    for candidate in candidates:
        if candidate.exists():
            return candidate
    raise SystemExit("Managed assembly not found. Build Ctp.Net/Ctp.Net.fsproj first.")


def find_probe(repo_root: Path) -> Path:
    candidates = [repo_root / "NativeBridge" / "build" / "ctp_bridge_abi_probe"]
    candidates.extend(sorted((repo_root / "NativeBridge" / "build").rglob("ctp_bridge_abi_probe")))
    candidates.extend(sorted((repo_root / "NativeBridge" / "build").rglob("ctp_bridge_abi_probe.exe")))
    for candidate in candidates:
        if candidate.exists():
            return candidate
    raise SystemExit("ABI probe not found. Build NativeBridge first.")


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo-root", type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument("--manifest", type=Path)
    parser.add_argument("--probe", type=Path)
    parser.add_argument("--assembly", type=Path)
    args = parser.parse_args()

    repo_root = args.repo_root.resolve()
    manifest_path = (args.manifest or repo_root / "api-manifest" / "ctp-api-manifest.json").resolve()
    probe_path = (args.probe or find_probe(repo_root)).resolve()
    assembly_path = (args.assembly or find_assembly(repo_root)).resolve()

    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    expected_probe = read_probe(probe_path)
    c_abi = {item["name"]: item for item in manifest["c_abi"]["structs"]}
    type_names = []
    for name in sorted(c_abi):
        type_names.append(native_type_name(name))
        type_names.extend(MANAGED_TYPE_ALIASES.get(name, []))
    managed = compiled_layouts(assembly_path, type_names)

    missing = sorted(set(type_names) - set(managed))
    if missing:
        raise SystemExit(f"Missing F# interop types: {missing}")

    probe_by_name = {item["name"]: item for item in expected_probe["structs"]}
    for c_name in sorted(c_abi):
        native = probe_by_name[c_name]
        expected_fields = c_abi[c_name]["fields"]
        for fsharp_name in [native_type_name(c_name), *MANAGED_TYPE_ALIASES.get(c_name, [])]:
            managed_layout = managed[fsharp_name]
            native_offsets = [field["offset"] for field in native["fields"]]
            if managed_layout["size"] != native["size"]:
                raise SystemExit(
                    f"Size drift for {c_name}/{fsharp_name}: "
                    f"native={native['size']} managed={managed_layout['size']}"
                )
            if managed_layout["offsets"] != native_offsets:
                raise SystemExit(
                    f"Offset drift for {c_name}/{fsharp_name}: "
                    f"native={native_offsets} managed={managed_layout['offsets']}"
                )

            managed_fields = managed_layout["fields"]
            if len(managed_fields) != len(expected_fields):
                raise SystemExit(
                    f"Field count drift for {c_name}/{fsharp_name}: "
                    f"native={len(expected_fields)} managed={len(managed_fields)}"
                )

            # Callback tables use deliberately different naming conventions
            # (on_rsp_* in C versus OnRsp* in F#), so their layout is checked by
            # size, field count, and offsets only.
            if c_name.endswith("_spi"):
                continue

            for expected, actual in zip(expected_fields, managed_fields):
                if compact_name(expected["name"]) != managed_field_key(actual["name"]):
                    raise SystemExit(
                        f"Field name drift for {c_name}/{fsharp_name}: "
                        f"native={expected['name']} managed={actual['name']}"
                    )

                expected_array_length = expected.get("array_length")
                actual_array_length = actual["array_length"] if actual["array_length"] >= 0 else None
                if expected_array_length != actual_array_length:
                    raise SystemExit(
                        f"Field array capacity drift for {c_name}.{expected['name']}/{fsharp_name}.{actual['name']}: "
                        f"native={expected_array_length} managed={actual_array_length}"
                    )

                if expected_array_length is not None:
                    expected_type = "System.Byte[]"
                elif expected.get("type") == "char":
                    expected_type = "System.Byte"
                elif expected.get("type") == "int32_t":
                    expected_type = "System.Int32"
                elif expected.get("type") == "int16_t":
                    expected_type = "System.Int16"
                elif expected.get("type") == "double":
                    expected_type = "System.Double"
                else:
                    expected_type = None
                if expected_type is not None and actual["type"] != expected_type:
                    raise SystemExit(
                        f"Field type drift for {c_name}.{expected['name']}/{fsharp_name}.{actual['name']}: "
                        f"native={expected_type} managed={actual['type']}"
                    )

    print(f"managed ABI ok: {len(c_abi)} C ABI structs match F# interop layouts")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
