# Install and use

Distribution source: ask the Visentio maintainer for the current approved ZIP. There is no published installer URL configured in this package yet. Do not invent one.

Requirements: Windows x64, installed Codex desktop and internet. The compact team bundle requires .NET 10 Desktop Runtime (x64), checked by the installer. A standalone bundle can include the runtime.

Extract the whole ZIP, then run Install.cmd. It installs into %LOCALAPPDATA%\Programs\VisentioCodexUsage, creates a Start menu and desktop shortcut, enables Windows startup, and opens the miniwindow. No elevation is required. Company policies can restrict unsigned scripts or software; ask IT if blocked.

Right-click the miniwindow > Nastavit účty… to add/remove accounts, set optional e-mail identity checks and choose icons. Use visentio for the company icon. A fresh install has one current Codex account with this icon. For additional accounts, sign in separately through the corresponding account menu. These profiles do not switch the main Codex account.

The window is 40 device-independent units high, always on top by default, draggable, and refreshes every ten seconds. The two rows show remaining five-hour and weekly Codex limits, with local 24-hour reset time and reset date. An asterisk means the reading is old, uncertain or awaiting a reset refresh.

Settings and encrypted profiles live under %LOCALAPPDATA%\CodexUsage. Never share this folder.

# Update

Extract the approved next-version ZIP and run Install.cmd. The installer verifies bundled file hashes, closes only an app installed under its own managed folder, stores the new version separately and updates shortcuts. Existing account settings and sign-in profiles remain untouched. Hashes detect corruption; they do not authenticate the publisher of an unsigned package.

# Troubleshoot

If the app is hidden, click its tray icon. If it is closed, open Visentio Codex Usage from Start.
If no quota appears, confirm Codex is installed and signed in. Open Stav připojení… for a short status. Request only a redacted error, never credentials.
If an account e-mail is configured, it must match the chosen account.
If the local encrypted storage fails, preserve it and contact the maintainer. Do not delete or replace the main Codex authentication store.
Only five-hour and weekly Codex windows are supported; other limits can be unavailable.

# Remove

Run Uninstall.ps1 from %LOCALAPPDATA%\Programs\VisentioCodexUsage. It removes only this installation and its shortcuts; account settings and encrypted sign-ins are retained.
