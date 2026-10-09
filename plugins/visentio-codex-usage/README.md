# Visentio Codex Usage

The workspace plugin includes the Windows x64 miniwindow, its installer and the setup workflow.
No separate download or ZIP transfer is needed by a colleague.

User flow:
1. Install this plugin from the Visentio Workspace catalog.
2. Run its Setup / Nastavit workflow in a local Windows ChatGPT/Codex environment with shell access.
3. Setup checks Codex and .NET 10 Desktop Runtime, installs the bundled app for this user, creates shortcuts, enables startup and launches it.

The onboarding entrypoint is skills/setup/SKILL.md. Supporting files stay inside that skill so
the installer and Windows payload travel with its instructions.
A web/cloud chat cannot perform installation on a user's Windows desktop.
Workspace delivery of the bundled executable and invocation of setup must be verified in the target client;
script tests alone are not proof of successful workspace onboarding.

Build source: CodexUsage/distribution/Build-WorkspacePlugin.ps1 from the repository root.
Regenerate and test the bundled app whenever the app version changes, then commit the payload with its manifest.
A GitHub workspace import reads committed files; CI artifacts alone do not update those files.

Updates: synchronize the plugin, then run Setup again. Windows accounts and encrypted sign-in profiles
remain in the user's profile. This is a user-triggered update, not a silent executable download.

No local usernames, e-mails, accounts.json, widget.settings.json or profiles may be included.
The package is unsigned, as requested for the small team. Hashes check corruption, not publisher identity.
