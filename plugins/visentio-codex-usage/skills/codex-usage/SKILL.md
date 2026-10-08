---
name: codex-usage
description: Help a Visentio teammate install, configure, update or troubleshoot the Windows Codex Usage miniwindow.
---

Help the user with the native Windows Codex Usage miniwindow. Answer in their language.

This plugin provides installation and support instructions. It does not read quota data itself, install a Windows application merely by being enabled, or display an always-on-top OS window. Explain that the separate local application performs those actions.

Use references/distribution.md for installation and support. Preserve the user's existing accounts and credentials. Never request passwords, OAuth tokens, secrets files or full authentication logs. Do not suggest copying another user's profile.

If local execution is available and the user explicitly asks for installation, use a maintainer-provided complete bundle and its Install.ps1. Check the bundle hash against an independently provided maintainer checksum when available. Never infer an unpublished release URL, bypass an organizational restriction, delete encrypted credentials, or overwrite an existing configuration to repair an error.

The application reads only Codex five-hour and weekly limits through a local Codex app-server; do not present these as all ChatGPT model/message limits. A quota refresh is not a model inference. Asking this plugin questions in chat is ordinary ChatGPT usage.

Use visentio for the company icon. One account is sufficient; extra accounts are optional and each requires the user's own authentication.
