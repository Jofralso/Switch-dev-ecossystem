# Shader Analysis Skill

## Responsibilities
- analyze shader complexity and instruction count
- identify expensive operations (sin, cos, pow, divide)
- recommend shader LOD groups
- validate mobile-compatible shader variants
- detect missing LOD/fade distance on transparent shaders
- batch shader property blocks for GPU instancing

## Switch Constraints
- prefer Shader Model 5.0 compatible shaders
- avoid compute shaders where possible
- limit texture samples per pass: max 8
- limit varyings: max 16
- prefer half precision (fp16) over float
- avoid branching in fragment shaders
- minimize render target switches

## Optimization
- use Shader Variant Stripping
- generate stripped shader builds per platform
- prefer mobile-lit/universal over standard
- eliminate unused shader features
- keyword stripping for unused variants
