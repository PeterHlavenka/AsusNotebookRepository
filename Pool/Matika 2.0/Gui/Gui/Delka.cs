using System.Collections.Generic;

namespace Matika.Gui
{
    public class Delka : IConvertable
    {
        public int Step => 10;
        public List<string> Units => new List<string>{"mm", "cm", "dm", "m", "null", "null", "km"};
    }
}