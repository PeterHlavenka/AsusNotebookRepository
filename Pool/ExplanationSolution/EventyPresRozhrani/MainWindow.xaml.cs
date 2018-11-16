using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EventyPresRozhrani
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Tato trida je tu jakoby navic abychom meli kde vytvorit instance normalne je resolvne Castle
        public MainWindow()
        {
            InitializeComponent();

            
            Posilac posilac = new Posilac();

            // Posluchac dostane posilace jako rozhrani (To pozaduje v konstruktoru)
            Posluchac posluchac = new Posluchac(posilac);

            // Vyhodime event:
            posilac.OnMainChanged();
        }
    }

    public class Posilac : ISelector
    {
        // Posilac musi mit event definovany v rozhrani
        public event EventHandler MyEvent;

        // a vyhazovaci metodu
        public void OnMainChanged()
        {
            MyEvent?.Invoke(this, null);
        }
    }

    public class Posluchac
    {
        // Rozhrani dostaneme v konstruktoru
        public Posluchac(ISelector selector)
        {
            // Trida ktera implementuje rozhrani musi mit event a vyhazovaci metodu
            selector.MyEvent += Metoda;
        }

        private void Metoda(object sender, EventArgs e)
        {
            Console.WriteLine($@"Tato metoda se provede kdyz se vyhodi event");
        }
    }
}
