using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Visentio.RabbitMessaging;

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
        const string consumerName = "nejakyconsumer";
        const string country = "CZ";
        const string environment = "Production";
        var importDateTime = GetActualDateTime();
        string[] adwDataIds = [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross];
        
        Environment.SetEnvironmentVariable("RabbitConnectionString", "amqp://default_user_B1BeQMkdhd6tF3Atabz:voIhR72Tmr1MyG4u8sn9Ndki28O9mh7b@10.255.240.241:5672/");
        
        var message = new RabbitMessage
        {
            Country = country,
            Environment = environment,
            DataTypes = adwDataIds,
            ImportDate = importDateTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
        await m_producer.SendMessage(consumerName, country, environment, message);

        while (true)
        {
            var pismeno = Console.ReadLine();
            if (pismeno == "m")
                await SendMultipleMessages(m_producer, consumerName);

            if (pismeno == "n")
            {
                message = new RabbitMessage
                {
                    Country = country,
                    Environment = environment,
                    DataTypes = adwDataIds,
                    ImportDate = GetActualDateTime().ToString("yyyy-MM-dd HH:mm:ss")
                };
                await m_producer.SendMessage(consumerName, country, environment, message);
            }
                
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


    private async Task SendMultipleMessages(RabbitMessageProducer producer, string consumerName)
    {
        var dateTime = GetActualDateTime();
        // CZ
        var message = CreateRabbitMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
        await producer.SendMessage(consumerName, "CZ", "Production", message);
        message = CreateRabbitMessage("CZ", "RC", dateTime, [AdwDataIds.DataCzAdCross]);
        await producer.SendMessage(consumerName, "CZ", "RC", message);
        message = CreateRabbitMessage("SK", "Production", dateTime, [AdwDataIds.DataSkKantarMonitoring]);
        await producer.SendMessage(consumerName, "SK", "Production", message);
        message = CreateRabbitMessage("SK", "RC", dateTime, [AdwDataIds.DataSkKantarTvIndivid]);
        await producer.SendMessage(consumerName, "SK", "RC", message);
    }
    
    private RabbitMessage CreateRabbitMessage(string country, string environment, DateTime importDate, string[] dataTypes)
    {
        return new RabbitMessage
        {
            Country = country,
            Environment = environment,
            ImportDate = importDate.ToString("yyyy-MM-dd HH:mm:ss"),
            DataTypes = dataTypes
        };
    }
}