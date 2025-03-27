using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// aby se fronta neztratila pri restartu serveru, nastavime durable na true
await channel.QueueDeclareAsync(queue: "persistent_Letterbox", durable: true, exclusive: false, autoDelete: false, arguments: null);
// timto reknu, ze tento consumer nebude dostavat dalsi zpravu, dokud nezpracuje tu co ma a neackne ji
await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += async (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    await Task.Delay(1000);
    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queue: "persistent_Letterbox", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();



// v tomto stavu kdyz producer vyprodukuje message, tak ji sezere jen jeden consumer. Ktery to bude je nahodne.. 