using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ComponentCustomizer;

public partial class ToggleButton : UserControl, INotifyPropertyChanged
{
    private Stream m_streamSource;

    public ToggleButton()
    {
        InitializeComponent();


        var svgDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\..\Src\UI\Images\svgs\");
        var svgBytes = File.ReadAllBytes(Path.Combine(svgDirectory, "History.svg"));
        using var stream = new MemoryStream(svgBytes);
        // StreamSource = stream; // binding works
        // SvgControlFromStreamSourceToggleButton.StreamSource = stream; // works only while the stream is not disposed throught using statement

        // best option - copy of stream is created withing SharpVectors library and source stream is disposed. (Should be async)
        SvgControlFromStreamSourceToggleButton.Load(svgBytes); // works 
    }

    public Stream StreamSource
    {
        get => m_streamSource;
        set
        {
            m_streamSource = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public override string ToString()
    {
        return "ToggleButton";
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}