using System.Collections.Generic;

namespace Matika.Gui
{
    public interface IConvertable
    {
        int Step { get; }
        List<string> Units { get; }
    }
}