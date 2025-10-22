using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rabbit.Messaging;

namespace ImportService;

public class Sender : BackgroundService
{
    private readonly ILogger<Sender> m_logger;
    private readonly RabbitMessageProducer m_producer;

    public Sender(RabbitMessageProducer producer, ILogger<Sender> logger)
    {
        m_producer = producer;
        m_logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Environment.SetEnvironmentVariable("RabbitConnectionString", "amqp://default_user_B1BeQMkdhd6tF3Atabz:voIhR72Tmr1MyG4u8sn9Ndki28O9mh7b@10.255.240.241:5672/");
        await m_producer.SendMessage("CZ", "Production", GetActualDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);

        while (true)
        {
            var pismeno = Console.ReadLine();
            if (pismeno == "m")
                await SendMultipleMessages(m_producer);

            if (pismeno == "n")
                await m_producer.SendMessage("CZ", "Production", GetActualDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
        }
    }

    private DateTime GenerateRandomDateTime()
    {
        // generata random date and time between 2023-01-01 and 2023-12-31 
        var random = new Random();
        var year = 2025;
        var month = random.Next(1, 13);
        var day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
        var hour = random.Next(0, 24);
        var minute = random.Next(0, 60);
        var second = random.Next(0, 60);
        return new DateTime(year, month, day, hour, minute, second);
    }

    private static DateTime GetActualDateTime()
    {
        return DateTime.Now;
    }


    private async Task SendMultipleMessages(RabbitMessageProducer sender)
    {
        var dateTime = GetActualDateTime();
        // CZ
        await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
        await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzMrTvIndivid]);
        await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzPemd]);
        await sender.SendMessage("CZ", "RC", dateTime, [AdwDataIds.DataCzAdCross]);
// neco.
        await sender.SendMessage("SK", "Production", dateTime, [AdwDataIds.DataSkKantarMonitoring]);
        await sender.SendMessage("SK", "RC", dateTime, [AdwDataIds.DataSkKantarTvIndivid]);
    }
}