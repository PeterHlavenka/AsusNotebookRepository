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
}