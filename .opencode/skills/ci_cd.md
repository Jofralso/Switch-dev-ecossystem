# CI/CD Skill

## Responsibilities
- optimize GitHub Actions pipelines
- cache Unity Library directory across runs
- generate build artifacts for each platform
- publish nightly builds to internal channel
- run automated test suites on PRs
- enforce code quality gates

## Pipeline Stages
1. lint: static analysis, code style
2. test: EditMode + PlayMode tests
3. build: standalone Windows + Linux
4. profile: automated benchmark scene
5. deploy: artifact upload + release notes

## Caching Strategy
- Library folder keyed by Assets hash
- NuGet packages for .NET projects
- npm cache for MCP server CI
- Docker layer caching for container builds

## Quality Gates
- all tests must pass
- zero new analyzer warnings
- no GC allocations in hot path tests
- frame time regression < 5% from baseline
- memory usage regression < 10% from baseline
