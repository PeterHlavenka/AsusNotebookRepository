using System.Collections.Generic;

namespace Matika.Gui
{
    public class Delka : IConvertable
    {
        public int Step => 10;

        public Dictionary<int, string> UnitsDictionary
        {
            get
            {
                var result = new Dictionary<int, string>
                {
                    {0, "mm"},
                    {1, "cm"},
                    {2, "dm"},
                    {3, "m"},
                    {4, string.Empty},
                    {5, string.Empty},
                    {6, "km"}
                };
                return result;
            }
        }
    }
}