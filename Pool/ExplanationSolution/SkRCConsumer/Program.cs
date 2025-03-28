using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// 1) deklarujeme exchange
await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);

// 2) deklarace fronty
const string queueName = "SK_RC_defaultConsumerQueue";
await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

// 3) musime frontu nabindovat na exchange
await channel.QueueBindAsync(queue: queueName, exchange: "importExchange", routingKey: "SK.RC");

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($"User - Received {message}");
    await Task.Delay(1000);
    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
Console.WriteLine(" consuming.");
Console.ReadLine();