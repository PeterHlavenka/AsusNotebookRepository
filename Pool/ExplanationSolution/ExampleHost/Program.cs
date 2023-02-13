using System.Reflection;
using System.Runtime.Loader;
using LibraryAsDll;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Create an ordinary instance in the current AppDomain
var localWorker = new Worker();
localWorker.PrintDomain();


// The Worker Service templates create a .NET Generic Host, HostBuilder. The Generic Host can be used with other types of .NET applications, such as Console apps.
// A host is an object that encapsulates an app's resources and lifetime functionality
var hostBuilder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // Configure your services here
        var neco = services.AddHostedService<ExampleHostedService>(); // dependency injection
        


        var provider = services.BuildServiceProvider();
        var foo = provider.GetService<ExampleHostedService>(); //.. returns null

        //var fooneco = foo.Worker;

        var enco = provider.GetService(typeof(ExampleHostedService));
        var test = neco.Last();
        var hej = test.ServiceType;
        var gbji = test.ImplementationInstance;
    });

using (var host = hostBuilder.Build())
{
    // Start your application
    host.RunAsync();

    var ha = host.Services;
    var be = ha.GetServices<ExampleHostedService>();


    // var first = be.First();
    // var ce = first.Worker;
    // ce.PrintDomain();


    host.StopAsync();
}


var context = new AssemblyLoadContext("MyDomain", true);

// var assembly = Assembly.GetAssembly(typeof(Worker));
// var path = assembly?.Location;

var library = Assembly.GetAssembly(typeof(Worker));
// var assembly = context.LoadFromAssemblyName(library.GetName());
var assembly = context.LoadFromAssemblyPath(@"D:\AsusNotebookRepository\Pool\ExplanationSolution\LibraryAsDllCore\bin\Debug\net6.0\LibraryAsDllCore.dll");

var neco = assembly.DefinedTypes.First();
var type = neco?.GetType();


// var instance =
// (assembly.CreateInstance(neco.FullName) as Worker).PrintDomain(); //.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);

// if (instance is Worker inst)
// {
// ((LibraryAsDllCore.Worker)instance).PrintDomain();
// }


Console.WriteLine();


// AppDomain ad = AppDomain.CreateDomain("New domain");
// Worker remoteWorker = (Worker) ad.CreateInstanceAndUnwrap(
//     typeof(Worker).Assembly.FullName,
//     "Worker");
// remoteWorker.PrintDomain();


public class MyService
{
    private readonly IHostedService _myHostedService;

    public MyService(IHostedService myHostedService)
    {
        _myHostedService = myHostedService;
    }

    // Other service logic here
}