
using Rabbit.Messaging;

const string connString = "amqp://phlavenka:LLykoat3J9HbDBUAjVW3@rmq.prod:5672/adw-test";
var consumer = new RabbitMessageConsumer(connString);
consumer.Worker = DoSomeWork;
await consumer.StartConsumingAsync(RabbitCommons.GetQueueInfo(ImportConsumer.Reports, "CZ", "Production"));

Console.WriteLine("Consuming.");
Console.ReadLine();
return;


async Task DoSomeWork(RabbitMessage message)
{
    await Task.Delay(2000);
}