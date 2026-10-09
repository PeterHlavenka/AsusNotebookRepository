# Codex Usage for Android (experimental)

Native Android home-screen widget, Android 8+ including Android 13. No PC, server, OpenAI API key, model requests or personal data bundled in the APK. One or two accounts in a compact 48dp horizontal strip; Visentio logo for work on the left, a circular P avatar for personal on the right. Remaining 5-hour and weekly percentages, 24-hour local reset time/date. Missing values stay missing; old cached values use a muted amber color. No update-time footer.

## Install

Install `dist/CodexUsage-0.1.2.apk` on the phone, allowing installation from the browser/file manager when Android asks. This development APK is signed with the build machine's local Android debug key. It is for testing, not a production release. Updates must use the same signing key or require reinstalling and signing in again. Keep signing keys out of Git.

Open Codex Usage, add an account, complete sign-in in the system browser, return to the app and add the widget. Long-press the home screen > Widgets > Codex Usage also works. Resize horizontally as needed. Existing widgets keep their launcher grid allocation; remove and re-add the widget to apply the new four-column, one-row default. Tap percentages to refresh; tap an icon for settings.

Periodic jobs request updates every 15 minutes; Android battery/network policies can delay them. Optional minute refresh runs a foreground service with a visible notification; Vivo battery restrictions may still stop it. After a restart, periodic jobs resume; minute mode is restarted manually. Allow notifications and unrestricted background battery use if required. The widget is on the home screen, not an overlay over other applications.

Version 0.1.1 targets Android 16/API 36, while retaining Android 8/API 26 as its minimum. It declares the Android 14+ data-sync foreground permission, handles Android 15+ service timeouts by stopping minute mode and retaining periodic jobs, and respects system bar insets on Android 15+. Android 15+ limits background data-sync foreground services to six hours per 24 hours; opening the app resets the available timer. See [foreground service timeouts](https://developer.android.com/develop/background-work/services/fgs/timeout).

## Sign-in and compatibility boundary

This is an **unofficial experimental Codex client**, not an OpenAI Android SDK or a separately registered OAuth application. It implements the public-client authorization-code + PKCE flow in `openai/codex`, including its existing public client identifier and loopback callback on `localhost:1455`. It opens the external browser, checks random state, and exchanges the code over TLS. Read-only usage is fetched from `https://chatgpt.com/backend-api/wham/usage` with the selected ChatGPT account header. These are Codex implementation endpoints, not a stable published third-party usage API; they may reject this client or change. Actual login and quota retrieval must be validated on a phone. Do not represent successful compilation as verified sign-in.

Credentials are AES-GCM encrypted using an Android Keystore key. Backups are disabled, no tokens are logged, and the HTTP client refuses redirects. No credentials from the Windows app are imported. Login uses a five-minute foreground service so the browser round trip can finish; it stops afterward. Removing an account deletes its local credentials; it does not revoke sessions on other devices.

Implementation references: [Codex login](https://github.com/openai/codex/blob/main/codex-rs/login/src/server.rs), [public client](https://github.com/openai/codex/blob/main/codex-rs/login/src/auth/manager.rs), [usage endpoint](https://github.com/openai/codex/blob/main/codex-rs/backend-client/src/client/rate_limit_resets.rs), [Android widget updates](https://developer.android.com/develop/ui/views/appwidgets/advanced).

## Build

JDK 17+ (JDK 21 also works), Gradle 8.11.1, Android SDK platform 36/build-tools 35.0.0. Run `./Build.ps1 -SdkRoot <SDK> -JavaHome <JDK> -Gradle <gradle.bat>`. It runs parser tests, Android lint, and assembles a signed development APK. No dependencies at runtime beyond Android framework APIs. Source and output contain no machine-specific paths.
