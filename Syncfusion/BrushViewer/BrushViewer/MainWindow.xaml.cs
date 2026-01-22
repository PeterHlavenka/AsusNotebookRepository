using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;

namespace BrushViewer
{
    public partial class MainWindow
    {
        private List<BrushPairItem> m_allBrushPairs = [];
        private BrushPairItem? m_selectedPair;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Relativní cesty k oběma souborům Brushes.xaml
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                var darkPath = Path.Combine(baseDir, @"..\..\..\..\..\Windows11MintDarkTheme\Common\Brushes.xaml");
                var lightPath = Path.Combine(baseDir, @"..\..\..\..\..\Windows11MintLightTheme\Common\Brushes.xaml");

                if (!File.Exists(darkPath) || !File.Exists(lightPath))
                {
                    StatusText.Text = "Jeden ze souborů Brushes.xaml nebyl nalezen!";
                    return;
                }

                var darkBrushes = LoadBrushes(darkPath);
                var lightBrushes = LoadBrushes(lightPath);

                // Spojení podle klíče
                var allKeys = new HashSet<string>();
                foreach (var b in darkBrushes) allKeys.Add(b.Key);
                foreach (var b in lightBrushes) allKeys.Add(b.Key);

                var pairs = new List<BrushPairItem>();
                foreach (var key in allKeys)
                    pairs.Add(new BrushPairItem
                    {
                        Key = key,
                        Dark = darkBrushes.Find(b => b.Key == key),
                        Light = lightBrushes.Find(b => b.Key == key)
                    });

                m_allBrushPairs = pairs;
                BrushList.ItemsSource = m_allBrushPairs;
                StatusText.Text = $"Načteno {pairs.Count} barevných párů";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"Chyba: {ex.Message}";
                MessageBox.Show($"Chyba při načítání: {ex.Message}", "Chyba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateDisplay();
        }

        private void CopyKeyToClipboard(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext is BrushPairItem item)
                try
                {
                    Clipboard.SetText(item.Key);
                    StatusText.Text = $"Zkopírováno do schránky: {item.Key}";
                }
                catch (Exception ex)
                {
                    StatusText.Text = $"Chyba při kopírování: {ex.Message}";
                }
        }

        private void UpdateDisplay()
        {
            var searchText = SearchBox.Text?.Trim().ToLowerInvariant() ?? string.Empty;

            if (string.IsNullOrEmpty(searchText))
            {
                BrushList.ItemsSource = m_allBrushPairs;
                StatusText.Text = $"Zobrazeno {m_allBrushPairs.Count} z {m_allBrushPairs.Count} barev";
            }
            else
            {
                var filtered = m_allBrushPairs.FindAll(item =>
                    item.Key.ToLowerInvariant().Contains(searchText));
                BrushList.ItemsSource = filtered;
                StatusText.Text = $"Zobrazeno {filtered.Count} z {m_allBrushPairs.Count} barev";
            }
        }

        private void BrushList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrushList.SelectedItem is BrushPairItem selectedPair)
            {
                m_selectedPair = selectedPair;
                UpdatePreview();
            }
        }

        private void UpdatePreview()
        {
            if (m_selectedPair == null)
            {
                PreviewDark.Background = Brushes.Transparent;
                PreviewLight.Background = Brushes.Transparent;
                PreviewKeyText.Text = "Vyberte barvu";
                PreviewDarkHex.Text = "";
                PreviewLightHex.Text = "";
                return;
            }

            PreviewKeyText.Text = m_selectedPair.Key;

            if (m_selectedPair.Dark != null)
            {
                PreviewDark.Background = m_selectedPair.Dark.Brush;
                PreviewDarkHex.Text = m_selectedPair.Dark.ColorHex;
            }
            else
            {
                PreviewDark.Background = Brushes.Gray;
                PreviewDarkHex.Text = "N/A";
            }

            if (m_selectedPair.Light != null)
            {
                PreviewLight.Background = m_selectedPair.Light.Brush;
                PreviewLightHex.Text = m_selectedPair.Light.ColorHex;
            }
            else
            {
                PreviewLight.Background = Brushes.Gray;
                PreviewLightHex.Text = "N/A";
            }
        }

        private List<BrushItem> LoadBrushes(string filePath)
        {
            var items = new List<BrushItem>();
            var doc = XDocument.Load(filePath);
            var ns = doc.Root?.GetDefaultNamespace() ?? XNamespace.None;
            var xNamespace = XNamespace.Get("http://schemas.microsoft.com/winfx/2006/xaml");

            // Načtení Color elementů
            foreach (var colorElement in doc.Descendants(ns + "Color"))
            {
                var key = colorElement.Attribute(xNamespace + "Key")?.Value;
                var colorValue = colorElement.Value;

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(colorValue))
                    try
                    {
                        var color = (Color)ColorConverter.ConvertFromString(colorValue);
                        var brush = new SolidColorBrush(color);
                        items.Add(new BrushItem
                        {
                            Key = key,
                            Brush = brush,
                            ColorHex = colorValue,
                            Type = "Color"
                        });
                    }
                    catch
                    {
                        // Ignorovat neplatné barvy
                    }
            }

            // Načtení SolidColorBrush elementů
            foreach (var brushElement in doc.Descendants(ns + "SolidColorBrush"))
            {
                var key = brushElement.Attribute(xNamespace + "Key")?.Value;
                var colorValue = brushElement.Attribute("Color")?.Value;

                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(colorValue))
                    try
                    {
                        var color = (Color)ColorConverter.ConvertFromString(colorValue);
                        var brush = new SolidColorBrush(color);
                        items.Add(new BrushItem
                        {
                            Key = key,
                            Brush = brush,
                            ColorHex = colorValue,
                            Type = "SolidColorBrush"
                        });
                    }
                    catch
                    {
                        // Ignorovat neplatné barvy
                    }
            }

            return items;
        }
    }

    public class BrushItem
    {
        public string Key { get; init; } = string.Empty;
        public SolidColorBrush Brush { get; init; } = Brushes.Black;
        public string ColorHex { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
    }

    public class BrushPairItem
    {
        public string Key { get; init; } = string.Empty;
        public BrushItem? Dark { get; init; }
        public BrushItem? Light { get; init; }
    }
}