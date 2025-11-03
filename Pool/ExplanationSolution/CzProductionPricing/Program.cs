using Visentio.RabbitMessaging;


Environment.SetEnvironmentVariable("RabbitConnectionString", "amqp://default_user_B1BeQMkdhd6tF3Atabz:voIhR72Tmr1MyG4u8sn9Ndki28O9mh7b@10.255.240.241:5672/");

// V proměnných prostředí musí být RabbitConnectionString

var consumer = new RabbitMessageConsumer();
consumer.Worker = DoSomeWork;

const string consumerName = "Nazev consumera";
const string country = "CZ";
const string environment = "Production";

var queueName = consumer.CreateQueueName(consumerName, country, environment);
var routingKey = consumer.CreateRoutingKey(consumerName, country, environment);

// Tento consumer bude poslouchat na fronte s názvem queueName a klíčem routingKey
await consumer.StartConsumingAsync(queueName, routingKey);

Console.WriteLine("Consuming.");
Console.ReadLine();
return;

async Task DoSomeWork(RabbitMessage message)
{
    await Task.Delay(2000);
    Console.WriteLine($"Message received: {message}");
}