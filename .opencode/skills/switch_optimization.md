# Switch Optimization Skill

## Responsibilities
- detect GC allocations
- reduce draw calls
- analyze shader complexity
- identify CPU bottlenecks
- optimize texture memory
- validate batching opportunities
- reduce overdraw
- profile Unity frame timings

## Performance Targets
- stable 30 FPS handheld
- stable 60 FPS docked
- memory budget under 3GB
- frame budget: 33ms (handheld), 16ms (docked)

## Rules
- avoid LINQ in gameplay loops
- avoid reflection in runtime systems
- avoid per-frame allocations
- prefer object pooling
- prefer async loading
- use compressed textures
- prefer baked lighting when possible
- use GPU instancing for repeated geometry
- limit transparent draw calls

## Validation
- reject frame spikes above 33ms
- reject excessive VRAM usage (>3GB)
- reject textures larger than 2048x2048 without justification
- reject unnecessary Update() usage
- warn on missing LOD groups
- warn on missing occlusion culling
