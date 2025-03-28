using System.Text;
using RabbitMQ.Client;

namespace RabbitMQMessageSender;

public class MessageSender
{
    public async Task SendMessage(string message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync("persistent_Letterbox", true, false, false, null);
        
        var body = Encoding.UTF8.GetBytes(message);
        var properties = new BasicProperties
        {
            Persistent = true
        };

        await channel.BasicPublishAsync(string.Empty, "persistent_Letterbox", true,
            properties, body);

        Console.WriteLine($" [x] Sent {message}");
    }
}