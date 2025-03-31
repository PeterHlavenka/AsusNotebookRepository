using DotVVM.Framework.ViewModel;

namespace DotVVM_table.Pages.Default;

public class DefaultViewModel : DotvvmViewModelBase
{
    public string Title { get; set; }

    public DefaultViewModel()
    {
        Title = "Hello from DotVVM!";
    }
}