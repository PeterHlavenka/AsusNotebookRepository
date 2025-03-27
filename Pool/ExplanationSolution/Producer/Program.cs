

using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory() { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "persistent_Letterbox", durable: true, exclusive: false, autoDelete: false, arguments: null);

var message = "Hello World!";

var body = Encoding.UTF8.GetBytes(message);

var properties = new BasicProperties
{
    Persistent = true
};

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "persistent_Letterbox", mandatory: true,
    basicProperties: properties, body: body);

Console.WriteLine($" [x] Sent {message}");
Console.WriteLine("Prerequisites: Make sure RabbitMQ is running on localhost:5672");
Console.WriteLine("You can show messages in the queue on http://localhost:8080/#/queues");
Console.ReadLine();