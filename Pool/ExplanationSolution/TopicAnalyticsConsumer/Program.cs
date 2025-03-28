using RabbitMQ.Client;
using RabbitMQ.Client.Events;

// nejprve zkousim  ExchangeType.DIRECT !!!!

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// 1) jako prvni potrebujeme odstranit deklaraci fronty
// await channel.QueueDeclareAsync(
//     "persistent_Letterbox",
//     true,
//     false,
//     false);

// 2) misto toho deklarujeme exchange
await channel.ExchangeDeclareAsync("myroutingexchange", ExchangeType.Direct, true, false);
var queueName = (await channel.QueueDeclareAsync()).QueueName;

// 3) musime frontu nabindovat na exchange
await channel.QueueBindAsync(queue: queueName, exchange: "myroutingexchange", routingKey: "analyticsonly");

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = System.Text.Encoding.UTF8.GetString(body);
    Console.WriteLine($" [Analytics] Received {message}");
    await Task.Delay(1000);
    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
Console.WriteLine(" Analytics - consuming.");
Console.ReadLine();