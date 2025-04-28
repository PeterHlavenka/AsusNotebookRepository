using Adwind.Rabbit.Messaging;
using ImportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

try
{
    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", true, true);
        config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
        config.AddJsonFile("serilogSettings.json", false, true);
        hostingContext.Configuration = config.Build();
    });

    builder.ConfigureServices((context, services) =>
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(context.Configuration)
            .CreateLogger();
        var loggerFactory = LoggerFactory.Create(bld => { bld.AddSerilog(logger); });
        services.AddSingleton(loggerFactory);
        
        // Producer musi dostat connection string na rabbita vystaveneho ven z clusteru
        services.Configure<Options>(context.Configuration.GetSection("Options").Bind);
        services.AddSingleton<RabbitMessageProducer>(_ => new RabbitMessageProducer(
            context.Configuration.GetSection("Options:ConnString").Value,
            loggerFactory.CreateLogger<RabbitMessageProducer>()));
        services.AddHostedService<Sender>();
    });

    var app = builder.Build();
    Log.Information("Starting Import service simulation");
    app.Run();
}
catch (Exception ex)
{ 
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}