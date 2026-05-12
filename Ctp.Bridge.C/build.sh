#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
BUILD_DIR="${SCRIPT_DIR}/build"
DEFAULT_CTP_SDK_ROOT="${SCRIPT_DIR}/ctp-sdk"
CTP_SDK_ROOT="${CTP_SDK_ROOT:-${DEFAULT_CTP_SDK_ROOT}}"
CTP_SDK_VERSION="${CTP_SDK_VERSION:-}"

cmake -S "${SCRIPT_DIR}" -B "${BUILD_DIR}" -DCTP_SDK_ROOT="${CTP_SDK_ROOT}" -DCTP_SDK_VERSION="${CTP_SDK_VERSION}"
cmake --build "${BUILD_DIR}" --parallel
