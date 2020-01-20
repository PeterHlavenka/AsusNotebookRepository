using System;
using System.Windows;

namespace EventyPresRozhrani
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Tato trida je tu jakoby navic abychom meli kde vytvorit instance normalne je resolvne Castle
        public MainWindow()
        {
            InitializeComponent();


            var posilac = new Posilac();

            // Posluchac dostane posilace jako rozhrani (To pozaduje v konstruktoru)
            var posluchac = new Posluchac(posilac);

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
            Console.WriteLine(@"Tato metoda se provede kdyz se vyhodi event");
        }
    }
}