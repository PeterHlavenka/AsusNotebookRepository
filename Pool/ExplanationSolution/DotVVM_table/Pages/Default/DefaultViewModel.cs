using System.Collections.ObjectModel;
using System.Linq;
using DotVVM.Framework.ViewModel;

namespace DotVVM_table.Pages.Default;

public class DefaultViewModel : DotvvmViewModelBase
{
    public DefaultViewModel(RabbitMqService rabbitMqService)
    {
        RabbitMqService = rabbitMqService;
        DisplayedRabbitMessages = new ObservableCollection<RabbitMessage>(RabbitMqService.RabbitMessages);
        DisplayedRawMessages = new ObservableCollection<string>(RabbitMqService.RawMessages.TakeLast(50).Reverse());
        Title = "RabbitMQ messages";
    }

    public string Title { get; set; }
    public RabbitMqService RabbitMqService { get; set; }
    public ObservableCollection<RabbitMessage> DisplayedRabbitMessages { get; set; }
    public ObservableCollection<string> DisplayedRawMessages { get; set; }
    public string? SelectedEnvironment { get; set; }
    public string? SelectedCountry { get; set; }
    public string? SelectedService { get; set; }
    public string? SelectedDataType { get; set; }
    
    public string[] Countries
    {
        get
        {
            var countries = RabbitMqService.RabbitMessages
                .Select(m => m.Country)
                .Distinct()
                .ToArray();
            return countries.Prepend("All").ToArray();
        }
    }

    public string[] Environments
    {
        get
        {
            var environments = RabbitMqService.RabbitMessages
                .Select(m => m.Environment)
                .Distinct()
                .ToArray();
            return environments.Prepend("All").ToArray();
        }
    }

    public string[] Services
    {
        get
        {
            var serviceNames = RabbitMqService.RabbitMessages
                .Select(m => m.ServiceName)
                .Distinct()
                .ToArray();
            return serviceNames.Prepend("All").ToArray();
        }
    }

    public string[] DataTypes
    {
        get
        {
            var dataTypes = RabbitMqService.RabbitMessages
                .Select(m => m.DataType)
                .Distinct()
                .ToArray();
            return dataTypes.Prepend("All").ToArray();
        }
    }
    
    public void FilterChanged(string parameter)
    {
        DisplayedRabbitMessages = new ObservableCollection<RabbitMessage>(
            RabbitMqService.RabbitMessages.Where(m =>
                (string.IsNullOrEmpty(SelectedCountry) || SelectedCountry == "All" || m.Country == SelectedCountry) &&
                (string.IsNullOrEmpty(SelectedEnvironment) || SelectedEnvironment == "All" || m.Environment == SelectedEnvironment) &&
                (string.IsNullOrEmpty(SelectedService) || SelectedService == "All" || m.ServiceName == SelectedService) &&
                (string.IsNullOrEmpty(SelectedDataType) || SelectedDataType == "All" || m.DataType == SelectedDataType)
            )
        );
        
        DisplayedRawMessages = new ObservableCollection<string>(
            RabbitMqService.RawMessages.TakeLast(50).Where(m =>
                (string.IsNullOrEmpty(SelectedCountry) || SelectedCountry == "All" || m.Split('_')[1] == SelectedCountry) &&
                (string.IsNullOrEmpty(SelectedEnvironment) || SelectedEnvironment == "All" || m.Split('_')[2] == SelectedEnvironment) &&
                (string.IsNullOrEmpty(SelectedService) || SelectedService == "All" || m.Split('_')[3] == SelectedService) &&
                (string.IsNullOrEmpty(SelectedDataType) || SelectedDataType == "All" || m.Split('_')[4] == SelectedDataType)
            ).Reverse()
        );
    }
}