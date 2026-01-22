using System.Windows.Controls;
using System.Windows.Media;

namespace ComponentCustomizer;

public partial class UpDown : UserControl
{
    public UpDown()
    {
        InitializeComponent();
    }
    
    public override string? ToString()
    {
        return "UpDown";
    }
}