using System.Collections.Generic;

namespace Matika.Gui
{
    public interface IConvertable
    {
        int Step { get; }
        Dictionary<int, string> UnitsDictionary { get; }
    }
}