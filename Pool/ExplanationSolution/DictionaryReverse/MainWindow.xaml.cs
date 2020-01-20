using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace DictionaryReverse
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var prices = new Dictionary<int, int>();
            prices.Add(1, 100);
            prices.Add(2, 200);
            prices.Add(3, 100);
            prices.Add(4, 300);

            // Otoci dictionary, klicem ted bude to co bylo ve value a zgrupuje podle noveho klice
            // V prices jsou 4 skupiny, v groups jen 3, protoze stovka je tam dvakrat
            var groups = prices.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.Select(i => i.Key).ToList());

            var neco = new Dictionary<int, List<int>>();

            neco.Add(1, new List<int> {100, 200}); // stejne
            neco.Add(2, new List<int> {100, 300});
            neco.Add(3, new List<int> {100, 200}); //stejne
            neco.Add(4, new List<int> {100, 400});
            neco.Add(5, new List<int> {600, 500});
            neco.Add(6, new List<int> {100, 200}); // stejne


            // Pokuk chci groupnout podle kolekce, musim ji prevest na string a groupovat podle nej
            var klicemJeString = neco.GroupBy(x => string.Join(",", x.Value.OrderBy(z => z))).ToDictionary(x => x.Key, x => x.Select(i => i.Key).ToList());


            Prices = klicemJeString;

            DataContext = Prices;
        }

        public Dictionary<string, List<int>> Prices { get; set; }
    }
}