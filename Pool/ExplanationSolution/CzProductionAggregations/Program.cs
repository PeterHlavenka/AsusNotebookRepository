

using Rabbit.Messaging;

// ConnString se z Environmentu vytahne v MessagingBase.cs a pouzije na vytvoreni kanalu. Value najdes v KeePassu.
// Ve value musi byt IP adresa, ne dns, pac to funguje jen uvnitr clusteru, dokud Borek nevystavi dns ven
Environment.SetEnvironmentVariable("RabbitConnectionString", "amqp://default_user_B1BeQMkdhd6tF3Atabz:voIhR72Tmr1MyG4u8sn9Ndki28O9mh7b@10.255.240.241:5672/");
var consumer = new RabbitMessageConsumer();
consumer.Worker = DoSomeWork;

var queueInfo = new string[] { "CZ", "Production", "Testovaci consumer" };
var queueName = consumer.CreateQueueName(queueInfo[0], queueInfo[1], queueInfo[2]);
var routingKey = consumer.CreateRoutingKey(queueInfo[0], queueInfo[1], queueInfo[2]);
await consumer.StartConsumingAsync(queueName, routingKey);

// Preposlani zpravy na dalsi sluzby

Console.WriteLine("Consuming.");
Console.ReadLine();
return;




async Task DoSomeWork(RabbitMessage message)
{
    await Task.Delay(2000);
    Console.WriteLine($"Message received: {message}");
}