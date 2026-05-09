# ECS Analysis Skill

## Responsibilities
- identify GameObject-heavy systems suitable for ECS migration
- analyze cache coherence of data-oriented systems
- detect inefficient ComponentData access patterns
- recommend Archetype layouts for optimal chunk utilization
- validate System ordering for deterministic updates
- profile ECS system cost vs GameObject equivalent

## Migration Candidates
- particle systems (VFX Graph with ECS)
- large numbers of interactable objects
- physics triggers and sensors
- AI agent flocks / crowds
- procedural generation systems
- projectile and hit registration

## Rules
- ECS systems must be deterministic
- avoid managed components in hot paths
- prefer IJobEntity over Entities.ForEach
- use SystemAPI.Query in modern Unity ECS
- batch structural changes with EntityCommandBuffer
