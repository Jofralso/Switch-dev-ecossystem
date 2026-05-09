# Unity Architecture Skill

## Goals
- maintain clean architecture
- reduce coupling
- improve testability
- prepare for console certification
- enable platform abstraction

## Patterns
- composition over inheritance
- dependency injection via Zenject / VContainer
- service abstraction with interfaces
- event-driven systems using C# events / UnityEvents
- ECS compatibility via Entities package
- ScriptableObject-based game data

## Structural Rules
- no circular dependencies between assemblies
- gameplay code must not reference editor code
- presentation layer separated from logic layer
- all public APIs documented
- interfaces in shared assembly, implementations in client

## Avoid
- god objects (MonoBehaviours > 300 lines)
- singleton abuse (max 3 in project)
- scene dependency chains (use Addressables)
- runtime FindObjectOfType() / GameObject.Find()
- hidden serialization coupling via [SerializeField] on private fields
- direct SceneManager.LoadScene in gameplay code

## Assembly Definitions
- Game.Core (shared interfaces, enums, models)
- Game.Engine (Unity-dependent systems)
- Game.Gameplay (player, combat, progression)
- Game.Audio (FMOD wrapper)
- Game.UI (presentation layer)
- Game.Editor (editor tools, inspectors)
