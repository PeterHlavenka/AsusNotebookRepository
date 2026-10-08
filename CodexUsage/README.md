# Codex Usage

Malé plovoucí miniokno pro Windows zobrazující zbývající limity jednoho nebo více účtů Codex. Je použitelné pro různé členy týmu; neobsahuje pevné e-maily, uživatelské iniciály ani firemní logo.

- Bez konfigurace zobrazí jeden účet z aktuálního přihlášení Codexu.
- Přes nabídku lze nastavit jeden až osm účtů, jejich názvy a vlastní ikony (iniciály nebo emoji).
- Šířka se přizpůsobuje počtu účtů: 121 bodů pro jeden, 240 pro dva. Výška je 40 bodů.
- Dva řádky u každého účtu: nahoře zbývající pětihodinový limit a čas resetu, dole týdenní limit a datum resetu.
- Čas v místním časovém pásmu Windows ve 24hodinovém formátu.
- Obnova každých 10 sekund, vždy navrchu, přesun tažením myši, bez tooltipů a přepínání dvojklikem.

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
- **Ikona**: nejvýše dva znaky nebo emoji. Bez vyplnění se odvodí iniciály z názvu.

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

Pravým tlačítkem lze obnovit údaje, zobrazit stav připojení, vypnout režim vždy navrchu, obnovit polohu, skrýt okno nebo aplikaci zavřít. Jedním kliknutím na ikonu v oznamovací oblasti se okno znovu zobrazí.

Procenta znamenají zbývající limit. Hvězdička označuje starou nebo neověřenou hodnotu, případně čekání na nové údaje po resetu. Podporované jsou pětihodinové a týdenní limity, jiné firemní limity se nezobrazují.

Pro automatické spuštění otevři pomocí Win + R složku shell:startup a vlož zástupce app\CodexUsage.exe. Složku aplikace potom nepřesouvej.

## Ověření

    .\app\CodexUsage.exe --check "$env:TEMP\CodexUsage-check"

Ověření po automatické obnově uloží check.json a widget.png. Používá vlastní testovací konfiguraci accounts.json v cílové složce; výchozí je jeden účet. Uživatelskou konfiguraci ani pozici nemění. Pro ověření více účtů vlož testovací accounts.json do cílové složky před spuštěním.

Kontroluje parser limitů, shodu účtů, 24hodinový formát, jednoúčtové a víceúčtové konfigurace, neplatná ID profilů, režim vždy navrchu a stav obnovy každého účtu. Živé čtení vyžaduje přihlášení příslušných účtů.

Pro otevření přihlášení konkrétního profilu lze použít --login ID. Parametr ID musí odpovídat ID v místní konfiguraci.
