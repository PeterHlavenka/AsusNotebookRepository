using System.Windows;
using CustomerInfo;

namespace CustServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CustomerLoader m_customer;

        // HttpServerChannel hts ;
        public MainWindow()
        {
            InitializeComponent();

            var customerLoader = CustomerLoaderProvider.GetCustomerLoader;

            m_customer = customerLoader.Invoke();
            
            

            // customerLoader.Init();
            // customerLoader.GetRow();
            // customerLoader.ExecuteSelectCommand("nejaky select command");
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            m_customer.OnNecoJsemNapsal("hahaha");
        }
    }
}