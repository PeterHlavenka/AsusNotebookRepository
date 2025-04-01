using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Threading;

namespace DotVVM_table;

public class RabbitMqService
{
    private const string Separator = "_";
    private static readonly ILogger<RabbitMqService> m_log = new Logger<RabbitMqService>(new LoggerFactory());
    private ObservableCollection<string> m_messages1 = new();

    public RabbitMqService()
    {
        Initialize().FireAndForgetSafeAsync(m_log.LogError, false);
    }

    public ObservableCollection<string> Messages
    {
        get => m_messages1;
        set
        {
            m_messages1 = value;

            RabbitMessages = new ObservableCollection<RabbitMessage>(m_messages1.Select(m =>
            {
                var parts = m.Split(Separator);
                return new RabbitMessage
                {
                    Country = parts[0],
                    Environment = parts[1],
                    ServiceName = parts[2],
                    ImportDate = parts[3],
                    DataType = parts[4]
                };
            }));
        }
    }

    public ObservableCollection<RabbitMessage> RabbitMessages { get; set; } = new();

    private async Task Initialize()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync(); // todo dispose / close connection
        var channel = await connection.CreateChannelAsync();

        // 1) deklarujeme exchange
        await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);

        // 2) deklarace fronty
        const string queueName = "webPageQueue";
        await channel.QueueDeclareAsync(queueName, true, false, false);

        // 3) musime frontu nabindovat na exchange
        await channel.QueueBindAsync(queueName, "importExchange", "#.webPage");


        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Messages.Add(message);
            
            var parts = message.Split(Separator);
            RabbitMessages.Add(new RabbitMessage
            {
                ImportDate = parts[0],
                Country = parts[1],
                Environment = parts[2],
                ServiceName = parts[3],
                DataType = parts[4]
            });
            
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }
}