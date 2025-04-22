using Adwind.Rabbit.Messaging;

var consumer = new RabbitMessageConsumer(DoSomeWork);
await consumer.StartConsumingAsync(RabbitCommons.GetQueueInfo(ImportConsumer.Pricing, "CZ", "Production"));

Console.WriteLine("Consuming.");
Console.ReadLine();
return;


async Task DoSomeWork(RabbitMessage message)
{
    await Task.Delay(2000);
}



