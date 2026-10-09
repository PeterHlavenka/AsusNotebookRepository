---
name: setup
description: Install, update and launch the bundled Visentio Codex Usage miniwindow on the user's local Windows computer, without a separate download or ZIP.
---

Set up the Windows miniwindow using the installer bundled with this skill. Respond in the user's language.

Selecting this setup workflow requests installation and launch of the bundled app. Explain briefly that it installs for the current Windows user, creates shortcuts, enables Windows sign-in startup and preserves existing account settings. Do not request redundant conversational confirmation; honor the host's execution approvals and company policies.

1. Establish the execution environment. Installation requires LOCAL execution on the user's Windows computer. Do not install into a remote/cloud environment, container, WSL, server or another user's computer. A Windows build server is not the user's desktop. If local execution is unavailable, explain that the user must open the plugin in their local Windows ChatGPT/Codex client with shell access and run setup there. Do not substitute manual ZIP distribution.
2. Resolve scripts/Setup.ps1 relative to THIS installed SKILL.md, never a hardcoded username, drive, Git repository or working directory.
3. Run the packaged script with -CheckOnly. It reports bundled version, runtime availability and Codex executable availability without reading credentials. If .NET 10 Desktop Runtime (x64) or Codex is missing, report the actual prerequisite. Do not download executables from invented URLs.
4. Run the same script without -CheckOnly. It installs from bundle/app, verifies file hashes, creates shortcuts and starts the miniwindow. Wait for completion. Do not unpack a separate ZIP, compile the application, create a GitHub release, use hooks or download app-server credentials.
5. Confirm installation from the script's actual result. Do not claim a window is visible solely because a process started. If the script reports that the app is already running outside its managed installation, ask the user to close that copy and retry.
6. Direct the user to right-click > Nastavit účty… only if they need more than the default current Codex account. Each additional profile uses their own sign-in.

Updates use this same setup script after a plugin sync. Re-run it when the user requests an update, preserving their settings. An installed plugin update alone does not update or start the Windows app automatically.

Never read passwords, OAuth tokens, encrypted auth files or unredacted private account logs. The source of the executable is this trusted plugin package; SHA-256 checks detect corruption, not a publisher signature. Do not bypass device restrictions or invent successful workspace execution.
