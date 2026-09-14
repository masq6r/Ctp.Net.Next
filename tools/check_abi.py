#!/usr/bin/env python3
"""Validate the C ABI layout probe against the checked-in C ABI inventory."""

from __future__ import annotations

import argparse
import json
import subprocess
from pathlib import Path


def main() -> int:
    parser = argparse.ArgumentParser()
    parser.add_argument("--repo-root", type=Path, default=Path(__file__).resolve().parents[1])
    parser.add_argument("--manifest", type=Path)
    parser.add_argument("--probe", type=Path)
    args = parser.parse_args()

    repo_root = args.repo_root.resolve()
    manifest_path = (args.manifest or repo_root / "api-manifest" / "ctp-api-manifest.json").resolve()
    probe_path = (args.probe or repo_root / "NativeBridge" / "build" / "ctp_bridge_abi_probe").resolve()
    if not probe_path.exists() and probe_path.suffix != ".exe":
        windows_probe = probe_path.with_suffix(".exe")
        if windows_probe.exists():
            probe_path = windows_probe

    manifest = json.loads(manifest_path.read_text(encoding="utf-8"))
    completed = subprocess.run([str(probe_path)], capture_output=True, text=True, check=False)
    if completed.returncode != 0:
        raise SystemExit(f"ABI probe failed with exit code {completed.returncode}: {completed.stderr}")
    actual = json.loads(completed.stdout)
    expected_structs = manifest["c_abi"]["structs"]
    actual_by_name = {item["name"]: item for item in actual["structs"]}
    expected_names = {item["name"] for item in expected_structs}
    actual_names = set(actual_by_name)
    if expected_names != actual_names:
        raise SystemExit(f"ABI struct name drift: missing={sorted(expected_names - actual_names)}, extra={sorted(actual_names - expected_names)}")

    for expected in expected_structs:
        actual_item = actual_by_name[expected["name"]]
        expected_fields = [field["name"] for field in expected["fields"]]
        actual_fields = [field["name"] for field in actual_item["fields"]]
        if expected_fields != actual_fields:
            raise SystemExit(f"ABI field drift for {expected['name']}: expected={expected_fields}, actual={actual_fields}")
        if actual_item["size"] <= 0 or actual_item["alignment"] <= 0:
            raise SystemExit(f"Invalid ABI layout for {expected['name']}: {actual_item}")
        offsets = [field["offset"] for field in actual_item["fields"]]
        if offsets != sorted(offsets):
            raise SystemExit(f"Non-monotonic ABI offsets for {expected['name']}: {offsets}")

    print(f"ABI probe ok: {len(actual_names)} C ABI structs and {sum(len(item['fields']) for item in actual['structs'])} fields")
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
