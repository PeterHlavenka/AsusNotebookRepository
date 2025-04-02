using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DotVVM.Framework.ViewModel;

namespace DotVVM_table.Pages.Default;

public class DefaultViewModel : DotvvmViewModelBase
{
    public RabbitMqService RabbitMqService { get; set;}

    public string Title { get; set; }
    
    public DefaultViewModel(RabbitMqService rabbitMqService)
    {
        RabbitMqService = rabbitMqService;
        Title = "RabbitMQ messages";
    }
    
    public string[] Environments { get; set; } = { "Apple", "Banana", "IceCream", "Orange" };

    public string SelectedEnvironment { get; set; }

    public string Message { get; set; }

    public void IsItReallyFruit()
    {
        if (SelectedEnvironment == "IceCream")
        {
            Message = "Ice cream isn't a fruit!";
        }
        else
        {
            Message = "Yes, it's a fruit!";
        }
    }
}