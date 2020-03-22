using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matika.Settings
{
    public class BigNumbersSettingsViewModel : SettingsBase
    {
        public BigNumbersSettingsViewModel(int first, int second)
        {
            FirstNumberSize = first;
            SecondNumberSize = second;
        }
        public int FirstNumberSize { get; set; }
        public int SecondNumberSize { get; set; }
        public bool WholeThousands { get; set; }
        public bool Overlaps { get; set; }
    }
}
