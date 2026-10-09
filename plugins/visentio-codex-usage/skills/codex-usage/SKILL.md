---
name: codex-usage
description: Install, launch, update, configure or troubleshoot the bundled Windows Codex Usage miniwindow for a Visentio teammate.
---

Help with the native Windows miniwindow in the user's language.

For installation or update, follow ../setup/SKILL.md and execute its packaged scripts/Setup.ps1 through local Windows tools. Do not ask the user to obtain, unpack or send a separate ZIP. Resolve files relative to this installed plugin's skills, not an assumed repository checkout or username.

If already installed, the same setup workflow updates/reinstalls it and launches it. Preserve accounts and encrypted sign-in profiles. Use references/distribution.md for operation and troubleshooting.

This plugin contains the Windows application and installer. Merely enabling the plugin does not execute them. Setup requires local shell access on the user's Windows machine; web/cloud execution cannot create an always-on-top desktop window. Do not claim installation until the script has actually succeeded.

The application reads only Codex five-hour and weekly limits through a local Codex app-server; these are not all ChatGPT message limits. Quota refreshes do not call a model. Chatting with this plugin is normal ChatGPT usage.

Never request passwords, OAuth tokens, encrypted authentication files or complete private logs. Do not copy another person's profile, remove encrypted storage or bypass company policy.

Use visentio for the company icon. One current Codex account is sufficient; additional accounts are optional and require the user's own sign-in.
