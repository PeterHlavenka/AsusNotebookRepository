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

namespace Eventy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
           
            // 4. Zaregistrujeme posluchace
            MyEvent += Posluchac;

            // 6. Zaregistrujeme posluchace v jine tride
            VnitrniTridaPosluchac tridaPosluchac = new VnitrniTridaPosluchac();            
            MyEvent += tridaPosluchac.PosluchacVJineTride;

            
        }

        
        // 1. Deklarujeme event v posilaci tride
        public event EventHandler MyEvent;

        // 2. Deklarujeme metodu ktera Invokuje event . Mohla by se jmenovat FireEvent
        public void OnMainChanged()
        {
            MyEvent?.Invoke(this, null);
        }

        // 3. Obsluzna metoda buttonu. Event vyhazujeme zmacknutim tlacitka, tady se zavola vyhazovaci metoda.
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OnMainChanged();
        }

        // 5. Metoda posluchace, zaregistrovana v konstruktoru
        public void Posluchac(object sender, EventArgs e)
        {
            Console.WriteLine($@"Dostal jsem event");
        }
    }

    public class VnitrniTridaPosluchac
    {
        public void PosluchacVJineTride(object sender, EventArgs e)
        {
            Console.WriteLine($@"Dostal jsem event a jsem pritom v jine tride");
        }
    }

    // EVENTY PRES ROZHRANI JSOU V NASLEDUJICIM PROJEKTU 
}
