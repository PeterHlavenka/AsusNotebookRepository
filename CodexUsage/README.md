# Codex Usage

Malé plovoucí miniokno pro Windows zobrazující zbývající limity jednoho nebo více účtů Codex. Je použitelné pro různé členy týmu; neobsahuje pevné e-maily ani uživatelské iniciály. Pro firemní použití nabízí volitelnou ikonu Visentia.

- Bez konfigurace zobrazí jeden účet z aktuálního přihlášení Codexu s ikonou Visentia.
- Přes nabídku lze nastavit jeden až osm účtů, jejich názvy a vlastní ikony (iniciály nebo emoji).
- Šířka se přizpůsobuje počtu účtů: 121 bodů pro jeden, 240 pro dva. Výška je 40 bodů.
- Dva řádky u každého účtu: nahoře zbývající pětihodinový limit a čas resetu, dole týdenní limit a datum resetu.
- Čas v místním časovém pásmu Windows ve 24hodinovém formátu.
- Obnova údajů každých 10 sekund. Miniokno trvale udržuje režim vždy navrchu a každou sekundu obnovuje pořadí oken bez přebírání fokusu. Přesun tažením myši, bez tooltipů a přepínání dvojklikem. Vlastní nabídky a dialogy mají přednost; zabezpečená plocha Windows a exkluzivní fullscreen mohou miniokno zakrýt.

## Sestavení a spuštění

Pro sestavení je potřeba Windows a .NET 10 SDK. Z této složky spusť v PowerShellu:

    dotnet publish .\source\CodexUsage.csproj -c Release -o .\app
    .\app\CodexUsage.exe

Uživatel hotové aplikace potřebuje nainstalovaný Codex a .NET 10 Desktop Runtime, SDK nepotřebuje. Aplikace komunikuje s místním codex app-server; nevolá model a nepotřebuje OpenAI API klíč.

## Jeden účet

Při prvním spuštění se vytvoří výchozí konfigurace s jediným účtem Codex bez pevného e-mailu. Miniokno přečte aktuální přihlášení Codexu. Pokud přihlášení chybí, klikni na ikonu účtu nebo vyber Přihlásit samostatně v nabídce pravého tlačítka.

Samostatné přihlášení používá profil miniokna a nepřepíná účet hlavní aplikace Codex.

## Více účtů a nastavení

Pravým tlačítkem otevři **Nastavit účty…**. Přidej, odeber nebo uprav účty a ulož změny. Miniokno ihned přizpůsobí zobrazení bez ručního restartu.

- **Název**: tvoje vlastní označení účtu.
- **E-mail**: volitelný. Pokud je vyplněný, miniokno ověřuje, že se načítá správný účet.
- **Ikona**: nejvýše dva znaky, emoji nebo hodnota visentio pro firemní logo. Bez vyplnění se odvodí iniciály z názvu. Volba loga neovlivňuje přihlášení ani výběr účtu.

Každý účet má vlastní profil. Při více účtech bez zadaného e-mailu se používají pouze samostatná přihlášení, aby se aktuální účet Codexu nezobrazil opakovaně pod různými ikonami. U účtů s vyplněným e-mailem lze před samostatným přihlášením použít aktuální Codex, pouze pokud e-mail souhlasí.

Přihlašuj účty postupně. Přihlášení lze zrušit přes nabídku příslušného účtu. Odebrání účtu z miniokna nesmaže jeho uložený přihlašovací profil.

## Místní nastavení a distribuce pro tým

Veškerá uživatelská nastavení jsou mimo složku aplikace:

- %LOCALAPPDATA%\CodexUsage\accounts.json – seznam účtů
- %LOCALAPPDATA%\CodexUsage\widget.settings.json – pozice miniokna
- %LOCALAPPDATA%\CodexUsage\profiles – oddělené profily přihlášení

Codex ukládá přihlašovací údaje šifrovaně pomocí úložiště Windows. Uživatelské konfigurace a přihlášení se nesdílejí přes Git.

Kolegům předej čistou sestavenou složku app (EXE, DLL, deps.json a runtimeconfig.json). Vlastní accounts.json, widget.settings.json ani složku profiles do balíčku nepřidávej. Každý uživatel má svoji konfiguraci a vlastní přihlášení. Zástupné konfigurace accounts.example.json a accounts.multiple.example.json ukazují formát pro jeden a více účtů.

Předchozí nastavení accounts.json vedle EXE se při prvním spuštění převede do uživatelského profilu. Zachová se ID účtů, takže jejich dřívější přihlášení zůstávají dostupná. Původní kopii s e-maily nepřidávej do distribuovaného balíčku.

## Další ovládání

Pravým tlačítkem lze obnovit údaje, zobrazit stav připojení, obnovit polohu, skrýt okno nebo aplikaci zavřít. Jedním kliknutím na ikonu v oznamovací oblasti se okno znovu zobrazí.

Procenta znamenají zbývající limit. Hvězdička označuje starou nebo neověřenou hodnotu, případně čekání na nové údaje po resetu. Podporované jsou pětihodinové a týdenní limity, jiné firemní limity se nezobrazují.

