#!/usr/bin/env python3
"""Check that every declared C ABI function is exported by a bridge library."""

from __future__ import annotations

import argparse
import json
import os
import shutil
import subprocess
from pathlib import Path


def find_dumpbin() -> str | None:
    dumpbin = shutil.which("dumpbin")
    if dumpbin:
        return dumpbin

    vswhere = shutil.which("vswhere")
    if not vswhere:
        candidate = Path(os.environ.get("ProgramFiles(x86)", "C:/Program Files (x86)")) / "Microsoft Visual Studio" / "Installer" / "vswhere.exe"
        if candidate.exists():
            vswhere = str(candidate)

    if vswhere:
        completed = subprocess.run(
            [vswhere, "-latest", "-products", "*", "-requires", "Microsoft.VisualStudio.Component.VC.Tools.x86.x64", "-property", "installationPath"],
            capture_output=True,
            text=True,
            check=False,
        )
        for installation_path in completed.stdout.splitlines():
            tool_root = Path(installation_path) / "VC" / "Tools" / "MSVC"
            candidates = sorted(tool_root.glob("*/bin/Host*x64/x64/dumpbin.exe"), reverse=True)
            if candidates:
                return str(candidates[0])

    return None


def symbols(path: Path) -> set[str]:
    if path.suffix.lower() == ".dll":
        dumpbin = find_dumpbin()
        if not dumpbin:
            raise SystemExit("dumpbin is required to inspect Windows bridge exports")
        completed = subprocess.run([dumpbin, "/EXPORTS", str(path)], capture_output=True, text=True, check=True)
        names = set()
        for line in completed.stdout.splitlines():
            parts = line.split()
            if len(parts) >= 4 and parts[0].isdigit() and parts[1].isdigit():
                names.add(parts[-1])
        return names

    nm = shutil.which("nm")
    if not nm:
        raise SystemExit("nm is required on non-Windows platforms")
    completed = subprocess.run([nm, "-D", "--defined-only", str(path)], capture_output=True, text=True, check=True)
    return {line.split()[-1] for line in completed.stdout.splitlines() if line.split()}


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo-root", type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument("--manifest", type=Path)
    parser.add_argument("libraries", nargs="+")
    args = parser.parse_args()

    repo_root = args.repo_root.resolve()
    manifest_path = (args.manifest or repo_root / "api-manifest" / "ctp-api-manifest.json").resolve()
    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    exported = set()
    for library in args.libraries:
        path = Path(library).resolve()
        if not path.exists():
            raise SystemExit(f"Native bridge library not found: {path}")
        exported.update(symbols(path))

    missing = sorted(item["name"] for item in manifest["c_abi"]["functions"] if item["name"] not in exported)
    if missing:
        raise SystemExit(f"Missing C ABI exports: {missing}")
    print(f"native exports ok: {len(manifest['c_abi']['functions'])} declared functions")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
