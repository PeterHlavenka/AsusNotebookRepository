
using ImportService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbit.Messaging;
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
            "amqp://default_user_B1BeQMkdhd6tF3Atabz:voIhR72Tmr1MyG4u8sn9Ndki28O9mh7b@10.255.240.241:5672/"));
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