Pro automatické spuštění otevři pomocí Win + R složku shell:startup a vlož zástupce app\CodexUsage.exe. Složku aplikace potom nepřesouvej.

## Ověření

    .\app\CodexUsage.exe --check "$env:TEMP\CodexUsage-check"

Ověření po automatické obnově uloží check.json a widget.png. Používá vlastní testovací konfiguraci accounts.json v cílové složce; výchozí je jeden účet. Uživatelskou konfiguraci ani pozici nemění. Pro ověření více účtů vlož testovací accounts.json do cílové složky před spuštěním.

Kontroluje parser limitů, shodu účtů, 24hodinový formát, jednoúčtové a víceúčtové konfigurace, neplatná ID profilů, režim vždy navrchu a stav obnovy každého účtu. Živé čtení vyžaduje přihlášení příslušných účtů.

Pro otevření přihlášení konkrétního profilu lze použít --login ID. Parametr ID musí odpovídat ID v místní konfiguraci.

## Distribuce přímo přes workspace plugin

Plugin v plugins/visentio-codex-usage obsahuje aplikaci pro Windows x64, instalátor a krok Nastavit / Setup. Kolega nepotřebuje zvláštní ZIP, download URL, SDK ani checkout zdrojového repozitáře.

Postup kolegy:
1. Přidá Visentio Codex Usage z firemního katalogu.
2. V místním Windows klientu ChatGPT/Codex se shell přístupem spustí Nastavit.
3. Workflow ověří Codex a .NET 10 Desktop Runtime (x64), nainstaluje přibalenou aplikaci, vytvoří zástupce, nastaví spuštění po přihlášení do Windows a miniokno spustí.

Webový nebo cloudový chat nemůže tento Windows instalátor spustit na uživatelově ploše. Pouhé zapnutí pluginu nic neinstaluje. Je nutné ověřit, že cílový workspace desktop klient přenáší celý přibalený skill a umožňuje jeho lokální spuštění; lokální test skriptu sám o sobě neprokazuje funkční onboarding workspace.

## Sestavení pluginu pro správce

    .\distribution\Build-WorkspacePlugin.ps1

Skript sestaví aplikaci a aktualizuje payload skills/setup/bundle přímo ve zdrojové složce pluginu. Přidává pouze EXE, DLL, deps.json, runtimeconfig.json, hash manifest a instalátor. Verze aplikace a pluginu musí souhlasit. Tyto soubory je nutné commitnout spolu se zdroji, protože GitHub workspace import čte repozitář, nikoli CI artifact. Přibalené EXE a DLL mají cílené výjimky z ignorování Gitem.

    .\distribution\Verify-TeamBundle.ps1 -Archive .\dist\Visentio-Codex-Usage-Workspace-Plugin-1.4.0.zip -WorkspacePlugin

Test spouští skutečný setup z rozbaleného pluginu v odděleném profilu bez zástupců a bez spuštění Codexu. Ověřuje předpoklady, instalaci, opakovanou instalaci, aktualizaci, zachování nastavení, odmítnutí poškozených souborů a cest mimo balíček a odinstalaci. Není to test hostitelského workspace UI.

GitHub Actions sestaví a ověří balíčky jako artifact. Nevydává veřejný release a necommitne aktualizovaný payload za správce. Před pushem změny verze vždy spusť Build-WorkspacePlugin.ps1 a commitni jeho payload.

## Instalace a aktualizace

Instalace je pro aktuálního uživatele bez administrátora do %LOCALAPPDATA%\Programs\VisentioCodexUsage. Účty, pozice okna a přihlášení zůstávají v %LOCALAPPDATA%\CodexUsage. Zástupci se vytvoří v nabídce Start, na ploše a ve složce po spuštění.

Po synchronizaci nové verze pluginu kolega spustí Nastavit znovu. Aktualizace zachová nastavení i předchozí verzi aplikace. Neprobíhá skryté spouštění nepodepsaného kódu při pouhé synchronizaci pluginu. Bez certifikátu mohou instalaci omezit firemní politiky Windows; hash kontroluje poškození, nikoli identitu vydavatele.

Odinstalace přes Uninstall.ps1 v instalační složce odstraní aplikaci a její zástupce, ale zachová nastavení a přihlášení.

Samostatný Build-TeamBundle.ps1 je ponechán pro správce a testování, nikoli jako požadovaný distribuční postup pro kolegy.

## Import do Visentio Workspace

Správce otevře Admin > Plugins > Add > Import marketplace:
- Source: URL tohoto GitHub repozitáře
- Path: prázdné (marketplace je v kořeni)
- Branch: master

Manifest .agents/plugins/marketplace.json obsahuje jediný plugin. Správce nastaví jeho dostupnost pro členy týmu. Existující import stačí po pushnutí aktualizovat volbou Synchronizovat nyní. Nedělej nový duplicitní plugin.

Dokumentace: https://learn.chatgpt.com/docs/enterprise/plugin-management
Onboarding: https://developers.openai.com/plugins/build/plugins
