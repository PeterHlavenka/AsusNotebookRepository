using System.Windows.Controls;
using System.Windows.Media;

namespace ComponentCustomizer
{
    public partial class TextBox : UserControl
    {
        public TextBox()
        {
            InitializeComponent();
        }
        
        public override string? ToString()
        {
            return "TextBox";
        }
    }
}