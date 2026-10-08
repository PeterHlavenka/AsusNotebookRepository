# Visentio Codex Usage

Workspace installation/support plugin for the separate local Windows miniwindow.
This package contains one skill and its documentation, no MCP server or account credentials.

Import through the repository's .agents/plugins/marketplace.json after the source is pushed:
Admin > Plugins > Add > Import marketplace; repository URL, empty Path, branch master.
Set workspace installation policy for the intended users after import.

For archive distribution, ZIP this entire visentio-codex-usage directory.
A completed import or account-save is not verified by the existence of this ZIP.

Maintainer: publish the approved Windows ZIP first, then add its real download link to
skills/codex-usage/references/distribution.md. Never include account profiles or local configuration.

The plugin is useful on the web as an installation guide. Direct installation requires
local execution and a user's explicit request. The native miniwindow itself runs only on Windows.
