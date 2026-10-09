# Set up without a separate ZIP

The Windows application and installer are included in ../setup/bundle as part of this plugin.
Invoke the plugin's Setup / Nastavit workflow in the user's LOCAL Windows ChatGPT/Codex client with shell access.
Follow ../setup/SKILL.md; it resolves scripts/Setup.ps1 from its own installed skill directory.
No download URL, GitHub release, separate ZIP, source checkout or SDK is needed for the colleague.

Requirements: local Windows x64, Codex desktop, .NET 10 Desktop Runtime (x64), internet for quota reads and the user's own Codex sign-in.
Setup -CheckOnly reports prerequisites without reading credentials.
The installed app goes to %LOCALAPPDATA%\Programs\VisentioCodexUsage, with Start/desktop shortcuts and Windows sign-in startup.
No administrator rights are needed. Company policies can still restrict unsigned software or scripts.

Web/cloud/mobile chats can use the support instructions, but cannot install or launch a local Windows window.
If local resources or execution are unavailable, tell the user to open setup in the local Windows client.
Do not substitute manual ZIP distribution or claim that adding the plugin silently installs Windows software.

# Accounts and operation

Right-click the miniwindow > Nastavit účty… to add/remove accounts, set optional identity-check e-mails or choose icons.
The value visentio draws the company logo. A fresh install uses one current Codex account.
Each optional additional account uses the user's own separate sign-in and does not switch the main Codex application.

The window is 40 device-independent units high, draggable, and maintains always-on-top every second without taking focus.
Secure Windows screens and some exclusive-fullscreen applications can still cover it.
Quota reads refresh every ten seconds. Rows display remaining five-hour and weekly Codex limits.
Time uses local Windows time in 24-hour format. An asterisk marks old, uncertain or reset-pending readings.

Settings and encrypted profiles live in %LOCALAPPDATA%\CodexUsage. Never share this directory or ask for credential files.

# Update and launch

After the plugin syncs to a new version, run the same setup workflow again.
Setup installs its bundled version, updates shortcuts and launches the app; it does not update in the background merely because the plugin synced.
The installer validates file hashes and retains previous version directories and all account settings.
A running copy outside the managed installation must be closed by the user before setup launches another copy.

# Troubleshoot and remove

Click the tray icon to show a hidden app; use Start > Visentio Codex Usage for a closed app.
If no quota appears, verify Codex is installed and signed in. Stav připojení… gives a short status; request only a redacted error.
A configured e-mail must match the selected account.
Preserve encrypted storage on errors; do not delete the main Codex authentication store.
Run Uninstall.ps1 from %LOCALAPPDATA%\Programs\VisentioCodexUsage to remove the installation and its shortcuts.
Accounts and encrypted profiles remain untouched.
