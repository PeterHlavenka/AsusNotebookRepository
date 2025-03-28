using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// 1) deklarujeme exchange
await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);
var queueName = (await channel.QueueDeclareAsync()).QueueName;

// 3) musime frontu nabindovat na exchange
await channel.QueueBindAsync(queue: queueName, exchange: "importExchange", routingKey: "SK.production");

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