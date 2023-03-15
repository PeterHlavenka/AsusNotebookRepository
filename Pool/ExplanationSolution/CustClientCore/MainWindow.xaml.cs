using CustomerInfo;

namespace CustClientCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            var customerLoader = CustomerLoaderProvider.GetCustomerLoader;

            var customer = customerLoader.Invoke();
            
            customer.NecoJsemNapsal += CustomerOnNecoJsemNapsal;  // todo dispose nebo odregistrace
        }

        private void CustomerOnNecoJsemNapsal(object? sender, string e)
        {
            TextBlock.Text = e;
        }
    }
}