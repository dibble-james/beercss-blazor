#!/usr/bin/env bash
# Installs the .NET 10 SDK used by global.json into ./.dotnet (the system install is untouched).
set -euo pipefail

INSTALL_DIR=".dotnet"
CHANNEL="10.0"
QUALITY="ga"

INSTALL_SCRIPT="$(mktemp "${TMPDIR:-/tmp}/dotnet-install.XXXXXX")"
trap 'rm -f "$INSTALL_SCRIPT"' EXIT

curl -fsSL https://dot.net/v1/dotnet-install.sh -o "$INSTALL_SCRIPT"
bash "$INSTALL_SCRIPT" --channel "$CHANNEL" --quality "$QUALITY" --install-dir "$INSTALL_DIR"

echo "Done. SDK $("$INSTALL_DIR/dotnet" --version) installed in $INSTALL_DIR"
