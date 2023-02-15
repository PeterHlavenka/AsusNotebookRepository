using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Client48;

/// <summary>
///     Interaction logic for Client.xaml
/// </summary>
public partial class Client
{
    public Client()
    {
        InitializeComponent();
        ClientTextBox.Text = "After initialize";

        StartHost();
    }

    private async void StartHost()
    {
        ClientTextBox.Text = "Starting";

        var host = Host.CreateDefaultBuilder(null)
            .ConfigureServices(services =>
            {
                services.AddHostedService<Worker>();
                services.AddSingleton(ClientTextBox);
            })
            .Build();

        await host.RunAsync();
    }
}