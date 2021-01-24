using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Matika.Gui
{
    public interface IConvertable
    {
        int Step { get; }
        Dictionary<int, string> UnitsDictionary { get; }
        bool IsEnabled { get; set; }
        string Name { get; }
        int MaxDifficulty { get; set; }
        BitmapImage HelpImage { get; }
    }
}