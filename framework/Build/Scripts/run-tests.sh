#!/usr/bin/env bash
set -euo pipefail

# Unity EditMode + PlayMode test runner for framework
# Usage: ./run-tests.sh [test-platform]
# Requires Unity executable in PATH or UNITY_HOME set

UNITY_EXEC="${UNITY_HOME:-/opt/Unity/Editor/Unity}"
PROJECT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../.." && pwd)/framework"
TEST_RESULTS="$PROJECT_PATH/TestResults"

mkdir -p "$TEST_RESULTS"

echo "Running Unity EditMode tests for framework..."

"$UNITY_EXEC" \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -runEditorTests \
  -editorTestsResultFile "$TEST_RESULTS/editmode-results.xml" \
  -logFile "$TEST_RESULTS/editmode-log.txt" \
  -quit

EDIT_EXIT=$?

if [ $EDIT_EXIT -ne 0 ]; then
  echo "[FAIL] EditMode tests failed (exit code: $EDIT_EXIT)"
  cat "$TEST_RESULTS/editmode-log.txt"
  exit $EDIT_EXIT
fi

echo "Running Unity PlayMode tests for framework..."

"$UNITY_EXEC" \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -runPlayModeTests \
  -playModeTestsResultFile "$TEST_RESULTS/playmode-results.xml" \
  -logFile "$TEST_RESULTS/playmode-log.txt" \
  -quit

PLAY_EXIT=$?

if [ $PLAY_EXIT -ne 0 ]; then
  echo "[FAIL] PlayMode tests failed (exit code: $PLAY_EXIT)"
  cat "$TEST_RESULTS/playmode-log.txt"
  exit $PLAY_EXIT
fi

echo "[PASS] All framework tests passed."
exit 0