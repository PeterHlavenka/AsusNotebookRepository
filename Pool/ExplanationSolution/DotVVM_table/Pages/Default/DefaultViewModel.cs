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
    public ObservableCollection<string> Messages => RabbitMqService.Messages;
    
    public ObservableCollection<RabbitMessage> RabbitMessages => RabbitMqService.RabbitMessages;
    
    public string[] Items { get; set; } = { "First", "Second", "Third" };
    public List<string> ItemsList { get; set; }

    public DefaultViewModel(RabbitMqService rabbitMqService)
    {
        RabbitMqService = rabbitMqService;
        Title = "Hello from DotVVM!";
        
        // aby bylo mozne refreshovat stranku kdyz se zmeni ObservableCollection
       // Messages = new ObservableCollection<string>();
        //Messages.CollectionChanged += (_, _) => Context.RedirectToUrl(Context.HttpContext.Request.Path.ToString() ?? string.Empty);
        
        ItemsList = new List<string>(){ "ItemsList from constructor", "Second", "Third" };
       // Messages.Add("V PreRenderu se mi to smaze");
        ItemsList.Add("jak notifikovat");
    }
}