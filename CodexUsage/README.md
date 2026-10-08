# Codex Usage

Malé plovoucí okno pro Windows, které současně zobrazuje zbývající limity osobního a pracovního účtu Codex.

- Rozměry 240 × 40 bodů, vždy navrchu a přesun tažením myši.
- Dva řádky pro každý účet: nahoře pětihodinový limit a čas resetu, dole týdenní limit a datum resetu.
- Čas resetu v místním čase počítače ve 24hodinovém formátu.
- Obnova každých 10 sekund, bez tooltipů a přepínání dvojklikem.
- Osobní a pracovní účet mají oddělené přihlašovací profily.

## Sestavení a spuštění

Potřebuje Windows, nainstalovaný Codex a .NET 10 SDK. Aplikace komunikuje s místním codex app-server; nevolá model a nepotřebuje OpenAI API klíč.

Z této složky spusť v PowerShellu:

    dotnet publish .\source\CodexUsage.csproj -c Release -o .\app
    Copy-Item .\accounts.example.json .\app\accounts.json
    notepad .\app\accounts.json

V app\accounts.json vyplň skutečné e-mailové adresy osobního a pracovního účtu. Při dalších sestaveních ponech tento soubor a přeskakuj jeho kopírování ze vzoru.

    .\app\CodexUsage.exe

Pro spuštění bez SDK stačí již sestavená složka app a .NET 10 Desktop Runtime.

## Přihlášení a ovládání

Nepřipojený účet přihlas kliknutím na jeho ikonu nebo přes pravé tlačítko → Osobní / Firemní → Přihlásit samostatně. V prohlížeči vyber odpovídající účet. Přihlašuj účty postupně.

Samostatné profily jsou v %LOCALAPPDATA%\CodexUsage\profiles; přihlášení ukládá Codex do Správce pověření Windows. Miniokno kontroluje e-mail, aby se údaje účtů nezaměnily. Pokud samostatný profil ještě nemá přihlášení, zkusí aktuální přihlášení Codexu, pouze když e-mail souhlasí.

Pravým tlačítkem lze obnovit údaje, zobrazit stav připojení, vypnout režim vždy navrchu, obnovit polohu, skrýt okno nebo aplikaci zavřít. Jedním kliknutím na ikonu v oznamovací oblasti se okno znovu zobrazí.

Procenta znamenají zbývající limit. Hvězdička označuje starou nebo neověřenou hodnotu, případně čekání na nové údaje po resetu. Podporované jsou pětihodinové a týdenní limity.

Pro automatické spuštění otevři pomocí Win + R složku shell:startup a vlož do ní zástupce app\CodexUsage.exe. Složku aplikace potom nepřesouvej.

## Ověření

    .\app\CodexUsage.exe --check "$env:TEMP\CodexUsage-check"

Ověření po automatické obnově uloží check.json a widget.png. Kontroluje parser limitů, shodu účtů, 24hodinový formát, režim vždy navrchu a stav obnovy každého účtu. Živé čtení vyžaduje přihlášení příslušných účtů.

Zdrojové soubory jsou v source. Sestavená aplikace, místní adresy účtů a uložená pozice se necommitují.
