# SfDataGridRoztahovaniFiltru

Jednoduchá WPF aplikace na .NET 9 demonstrující použití Syncfusion SfDataGrid s filtrováním a roztahovacími popupy.

## ⚠️ Důležité - Syncfusion licence

Před spuštěním aplikace musíte zaregistrovat Syncfusion licenci:

1. **Community licence (ZDARMA)** - Pro individuální vývojáře a malé společnosti:
   - Registrace: https://www.syncfusion.com/products/communitylicense
   - Požadavky: příjem firmy < $1M USD, max 5 vývojářů
   
2. **Po získání licence:**
   - Otevřete soubor `App.xaml.cs`
   - Nahraďte `"YOUR_LICENSE_KEY_HERE"` svým licenčním klíčem
   - Klíč najdete ve svém Syncfusion účtu

3. **Verze balíčků:**
   - Všechny Syncfusion balíčky jsou sjednoceny na verzi **32.1.23**
   - Při aktualizaci aktualizujte všechny balíčky najednou!

## Vlastnosti

- **WPF aplikace na .NET 9** - Využívá nejnovější .NET framework
- **Syncfusion SfDataGrid** - Pokročilý data grid s mnoha funkcemi
- **Filtrování** - AllowFiltering=True umožňuje filtrovat data ve sloupcích
- **Roztahovací filter popupy** - Filter popupy obsahují Thumb (úchyt) vpravo dole pro změnu velikosti
- **Ukázková data** - 15 záznamů objednávek s různými údaji

## Struktura projektu

```
SfDataGridRoztahovaniFiltru/
├── App.xaml              - Definice aplikace
├── App.xaml.cs           - Code-behind aplikace
├── MainWindow.xaml       - Hlavní okno s SfDataGrid
├── MainWindow.xaml.cs    - Logika pro roztahovací popupy
├── OrderInfo.cs          - Model dat pro objednávky
├── ViewModel.cs          - ViewModel s ukázkovými daty
└── SfDataGridRoztahovaniFiltru.csproj
```

## Jak to funguje

### Roztahovací popupy

Aplikace implementuje roztahovací popupy pro filtry pomocí:

1. **DispatcherTimer** - Pravidelně monitoruje visual tree pro nové popupy
2. **Visual Tree Walker** - Prochází visual tree a hledá Popup kontroly
3. **Thumb Control** - Přidává úchyt (thumb) do pravého dolního rohu popup
4. **DragDelta Event** - Zachytává pohyb thumbu a mění velikost popupu

### Implementační detaily

```csharp
// Timer pravidelně kontroluje visual tree
var timer = new System.Windows.Threading.DispatcherTimer
{
    Interval = System.TimeSpan.FromMilliseconds(500)
};

// Při detekci otevřeného popupu přidá Thumb
AddResizeThumbToPopup(popup);

// Thumb handler mění velikost
resizeThumb.DragDelta += (s, args) =>
{
    var newWidth = popupChild.ActualWidth + args.HorizontalChange;
    var newHeight = popupChild.ActualHeight + args.VerticalChange;
    // ... nastavení nové velikosti
};
```

## Spuštění

```bash
cd SfDataGridRoztahovaniFiltru
dotnet restore
dotnet build
dotnet run
```

Nebo otevřete v Visual Studio / Rider a spusťte projekt.

## Použití

1. Spusťte aplikaci
2. Klikněte na ikonu filtru v záhlaví libovolného sloupce
3. Otevře se filter popup
4. V pravém dolním rohu uvidíte úchyt (tři šikmé čárky)
5. Uchopte a táhněte pro změnu velikosti popupu

## Závislosti

- .NET 9.0
- Syncfusion.SfGrid.WPF 28.1.33
- Syncfusion.Themes.MaterialDark.WPF 28.1.33

## Poznámky

- Minimální velikost popupu je 200x150 px
- Timer kontroluje popupy každých 500 ms
- Thumb je poloprůhledný (opacity 0.6) pro lepší viditelnost

