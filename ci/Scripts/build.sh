#!/usr/bin/env bash
set -euo pipefail

# Unity build script for multi-game support
# Usage: ./build.sh <game_name> <target>
# Example: ./build.sh platformer StandaloneWindows64
# Targets: StandaloneWindows64, StandaloneLinux64, StandaloneOSX, Switch

GAME_NAME="${1:-platformer}"
TARGET="${2:-StandaloneWindows64}"
UNITY_EXEC="${UNITY_HOME:-/opt/Unity/Editor/Unity}"
PROJECT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)/games/${GAME_NAME}/client"
OUTPUT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)/games/${GAME_NAME}/build/Artifacts"

mkdir -p "$OUTPUT_PATH"

echo "Building Unity project '$GAME_NAME' for target: $TARGET"
echo "Project path: $PROJECT_PATH"
echo "Output path: $OUTPUT_PATH/$TARGET"

"$UNITY_EXEC" \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -buildTarget "$TARGET" \
  -buildWindows64Player "$OUTPUT_PATH/${TARGET}/game.exe" \
  -logFile "$OUTPUT_PATH/build-${TARGET}.log" \
  -quit

BUILD_EXIT=$?

if [ $BUILD_EXIT -ne 0 ]; then
  echo "[FAIL] Build failed for game '$GAME_NAME' target $TARGET (exit code: $BUILD_EXIT)"
  cat "$OUTPUT_PATH/build-${TARGET}.log"
  exit $BUILD_EXIT
fi

echo "[PASS] Build completed for '$GAME_NAME': $OUTPUT_PATH/$TARGET"
exit 0