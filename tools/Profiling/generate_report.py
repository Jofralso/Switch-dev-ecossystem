#!/usr/bin/env python3
"""Generate performance regression report from profiling exports."""

import argparse
import json
import os
from datetime import datetime


def load_profiling_data(input_dir: str) -> dict:
    """Load profiling data from exported JSON files."""
    data = {"frames": [], "memory": {}, "summary": {}}
    frames_path = os.path.join(input_dir, "frames.json")
    memory_path = os.path.join(input_dir, "memory.json")

    if os.path.exists(frames_path):
        with open(frames_path) as f:
            data["frames"] = json.load(f)

    if os.path.exists(memory_path):
        with open(memory_path) as f:
            data["memory"] = json.load(f)

    return data


def calculate_summary(data: dict) -> dict:
    """Calculate performance summary statistics."""
    frames = data.get("frames", [])
    if not frames:
        return {"error": "No frame data available"}

    frame_times = [f.get("time_ms", 0) for f in frames]
    gc_allocations = [f.get("gc_alloc_mb", 0) for f in frames]
    draw_calls = [f.get("draw_calls", 0) for f in frames]

    return {
        "total_frames": len(frames),
        "avg_frame_time_ms": round(sum(frame_times) / len(frame_times), 2),
        "max_frame_time_ms": round(max(frame_times), 2),
        "p95_frame_time_ms": round(sorted(frame_times)[int(len(frame_times) * 0.95)], 2),
        "avg_gc_alloc_mb": round(sum(gc_allocations) / len(gc_allocations), 4),
        "max_gc_alloc_mb": round(max(gc_allocations), 4),
        "avg_draw_calls": round(sum(draw_calls) / len(draw_calls), 1),
        "max_draw_calls": max(draw_calls),
    }


def check_regressions(
    summary: dict, baseline: dict | None
) -> list[dict]:
    """Compare against baseline and flag regressions."""
    regressions = []
    if not baseline:
        return regressions

    thresholds = {
        "avg_frame_time_ms": 0.05,   # 5% regression
        "max_frame_time_ms": 0.10,
        "avg_gc_alloc_mb": 0.10,
        "avg_draw_calls": 0.05,
    }

    for key, threshold in thresholds.items():
        if key in summary and key in baseline:
            if baseline[key] > 0:
                change = (summary[key] - baseline[key]) / baseline[key]
                if change > threshold:
                    regressions.append(
                        {
                            "metric": key,
                            "baseline": baseline[key],
                            "current": summary[key],
                            "change_pct": round(change * 100, 1),
                            "severity": "high" if change > threshold * 2 else "medium",
                        }
                    )

    return regressions


def generate_report(data: dict, output_path: str, baseline_path: str | None):
    """Generate the full markdown performance report."""
    summary = calculate_summary(data)
    baseline = None
    if baseline_path and os.path.exists(baseline_path):
        with open(baseline_path) as f:
            baseline = json.load(f)

    regressions = check_regressions(summary, baseline)

    lines = []
    lines.append("# Performance Report")
    lines.append(f"Generated: {datetime.now().isoformat()}")
    lines.append("")

    # Summary table
    lines.append("## Summary")
    lines.append("")
    lines.append("| Metric | Value |")
    lines.append("|--------|-------|")
    for key, value in summary.items():
        lines.append(f"| {key} | {value} |")

    lines.append("")

    # Regressions
    if regressions:
        lines.append("## Regressions Detected")
        lines.append("")
        lines.append("| Metric | Baseline | Current | Change | Severity |")
        lines.append("|--------|----------|---------|--------|----------|")
        for r in regressions:
            lines.append(
                f"| {r['metric']} | {r['baseline']} | {r['current']} "
                f"| +{r['change_pct']}% | {r['severity']} |"
            )
    else:
        lines.append("## Regressions")
        lines.append("")
        lines.append("No regressions detected.")

    lines.append("")

    # Memory report
    memory = data.get("memory", {})
    if memory:
        lines.append("## Memory Usage")
        lines.append("")
        lines.append("| Category | Size (MB) |")
        lines.append("|----------|-----------|")
        for category, size in memory.items():
            lines.append(f"| {category} | {size} |")

    report = "\n".join(lines)

    with open(output_path, "w") as f:
        f.write(report)

    print(f"Report written to {output_path}")

    # Update baseline if this is a new baseline run
    if not baseline_path:
        baseline_path = os.path.join(
            os.path.dirname(output_path), "baseline.json"
        )
        with open(baseline_path, "w") as f:
            json.dump(summary, f, indent=2)
        print(f"Baseline saved to {baseline_path}")


def main():
    parser = argparse.ArgumentParser(description="Generate performance report")
    parser.add_argument("--input", required=True, help="Profiling export directory")
    parser.add_argument("--output", required=True, help="Report output path")
    parser.add_argument("--baseline", help="Optional baseline JSON file")
    args = parser.parse_args()

    data = load_profiling_data(args.input)
    generate_report(data, args.output, args.baseline)


if __name__ == "__main__":
    main()
