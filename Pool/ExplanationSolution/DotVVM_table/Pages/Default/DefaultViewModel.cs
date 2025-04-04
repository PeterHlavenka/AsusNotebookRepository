using System.Collections.ObjectModel;
using System.Linq;
using DotVVM.Framework.ViewModel;
using RabbitCommon;

namespace DotVVM_table.Pages.Default;

public class DefaultViewModel : DotvvmViewModelBase
{
    public DefaultViewModel(RabbitMqService rabbitMqService)
    {
        RabbitMqService = rabbitMqService;
        DisplayedRabbitMessages = new ObservableCollection<RabbitMessage>(RabbitMqService.RabbitMessages);
        DisplayedRawMessages = new ObservableCollection<string>(RabbitMqService.RawMessages.TakeLast(50).Reverse().Select(m => m.ToString()));
    }

    public string Title => "Adwind RabbitMQ messages";
    public RabbitMqService RabbitMqService { get; set; }
    public ObservableCollection<RabbitMessage> DisplayedRabbitMessages { get; set; }
    public ObservableCollection<string> DisplayedRawMessages { get; set; }
    public string? SelectedEnvironment { get; set; }
    public string? SelectedCountry { get; set; }
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

    public string[] DataTypes
    {
        get
        {
            var dataTypes = RabbitMqService.RabbitMessages
                .SelectMany(m => m.DataTypes)
                .Distinct()
                .ToList();
            return dataTypes.Prepend("All").ToArray();
        }
    }

    public void FilterChanged(string parameter)
    {
        DisplayedRabbitMessages = new ObservableCollection<RabbitMessage>(
            RabbitMqService.RabbitMessages.Where(m =>
                (string.IsNullOrEmpty(SelectedCountry) || SelectedCountry == "All" || m.Country == SelectedCountry) &&
                (string.IsNullOrEmpty(SelectedEnvironment) || SelectedEnvironment == "All" || m.Environment == SelectedEnvironment) &&
                (string.IsNullOrEmpty(SelectedDataType) || SelectedDataType == "All" || m.DataTypes.Contains(SelectedDataType))
            )
        );

        DisplayedRawMessages = new ObservableCollection<string>(
            RabbitMqService.RawMessages.TakeLast(50)
                .Where(m => (string.IsNullOrEmpty(SelectedCountry) || SelectedCountry == "All" || m.Country == SelectedCountry) &&
                            (string.IsNullOrEmpty(SelectedEnvironment) || SelectedEnvironment == "All" || m.Environment == SelectedEnvironment) &&
                            (string.IsNullOrEmpty(SelectedDataType) || SelectedDataType == "All" || m.DataTypes.Contains(SelectedDataType)))
                .Reverse()
                .Select(m => m.ToString())
        );
    }
}