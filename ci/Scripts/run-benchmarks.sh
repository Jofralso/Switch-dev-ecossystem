#!/usr/bin/env bash
set -euo pipefail

# Automated benchmark profiling script
# Generates profiling data and regression reports

UNITY_EXEC="${UNITY_HOME:-/opt/Unity/Editor/Unity}"
PROJECT_PATH="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../game/client" && pwd)"
PROFILING_EXPORT="$(cd "$(dirname "${BASH_SOURCE[0]}")/../../profiling/Exports" && pwd)"

mkdir -p "$PROFILING_EXPORT"

echo "Running benchmark scene..."

"$UNITY_EXEC" \
  -batchmode \
  -nographics \
  -projectPath "$PROJECT_PATH" \
  -executeMethod Game.BenchmarkRunner.Run \
  -logFile "$PROFILING_EXPORT/benchmark-log.txt" \
  -quit

echo "Profiling data exported to: $PROFILING_EXPORT"

# Generate performance report
python3 "$(dirname "${BASH_SOURCE[0]}")/../../tools/Profiling/generate_report.py" \
  --input "$PROFILING_EXPORT" \
  --output "$PROFILING_EXPORT/report.md" \
  --baseline "$PROFILING_EXPORT/baseline.json"

echo "[DONE] Benchmark run completed."
