# Profiling Skill

## Responsibilities
- analyze Unity Profiler exports
- detect CPU/GPU hot paths
- compare build performance across versions
- benchmark core game systems
- identify memory leaks and GC pressure
- generate performance regression reports

## Tools
- Unity Profiler
- RenderDoc (GPU debugging)
- Arm Mobile Studio (Switch GPU profiling)
- Perfetto (system tracing)
- Custom Benchmark .unity scene

## Analysis Points
- frame time breakdown (scripting, rendering, physics, UI)
- GC allocation rate per frame
- draw calls and batches
- VRAM usage by category (textures, meshes, RTs)
- asset load times
- physics timestep cost

## Output
- performance report in docs/profiling/
- regression alerts in CI
- flame graphs for CPU spikes
- memory snapshot diffs between builds
