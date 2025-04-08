using System.Text;
using System.Text.Json;
using RabbitCommon;
using RabbitMQ.Client;

namespace RabbitSender;

public class RabbitMessageSender : IDisposable
{
    private const string DataExchangeName = "importExchange";
    private IConnection m_connection = null!;
    private IChannel m_channel = null!;

    public async Task Initialize(string[][] queues)
    {
        // 0) Create connection and channel
        var factory = new ConnectionFactory { HostName = "localhost" };
        m_connection = await factory.CreateConnectionAsync();
        m_channel = await m_connection.CreateChannelAsync();

        // 1) Declare exchange
        await m_channel.ExchangeDeclareAsync(DataExchangeName, ExchangeType.Topic, true, false);

        // 2) Declare all queues and bind them to the exchange
        foreach (var queue in queues)
        {
            var queueName = $"{queue[0]}_{queue[1]}_{queue[2]}Queue";
            await m_channel.QueueDeclareAsync(queueName, true, false, false);
            // The routing key does not define the consumer (this is known in the queue declaration).
            // The message goes to all consumers.
            var routingKey = $"{queue[0]}.{queue[1]}";
            await m_channel.QueueBindAsync(queueName, DataExchangeName, routingKey);
        }
    }

    public async Task SendMessage(string country, string environment, DateTime importDateTime, string[] adwDataIds)
    {
        // Send message to the exchange with routing key "import.{environment}.{country}"
        var routingKey = $"{environment}.{country}";
        var rabbitMessage = new RabbitMessage
        {
            Country = country,
            Environment = environment,
            DataTypes = adwDataIds,
            ImportDate = importDateTime.ToString("yyyy-MM-dd HH:mm:ss")
        };
        var message = JsonSerializer.Serialize(rabbitMessage);
        var body = Encoding.UTF8.GetBytes(message);
        await m_channel.BasicPublishAsync(DataExchangeName, routingKey, body);

        // Send the same message to the web page
        var web = "webPage";
        routingKey = $"{environment}.{country}.{web}";
        Console.WriteLine("Sending message to web page");
        await m_channel.BasicPublishAsync(DataExchangeName, routingKey, body);
    }
    
    public void Dispose()
    {
        m_channel.Dispose();
        m_connection.Dispose();
    }
}