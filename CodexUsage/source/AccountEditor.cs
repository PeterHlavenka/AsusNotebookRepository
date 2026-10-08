using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CodexUsage;

public sealed class AccountEditor : Window
{
    private readonly ObservableCollection<AccountDefinition> accounts;
    private readonly DataGrid grid;
    public AccountDefinition[]? Result { get; private set; }

    public AccountEditor(AccountDefinition[] definitions)
    {
        Title = "Účty · Codex Usage"; Width = 600; Height = 350;
        MinWidth = 500; MinHeight = 300; WindowStartupLocation = WindowStartupLocation.CenterOwner;
        ShowInTaskbar = false;
        accounts = new(definitions.Select(a => new AccountDefinition { Id = a.Id, Name = a.Name, Email = a.Email, Icon = a.Icon }));
        var layout = new DockPanel { Margin = new Thickness(14) };
        var help = new TextBlock {
            Text = "Přidej účty, které chceš sledovat. E-mail je volitelný; při vyplnění se ověřuje shoda.\nJeden účet bez e-mailu použije aktuální Codex. Ikona: iniciály, emoji nebo visentio.",
            TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 12)
        };
        DockPanel.SetDock(help, Dock.Top); layout.Children.Add(help);
        var buttons = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 12, 0, 0) };
        DockPanel.SetDock(buttons, Dock.Bottom); layout.Children.Add(buttons);
        grid = new DataGrid { ItemsSource = accounts, AutoGenerateColumns = false, CanUserAddRows = false,
            CanUserDeleteRows = false, SelectionMode = DataGridSelectionMode.Single };
        grid.Columns.Add(Column("Název", nameof(AccountDefinition.Name), 140));
        grid.Columns.Add(Column("E-mail (volitelný)", nameof(AccountDefinition.Email), 270));
        grid.Columns.Add(Column("Ikona", nameof(AccountDefinition.Icon), 85));
        layout.Children.Add(grid);
        AddButton(buttons, "Přidat", () => {
            if (accounts.Count >= 8) return;
            var account = new AccountDefinition { Id = "account-" + Guid.NewGuid().ToString("N"), Name = "Účet " + (accounts.Count + 1) };
            accounts.Add(account); grid.SelectedItem = account; grid.ScrollIntoView(account);
        });
        AddButton(buttons, "Odebrat", () => {
            if (grid.SelectedItem is AccountDefinition selected && accounts.Count > 1) accounts.Remove(selected);
        });
        AddButton(buttons, "Zrušit", () => DialogResult = false);
        AddButton(buttons, "Uložit", () => {
            if (!grid.CommitEdit(DataGridEditingUnit.Cell, true) || !grid.CommitEdit(DataGridEditingUnit.Row, true)) return;
            try { Result = AccountConfiguration.Validate(accounts); DialogResult = true; }
            catch (InvalidDataException e) { MessageBox.Show(this, e.Message, "Nastavení účtů"); }
        });
        Content = layout;
    }

    private static DataGridTextColumn Column(string header, string property, int width) =>
        new() { Header = header, Binding = new Binding(property) { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged }, Width = width };
    private static void AddButton(Panel panel, string title, Action action)
    {
        var button = new Button { Content = title, Padding = new Thickness(10, 5, 10, 5), Margin = new Thickness(6, 0, 0, 0) };
        button.Click += (_, _) => action(); panel.Children.Add(button);
    }
}
