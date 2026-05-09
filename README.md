# Switch Game Framework

A unified framework for developing multiple Nintendo Switch games using Unity, with shared infrastructure, CI/CD pipelines, and platform optimization tools.

## Overview

This repository provides a multi-game ecosystem where each game can be developed independently while sharing:
- A common Unity framework with reusable systems
- Standardized build and test pipelines
- Nintendo Switch optimization tools
- Shared CI/CD workflows
- Cross-game profiling and analytics

## Directory Structure

```
switch-game/
├── framework/              # Shared Unity framework (Core, Audio, Input, etc.)
├── games/                  # Individual game projects
│   ├── platformer/         # 2D platformer game (migrated from original)
│   ├── rpg/                # RPG-style game (stub)
│   └── shooter/            # Shooter-style game (stub)
├── ci/                     # CI/CD pipelines and scripts
├── tools/                  # Developer tooling (profiling, automation)
├── docs/                   # Documentation
├── profiling/              # Cross-game profiling data
├── .github/                # GitHub Actions workflows
├── docker-compose.yml      # Development services (Postgres, Grafana, etc.)
├── opencode.json           # AI agent configuration
└── setup.sh                # Repository bootstrap script
```

## Games

### Platformer (`games/platformer/`)
A 2D cooperative platformer featuring:
- 4-player local co-op
- Collectibles, puzzles, and hazards
- Dynamic resolution scaling for Switch
- Performance monitoring
- Original game migrated from the repository's initial state

### RPG (`games/rpg/`)
*Stub* - A role-playing game foundation featuring:
- Party-based combat system
- Dialogue and quest framework
- Inventory management
- Character progression systems

### Shooter (`games/shooter/`)
*Stub* - A multiplayer shooter foundation featuring:
- 16-player online matches
- Multiple game modes (TDM, CTF, etc.)
- Map voting system
- Scoreboard and matchmaking

## Framework

The `framework/` directory contains reusable Unity systems:

### Core Systems
- **Framework.Core**: Base classes, interfaces, and services
- **Framework.Audio**: Audio management system
- **Framework.Input**: Input abstraction layer
- **Framework.Platform**: Platform-specific code (Switch optimization)
- **Framework.Networking**: Network transport abstractions
- **Framework.Build**: Shared build pipeline scripts

### Key Features
- Performance monitoring with Switch-appropriate thresholds
- Dynamic resolution scaling for handheld/docked modes
- Audio manager with music/SFX/ambient channels
- Input system supporting keyboard and gamepad
- Service locator pattern for dependency injection
- Assembly definition files for clear dependency boundaries

## Getting Started

### Prerequisites
- Unity 2021.3 LTS or later
- Nintendo Switch SDK (registered developers only)
- Docker and Docker Compose
- Git
- .NET 6.0+ (for CI scripts)

### Setup
1. Clone the repository:
   ```bash
   git clone https://github.com/jofralso/switch-game-framework.git
   cd switch-game-framework
   ```

2. Run the bootstrap script:
   ```bash
   ./setup.sh
   ```

3. Start development services:
   ```bash
   docker compose up -d
   ```

4. Open in Unity:
   - Platformer: `games/platformer/client/`
   - RPG: `games/rpg/client/`
   - Shooter: `games/shooter/client/`

## Development Workflow

### Building a Game
```bash
# Build platformer for Windows
./ci/Scripts/build.sh platformer StandaloneWindows64

# Build platformer for Switch (requires Switch SDK)
./ci/Scripts/build.sh platformer Switch
```

### Running Tests
```bash
# Run tests for platformer
./ci/Scripts/run-tests.sh platformer

# Run tests for RPG
./ci/Scripts/run-tests.sh rpg
```

### Profiling and Benchmarks
```bash
# Run benchmark for platformer
./ci/Scripts/run-benchmarks.sh platformer

# Profiling data appears in: profiling/Exports/
# Reports generated in: profiling/Reports/
```

## CI/CD Pipeline

The repository uses GitHub Actions for continuous integration:

### Workflows
- **Unity CI** (`.github/workflows/unity-ci.yml`): Builds, tests, and profiles all games on push/PR
- **Platform-specific workflows**: Can be added for release branches

### Stages
1. **Lint**: Static analysis and formatting checks
2. **Test**: EditMode and PlayMode unit tests for each game
3. **Profiling**: Performance benchmarking and regression detection
4. **Build**: Standalone builds for Windows/Linux (Switch builds require manual trigger)

## Nintendo Switch Optimization

The framework includes Switch-specific optimizations:

### Performance Targets
| Metric | Handheld | Docked |
|--------|----------|--------|
| Frame rate | 30 FPS | 60 FPS |
| Frame budget | 33ms | 16ms |
| Draw calls | < 80 | < 120 |
| VRAM | < 2.5GB | < 3GB |
| Resolution | 720p dynamic | 1080p dynamic |

### Optimization Systems
- **DynamicResolution.cs**: Automatic resolution scaling based on frame time
- **PerformanceMonitor.cs**: Real-time performance tracking with alerts
- **Asset validation**: Ensures ASTC texture compression, proper mesh settings
- **Build pipeline**: Optimized for Switch deployment

## Adding a New Game

To add a new game to the ecosystem:

1. Create a new directory under `games/`:
   ```bash
   cp -r games/platformer/ games/mynewgame/
   ```

2. Update namespaces and references:
   - Replace `Game.*` namespaces with `MyNewGame.*`
   - Update assembly definition files
   - Replace game-specific assets and code

3. Add to CI workflows (if needed):
   - Update `.github/workflows/unity-ci.yml` to include your game
   - Or rely on the default workflow that builds all games

4. Develop your game:
   - Use framework systems where appropriate (`Framework.Audio`, `Framework.Input`, etc.)
   - Implement game-specific systems in your game's directories
   - Follow the existing patterns for client/server/shared separation

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

- Built with Unity Engine
- Nintendo Switch development tools and guidelines
- Open-source Unity packages and assets