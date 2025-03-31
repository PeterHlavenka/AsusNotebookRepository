using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.ViewModel;
using Microsoft.AspNetCore.Diagnostics;

namespace DotVVM_table.Pages.Error;

public class ErrorViewModel : DotvvmViewModelBase
{
    [Bind(Direction.None)] public string? RequestId { get; set; }

    [Bind(Direction.None)] public string? ExceptionType { get; set; }

    [Bind(Direction.None)] public string? RequestPath { get; set; }


    public ErrorViewModel()
    {
    }

    public override Task Init()
    {
        var aspcontext = Context.GetAspNetCoreContext();
        var exceptionInfo = aspcontext.Features.Get<IExceptionHandlerFeature>();
        if (exceptionInfo is null)
        {
            ExceptionType = "View called without IExceptionHandlerFeature";
            return base.Init();
        }

        ExceptionType = exceptionInfo.Error.GetBaseException().GetType().Name;
        RequestId = aspcontext.TraceIdentifier;
        RequestPath = exceptionInfo.Path;
        return base.Init();
    }
}