using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlock.Text = "Running";
            //tato moznost vola metodu bez navratoveho typu
            await RunCompute();


            //tato moznost vola asynchronni metodu rovnou. Jeji navratovy typ je int
            //int result = await ComputeAsync();            
            //TextBlock.Text = result.ToString();

            TextBlock.Text = @" pokracuju po zavolani asynchronni metody";
        }

        // dlouhotrvajici uloha
        private int Compute()
        {
            Thread.Sleep(3000);
            return 42;
        }

        //A protože je tento výpočet tak náročný, možná by bylo dobré mít k dispozici také asynchronní verzi této metody:
        private Task<int> ComputeAsync()
        {
            return Task.Run(() =>
            {
                Thread.Sleep(3000);
                return 42;
            });
        }

        //Jakým způsobem metoda dokáže to, že je asynchronní, nám v mnoha případech může zůstat uvnitř metody skryto (1). 
        //Důležité je však to, že ze signatury metody resp. přímo toho, že její návratová hodnota je typu Task nebo Task<T> poznáme, že je jedná o asynchronní metodu (2).

        public async Task RunCompute() //nic nevraci
        {
            //SyncComputation
            // var result = Compute();

            //Async Computation
            var result = await ComputeAsync();

            TextBlock.Text = result.ToString();
        }

        private void Sync_OnClick(object sender, RoutedEventArgs e)
        {
            TextBlock.Text = "Running";
            Compute();
        }
    }
}