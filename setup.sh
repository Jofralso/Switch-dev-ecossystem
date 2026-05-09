#!/usr/bin/env bash
set -euo pipefail

PROJECT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$PROJECT_DIR"

echo "================================================"
echo "  ForgeSwitchArchitect — Project Bootstrap"
echo "================================================"
echo ""

# --- Step 1: Create OpenCode directory structure ---
echo "[1/5] Creating OpenCode directory structure..."
mkdir -p .opencode/{agents,skills,memory,mcp}
mkdir -p framework/{Core/{Runtime,Editor},Audio,Input,Platform/Switch,Networking,Build/{Scripts,Config},Tests,UI}
mkdir -p games/platformer/{client/{Scripts/{Core,Gameplay,Player,UI,Data,Editor},Art,Audio,Prefabs,Resources,Scenes,StreamingAssets,UI},server/{Src,Tests,Config},shared/{Data,Networking,Utils},assets/{Source,Exports},tests/{EditMode,PlayMode},build/{Artifacts,Config,Scripts},docs/{Architecture,Switch,Workflows}}
mkdir -p games/rpg/{client/{Scripts,Art,Audio,Prefabs,Resources,Scenes,StreamingAssets,UI},server/{Src,Tests,Config},shared/{Data,Networking,Utils},assets,tests,build,docs}
mkdir -p games/shooter/{client/{Scripts,Art,Audio,Prefabs,Resources,Scenes,StreamingAssets,UI},server:{Src,Tests,Config},shared:{Data,Networking,Utils},assets,tests,build,docs}
mkdir -p ci/{Config,Scripts,Workflows}
mkdir -p tools/{Automation,Profiling,Scripts}
mkdir -p profiling/{Exports,Reports}
mkdir -p docs/{Architecture,Framework,Switch,Workflows}
mkdir -p .github/workflows
echo "  Done."

# --- Step 2: Install MCP servers ---
echo "[2/5] Installing recommended MCP servers..."
npm install -g \
  @modelcontextprotocol/server-filesystem \
  @modelcontextprotocol/server-git \
  @modelcontextprotocol/server-github \
  @modelcontextprotocol/server-postgres \
  @modelcontextprotocol/server-playwright \
  @modelcontextprotocol/server-docker 2>/dev/null || {
  echo "  [WARN] npm global install failed. Install manually:"
  echo "    npm install -g @modelcontextprotocol/server-{filesystem,git,github,postgres,playwright,docker}"
}
echo "  Done."

# --- Step 3: Configure environment variables ---
echo "[3/5] Checking environment variables..."
MISSING=0

if [ -z "${GITHUB_TOKEN:-}" ]; then
  echo "  [WARN] GITHUB_TOKEN is not set"
  MISSING=$((MISSING + 1))
fi

if [ -z "${OPENAI_API_KEY:-}" ]; then
  echo "  [WARN] OPENAI_API_KEY is not set"
  MISSING=$((MISSING + 1))
fi

if [ -z "${ANTHROPIC_API_KEY:-}" ]; then
  echo "  [WARN] ANTHROPIC_API_KEY is not set"
  MISSING=$((MISSING + 1))
fi

if [ $MISSING -gt 0 ]; then
  echo ""
  echo "  Set missing variables in your shell profile:"
  echo "    export GITHUB_TOKEN=your_github_token"
  echo "    export OPENAI_API_KEY=your_openai_key"
  echo "    export ANTHROPIC_API_KEY=your_anthropic_key"
fi
echo "  Done."

# --- Step 4: Initialize git repository ---
echo "[4/5] Initializing git repository..."
if [ ! -d .git ]; then
  git init
  git checkout -b main
  echo "  Done."
else
  echo "  Already initialized."
fi

# --- Step 5: Verify OpenCode agent configuration ---
echo "[5/5] Verifying agent configuration..."
if [ -f .opencode/agents/forge-switch-architect.yaml ]; then
  echo "  Agent config found: .opencode/agents/forge-switch-architect.yaml"
fi
if [ -f .opencode/mcp.json ]; then
  echo "  MCP config found: .opencode/mcp.json"
fi
echo "  Done."

echo ""
echo "================================================"
echo "  Bootstrap complete!"
echo ""
echo "  Next steps:"
echo "    1. Start Docker services:  docker compose up -d"
echo "    2. Run the agent:          opencode run ForgeSwitchArchitect --task \"<task>\""
echo "    3. Open in Unity:"
echo "       - Platformer: games/platformer/client/"
echo "       - RPG: games/rpg/client/"
echo "       - Shooter: games/shooter/client/"
echo "================================================"