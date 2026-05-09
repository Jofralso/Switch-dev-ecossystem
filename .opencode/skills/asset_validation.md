# Asset Validation Skill

## Responsibilities
- validate texture resolution and compression settings
- check atlas grouping efficiency
- ensure LOD groups exist on complex meshes
- verify audio compression and sample rate
- detect oversized or unused assets
- validate import settings match platform requirements

## Texture Rules (Switch)
- max resolution: 2048x2048 (exceptions approved per asset)
- compression: ASTC 6x6 (RGB), ASTC 8x8 (RGBA)
- no mipmaps on UI textures
- enable mipmap streaming on large textures
- power-of-two dimensions required
- no Read/Write enabled on runtime textures

## Mesh Rules
- vertex limit: 65k per mesh
- LOD 0: full detail
- LOD 1: 50% triangles
- LOD 2: 25% triangles
- LOD 3: 10% triangles (cull at distance)
- enable mesh compression
- prefer 16-bit indices when possible

## Audio Rules
- format: Vorbis/ADPCM for Switch
- sample rate: 44100Hz max
- force to mono for SFX
- streaming for music, compressed in memory for SFX
- background loading for all audio
