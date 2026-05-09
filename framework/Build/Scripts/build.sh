#!/usr/bin/env bash
set -euo pipefail

# Unity build script for framework
# Usage: ./build.sh <target>
# Targets: StandaloneWindows64, StandaloneLinux64, StandaloneOSX, Switch

TARGET="${1:-StandaloneWindows64}"
UNITY_EXEC="${UNITY_HOME:-/opt/Unity/Editor/Unity}"
PROJECT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../.." && pwd)/framework"
OUTPUT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../../game/build/Artifacts" && pwd)"

mkdir -p "$OUTPUT_PATH"

echo "Building Unity framework for target: $TARGET"
echo "Project path: $PROJECT_PATH"
echo "Output path: $OUTPUT_PATH/$TARGET"

"$UNITY_EXEC" \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -buildTarget "$TARGET" \
  -buildWindows64Player "$OUTPUT_PATH/${TARGET}/framework.exe" \
  -logFile "$OUTPUT_PATH/build-${TARGET}.log" \
  -quit

BUILD_EXIT=$?

if [ $BUILD_EXIT -ne 0 ]; then
  echo "[FAIL] Build failed for target $TARGET (exit code: $BUILD_EXIT)"
  cat "$OUTPUT_PATH/build-${TARGET}.log"
  exit $BUILD_EXIT
fi

echo "[PASS] Build completed: $OUTPUT_PATH/$TARGET"
exit 0