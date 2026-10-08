using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Forms = System.Windows.Forms;
using Drawing = System.Drawing;
using FilePath = System.IO.Path;

namespace CodexUsage;

public sealed class Widget : Window
{
    private sealed class AccountPanel(AccountDefinition definition)
    {
        public AccountDefinition Definition { get; } = definition;
        public string Key => Definition.Id;
        public string Name => Definition.Name;
        public string? Email => Definition.Email;
        public string DisplayEmail => Connection?.Email ?? Email ?? "Aktuální účet Codexu";
        public string Home { get; } = FilePath.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CodexUsage", "profiles", definition.Id);
        public CodexConnection? Connection;
        public UsageSnapshot? Snapshot;
        public string? Failure;
        public bool Refreshing, SigningIn, Independent;
        public TextBlock[] Percentages = new TextBlock[2], Resets = new TextBlock[2];
    }
    private readonly AccountPanel[] accounts;
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromSeconds(10) };
    private readonly Border panel;
    private readonly Forms.NotifyIcon tray;
    private readonly Drawing.Icon icon;
    private readonly CancellationTokenSource lifetime = new();
    private readonly string settingsPath = FilePath.Combine(AccountConfiguration.UserDirectory, "widget.settings.json");
    private readonly string? checkDirectory;
    private bool closed, pinned = true;
    private Settings settings;
    private sealed record Settings(double? Left = null, double? Top = null);

    public Widget(string? checkDirectory = null, string? loginAccount = null)
    {
        this.checkDirectory = checkDirectory;
        var configurationPath = checkDirectory is null ? AccountConfiguration.FileName : FilePath.Combine(checkDirectory, "accounts.json");
        accounts = AccountConfiguration.Load(configurationPath, checkDirectory is null ? FilePath.Combine(AppContext.BaseDirectory, "accounts.json") : configurationPath)
            .Select(definition => new AccountPanel(definition)).ToArray();
        if (checkDirectory is null && !File.Exists(settingsPath) && File.Exists(FilePath.Combine(AppContext.BaseDirectory, "widget.settings.json")))
            File.Copy(FilePath.Combine(AppContext.BaseDirectory, "widget.settings.json"), settingsPath);
        try { settings = JsonSerializer.Deserialize<Settings>(File.ReadAllText(settingsPath)) ?? new(); } catch { settings = new(); }
        Title = "Codex Usage"; Width = WidthForAccounts(accounts.Length); Height = 40;
        WindowStyle = WindowStyle.None; ResizeMode = ResizeMode.NoResize;
        AllowsTransparency = true; Background = Brushes.Transparent;
        Topmost = true; ShowInTaskbar = false; ShowActivated = false;
        UseLayoutRounding = true; SnapsToDevicePixels = true;
        panel = new Border {
            CornerRadius = new CornerRadius(6), Background = Brush("#202222"),
            BorderBrush = Brush("#474C47"), BorderThickness = new Thickness(1),
            Padding = new Thickness(4, 2, 4, 2)
        };
        ToolTipService.SetIsEnabled(panel, false);
        var layout = new Grid();
        for (int i = 0; i < accounts.Length; i++) {
            if (i > 0) {
                layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
                var separator = new Border { Width = 1, Height = 24, Background = Brush("#424842"), VerticalAlignment = VerticalAlignment.Center };
                Grid.SetColumn(separator, i * 2 - 1); layout.Children.Add(separator);
            }
            layout.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            var view = CreateAccountView(accounts[i], i); Grid.SetColumn(view, i * 2); layout.Children.Add(view);
        }
        panel.Child = layout; Content = panel;
        panel.MouseLeftButtonDown += (_, _) => { try { DragMove(); Save(); } catch (InvalidOperationException) { } };
        panel.ContextMenu = BuildMenu();
        icon = CreateIcon(); tray = new Forms.NotifyIcon { Icon = icon, Text = "", Visible = true };
        var trayMenu = new Forms.ContextMenuStrip();
        trayMenu.Items.Add("Zobrazit miniokno", null, (_, _) => Dispatcher.Invoke(() => { Show(); Topmost = pinned; }));
        trayMenu.Items.Add("Obnovit", null, async (_, _) => await Dispatcher.InvokeAsync(RefreshAsync).Task.Unwrap());
        trayMenu.Items.Add("Zavřít", null, (_, _) => Dispatcher.Invoke(Close));
        tray.ContextMenuStrip = trayMenu;
        tray.MouseClick += (_, e) => { if (e.Button == Forms.MouseButtons.Left) Dispatcher.Invoke(() => { Show(); Topmost = pinned; }); };
        Loaded += async (_, _) => {
            PositionWindow(); timer.Tick += async (_, _) => await RefreshAsync(); timer.Start();
            await RefreshAsync(); if (checkDirectory is not null) await FinishCheckAsync();
            else if (loginAccount is not null && accounts.FirstOrDefault(a => a.Key == loginAccount) is { } account) await SignInAsync(account);
        };
        Closed += (_, _) => {
            closed = true; lifetime.Cancel(); timer.Stop(); Save(); tray.Visible = false; tray.Dispose(); icon.Dispose();
            foreach (var account in accounts) account.Connection?.Dispose();
        };
    }

    private Grid CreateAccountView(AccountPanel account, int index)
    {
        var rows = new Grid();
        foreach (int width in new[] { 24, 38 }) rows.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(width) });
        rows.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        var avatar = CreateAvatar(account.Definition, index); Grid.SetRowSpan(avatar, 2); rows.Children.Add(avatar);
        avatar.MouseLeftButtonDown += async (_, e) => { e.Handled = true; if (account.Snapshot is null) await SignInAsync(account); };
        for (int i = 0; i < 2; i++) {
            rows.RowDefinitions.Add(new RowDefinition { Height = new GridLength(17) });
            account.Percentages[i] = Text("—", "#B8CEAA", true); account.Percentages[i].TextAlignment = TextAlignment.Right;
            account.Resets[i] = Text("—", "#A4ABA0", false); account.Resets[i].TextAlignment = TextAlignment.Right;
            Add(rows, account.Percentages[i], i, 1); Add(rows, account.Resets[i], i, 2);
        }
        System.Windows.Automation.AutomationProperties.SetName(rows, account.Name + " účet: " + account.DisplayEmail);
        return rows;
    }
    private static UIElement CreateAvatar(AccountDefinition account, int index)
    {
        if (string.Equals(account.Icon, "visentio", StringComparison.OrdinalIgnoreCase)) {
            var canvas = new Canvas { Width = 24, Height = 24, VerticalAlignment = VerticalAlignment.Center };
            var teal = Brush("#00B4B0");
            var left = new Ellipse { Width = 6, Height = 6, Fill = teal };
            Canvas.SetLeft(left, 2); Canvas.SetTop(left, 2); canvas.Children.Add(left);
            var right = new Ellipse { Width = 6, Height = 6, Fill = teal };
            Canvas.SetLeft(right, 13); Canvas.SetTop(right, 1); canvas.Children.Add(right);
            canvas.Children.Add(new System.Windows.Shapes.Path {
                Data = Geometry.Parse("M 2,11 C 2,8 8,8 8,11 L 8,20 C 8,23 2,23 2,20 Z"), Fill = teal
            });
            canvas.Children.Add(new System.Windows.Shapes.Path {
                Data = Geometry.Parse("M 10,12 L 14,17 L 21,8 L 23,10 L 14,23 L 10,18 Z"), Fill = teal
            });
            return canvas;
        }
        string[] colors = ["#385063", "#32625B", "#63506F", "#6B543D"];
        return new Border {
            Width = 22, Height = 22, CornerRadius = new CornerRadius(11),
            Background = Brush(colors[index % colors.Length]), VerticalAlignment = VerticalAlignment.Center,
            Child = new TextBlock {
                Text = account.AvatarText, Foreground = Brush("#E6ECEF"), FontSize = 9, FontFamily = new FontFamily("Segoe UI"),
                HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Center
            }
        };
    }
    private static SolidColorBrush Brush(string color) => (SolidColorBrush)new BrushConverter().ConvertFromString(color)!;
    private static TextBlock Text(string text, string color, bool bold) => new() {
        Text = text, FontFamily = new FontFamily("Segoe UI"), FontSize = 11,
        FontWeight = bold ? FontWeights.SemiBold : FontWeights.Normal, Foreground = Brush(color), VerticalAlignment = VerticalAlignment.Center
    };
    private static void Add(Grid grid, UIElement child, int row, int column) { Grid.SetRow(child, row); Grid.SetColumn(child, column); grid.Children.Add(child); }

    private ContextMenu BuildMenu()
    {
        var menu = new ContextMenu();
        var refresh = new MenuItem { Header = "Obnovit údaje" }; refresh.Click += async (_, _) => await RefreshAsync(); menu.Items.Add(refresh);
        foreach (var account in accounts) {
            var accountMenu = new MenuItem { Header = account.Name + (account.Email is null ? "" : " · " + account.Email) };
            var login = new MenuItem { Header = "Přihlásit samostatně…" }; login.Click += async (_, _) => await SignInAsync(account);
            var cancel = new MenuItem { Header = "Zrušit probíhající přihlášení" }; cancel.Click += (_, _) => {
                account.Connection?.Dispose(); account.Connection = null; account.Failure = "Přihlášení zrušeno"; Render();
            };
            accountMenu.Items.Add(login); accountMenu.Items.Add(cancel); menu.Items.Add(accountMenu);
            menu.Opened += (_, _) => { login.IsEnabled = !accounts.Any(a => a.SigningIn); cancel.IsEnabled = account.SigningIn; };
        }
        var status = new MenuItem { Header = "Stav připojení…" };
        status.Click += (_, _) => MessageBox.Show(string.Join("\n\n", accounts.Select(a => a.Name + " · " + a.DisplayEmail + "\n" +
            (a.Failure ?? (a.Snapshot is null ? "Není připojeno" : "Načteno " + a.Snapshot.ObservedAt.ToLocalTime().ToString("HH:mm:ss"))) +
            (a.Independent ? "\nSamostatné přihlášení" : "\nPřihlášení aktuálního Codexu"))), "Codex Usage");
        menu.Items.Add(status);
        var edit = new MenuItem { Header = "Nastavit účty…" }; edit.Click += (_, _) => EditAccounts(); menu.Items.Add(edit);
        menu.Opened += (_, _) => edit.IsEnabled = !accounts.Any(a => a.SigningIn);
        var top = new MenuItem { Header = "Vždy navrchu", IsCheckable = true, IsChecked = true }; top.Click += (_, _) => { pinned = top.IsChecked; Topmost = pinned; };
        var reposition = new MenuItem { Header = "Vrátit do pravého dolního rohu" }; reposition.Click += (_, _) => { settings = new(); PositionWindow(); Save(); };
        var hide = new MenuItem { Header = "Skrýt do oznamovací oblasti" }; hide.Click += (_, _) => Hide();
        var close = new MenuItem { Header = "Zavřít" }; close.Click += (_, _) => Close();
        menu.Items.Add(top); menu.Items.Add(reposition); menu.Items.Add(new Separator()); menu.Items.Add(hide); menu.Items.Add(close);
        menu.Opened += (_, _) => top.IsChecked = pinned;
        return menu;
    }

    private void EditAccounts()
    {
        var editor = new AccountEditor(accounts.Select(a => a.Definition).ToArray()) { Owner = this };
        if (editor.ShowDialog() != true || editor.Result is null) return;
        try {
            AccountConfiguration.Save(AccountConfiguration.FileName, editor.Result); Save();
            var replacement = new Widget(); System.Windows.Application.Current.MainWindow = replacement;
            Close(); replacement.Show();
        } catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException) {
            MessageBox.Show(this, e.Message, "Nastavení účtů");
        }
    }

    private async Task SignInAsync(AccountPanel account)
    {
        if (accounts.Any(a => a.SigningIn)) return;
        account.SigningIn = true; account.Failure = "Čeká na přihlášení"; account.Snapshot = null; Render();
        while (account.Refreshing && !closed) await Task.Delay(100);
        if (closed) return;
        account.Connection?.Dispose();
        var connection = new CodexConnection(account.Home, account.Email); account.Connection = connection;
        try {
            await connection.LoginAsync(url => Process.Start(new ProcessStartInfo(url.AbsoluteUri) { UseShellExecute = true }), lifetime.Token);
            account.Independent = true; File.WriteAllText(FilePath.Combine(account.Home, "connected"), "Codex Usage");
            account.Snapshot = await connection.ReadAsync(); account.Failure = null;
        }
        catch (OperationCanceledException) { account.Failure = "Přihlášení zrušeno nebo vypršelo"; }
        catch (Exception e) { account.Failure = e is AccountUnavailableException or InvalidDataException ? e.Message : "Přihlášení selhalo"; }
        finally { account.SigningIn = false; Render(); }
        if (account.Failure is not null && !closed) MessageBox.Show(account.Failure + "\nZkontroluj volbu účtu v prohlížeči a zkus přihlášení znovu.", account.Name + " · Codex Usage");
    }
    private async Task RefreshAsync() => await Task.WhenAll(accounts.Select(RefreshAccountAsync));
    private async Task RefreshAccountAsync(AccountPanel account)
    {
        if (closed || account.Refreshing || account.SigningIn) return;
        account.Refreshing = true;
        try {
            if (account.Connection is null) {
                account.Independent = File.Exists(FilePath.Combine(account.Home, "connected")) || (accounts.Length > 1 && account.Email is null);
                account.Connection = new CodexConnection(account.Independent ? account.Home : null, account.Email);
            }
            account.Snapshot = await account.Connection.ReadAsync(); account.Failure = null;
        }
        catch (AccountUnavailableException) { account.Snapshot = null; account.Failure = "Přihlásit samostatně"; }
        catch (Exception e) when (e is not OperationCanceledException) { account.Failure = e is FileNotFoundException or InvalidDataException ? e.Message : "Načtení selhalo"; }
        catch (OperationCanceledException) { }
        finally { account.Refreshing = false; Render(); }
    }
    private void Render()
    {
        if (closed) return;
        var now = DateTimeOffset.UtcNow;
        foreach (var account in accounts) {
            var values = new[] { account.Snapshot?.FiveHour, account.Snapshot?.Weekly };
            bool stale = account.Snapshot is not null && now - account.Snapshot.ObservedAt > TimeSpan.FromSeconds(45);
            for (int i = 0; i < 2; i++) {
                var quota = values[i]; bool pastReset = quota?.ResetsAt is long reset && now >= DateTimeOffset.FromUnixTimeSeconds(reset);
                account.Percentages[i].Text = (quota is null ? "—" : (100 - quota.UsedPercent).ToString("0.#", CultureInfo.GetCultureInfo("cs-CZ")) + "%") +
                    (quota is not null && (stale || account.Failure is not null || pastReset) ? "*" : "");
                account.Percentages[i].Foreground = Brush(quota is null || stale || account.Failure is not null || pastReset ? "#DCB97E" : quota.UsedPercent >= 80 ? "#EE9D7E" : "#B8CEAA");
                account.Resets[i].Text = quota?.ResetsAt is long seconds ? FormatReset(DateTimeOffset.FromUnixTimeSeconds(seconds).ToLocalTime(), i) :
                    account.SigningIn ? "Login…" : i == 1 && account.Snapshot is null ? "Přihlásit" : "—";
            }
        }
    }
    public static double WidthForAccounts(int count) => 10 + 111 * count + 8 * Math.Max(0, count - 1);
    public static string FormatReset(DateTimeOffset localTime, int row) => localTime.ToString(row == 0 ? "HH':'mm" : "d. M.", CultureInfo.InvariantCulture);
    private void PositionWindow()
    {
        var area = SystemParameters.WorkArea; Left = settings.Left ?? area.Right - Width - 15; Top = settings.Top ?? area.Bottom - Height - 10;
        if (Left + Width < SystemParameters.VirtualScreenLeft + 30 || Top + Height < SystemParameters.VirtualScreenTop + 20 ||
            Left > SystemParameters.VirtualScreenLeft + SystemParameters.VirtualScreenWidth - 30 || Top > SystemParameters.VirtualScreenTop + SystemParameters.VirtualScreenHeight - 20)
        { Left = area.Right - Width - 15; Top = area.Bottom - Height - 10; }
    }
    private void Save()
    {
        if (checkDirectory is not null) return;
        settings = new(Left, Top);
        try { Directory.CreateDirectory(AccountConfiguration.UserDirectory); File.WriteAllText(settingsPath + ".tmp", JsonSerializer.Serialize(settings)); File.Move(settingsPath + ".tmp", settingsPath, true); } catch { }
    }
    private async Task FinishCheckAsync()
    {
        var firstObservation = accounts.Select(a => a.Snapshot?.ObservedAt).ToArray(); await Task.Delay(11300);
        Directory.CreateDirectory(checkDirectory!); var dpi = VisualTreeHelper.GetDpi(this);
        var bitmap = new RenderTargetBitmap((int)Math.Ceiling(ActualWidth * dpi.DpiScaleX), (int)Math.Ceiling(ActualHeight * dpi.DpiScaleY), 96 * dpi.DpiScaleX, 96 * dpi.DpiScaleY, PixelFormats.Pbgra32);
        bitmap.Render(this); var encoder = new PngBitmapEncoder(); encoder.Frames.Add(BitmapFrame.Create(bitmap));
        using (var stream = File.Create(FilePath.Combine(checkDirectory!, "widget.png"))) encoder.Save(stream);
        var results = accounts.Select((a, i) => new { account = a.Key, liveRead = a.Snapshot is not null,
            automaticRefresh = a.Snapshot?.ObservedAt > firstObservation[i], error = a.Failure,
            rows = a.Percentages.Select(x => x.Text).ToArray(), resetLabels = a.Resets.Select(x => x.Text).ToArray() }).ToArray();
        File.WriteAllText(FilePath.Combine(checkDirectory!, "check.json"), JsonSerializer.Serialize(new {
            accounts = results, parserChecks = ParserChecks.Run(), topmost = Topmost,
            nativeTopmost = (GetWindowLong(new WindowInteropHelper(this).Handle, -20) & 8) != 0,
            tooltipDisabled = !ToolTipService.GetIsEnabled(panel) && panel.ToolTip is null && tray.Text == "",
            showInTaskbar = ShowInTaskbar, width = ActualWidth, height = ActualHeight,
            taskbarHeight = SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Height
        }, new JsonSerializerOptions { WriteIndented = true })); Close();
    }
    private static Drawing.Icon CreateIcon()
    {
        using var bitmap = new Drawing.Bitmap(16, 16); using var graphics = Drawing.Graphics.FromImage(bitmap); graphics.Clear(Drawing.Color.Transparent);
        using var brush = new Drawing.SolidBrush(Drawing.Color.FromArgb(184, 206, 170)); graphics.FillRectangle(brush, 2, 4, 12, 3); graphics.FillRectangle(brush, 2, 10, 8, 3);
        var handle = bitmap.GetHicon(); try { using var native = Drawing.Icon.FromHandle(handle); return (Drawing.Icon)native.Clone(); } finally { DestroyIcon(handle); }
    }
    [DllImport("user32.dll")] private static extern bool DestroyIcon(IntPtr icon);
    [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr window, int index);
}
public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        var check = args.Length == 2 && args[0] == "--check" ? FilePath.GetFullPath(args[1]) : null;
        string? login = args.Length == 2 && args[0] == "--login" ? args[1] : null;
        using var mutex = new Mutex(true, "Local\\CodexUsageWidget", out var created);
        if (!created && check is null) { MessageBox.Show("Codex Usage už běží. Najdeš ho v oznamovací oblasti.", "Codex Usage"); return; }
        var app = new System.Windows.Application { ShutdownMode = ShutdownMode.OnMainWindowClose }; try { app.Run(new Widget(check, login)); }
        catch (Exception e) when (e is IOException or InvalidDataException or UnauthorizedAccessException) { MessageBox.Show(e.Message, "Codex Usage"); }
    }
}
