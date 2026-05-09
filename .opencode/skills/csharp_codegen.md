# C# Code Generation Skill

## Rules
- generate idiomatic modern C# (latest supported by Unity)
- prefer readonly structs for data containers
- use nullable reference types where applicable
- generate XML docs for all public APIs
- follow Microsoft .NET naming conventions
- avoid dynamic/reflection in hot paths
- use Span<T> and Memory<T> for buffer processing
- prefer structs for small data types (< 16 bytes)
- implement IDisposable for unmanaged resources
- use async/await with ConfigureAwait(false) for library code

## Patterns
- Factory for object creation
- Strategy for interchangeable algorithms
- Observer for event-driven communication
- Command for input buffering
- State for gameplay state machines
- Object Pool for frequently created/destroyed objects

## Code Style
- file-scoped namespaces
- implicit usings in modern projects
- expression-bodied members where readable
- pattern matching over casting
- primary constructors where appropriate
