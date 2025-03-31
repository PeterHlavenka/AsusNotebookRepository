using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DotVVM_table;

public class RabbitMqService
{
    private readonly ConcurrentQueue<string> m_messages = new();
    private string[] AllQueues { get; } = 
    {
        // "CZ_Production_defaultConsumerQueue",
        // "CZ_Production_AggregationsQueue",
        // "SK_Production_defaultConsumerQueue",
        // "CZ_RC_defaultConsumerQueue",
        // "SK_RC_defaultConsumerQueue",
        "webPageQueue"
    };
    
    public RabbitMqService()
    {
        Initialize(); // todo fireandforget
    }

    public ObservableCollection<string> Messages { get; set; }
    
    private async Task Initialize()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = await factory.CreateConnectionAsync();  // todo dispose / close connection
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
            m_messages.Enqueue(message);
            Messages.Add(message);
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);
    }

    public ConcurrentQueue<string> GetMessages()
    {
        return m_messages;
    }
}