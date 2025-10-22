

using Rabbit.Messaging;

// ConnString se z Environmentu vytahne v MessagingBase.cs a pouzije na vytvoreni kanalu. Value najdes v KeePassu.
// Ve value musi byt IP adresa, ne dns, pac to funguje jen uvnitr clusteru, dokud Borek nevystavi dns ven
Environment.SetEnvironmentVariable("ConnString", "value z meho work keepassu");
var consumer = new RabbitMessageConsumer();
consumer.Worker = DoSomeWork;
// await consumer.StartConsumingAsync(RabbitCommons.GetQueueInfo("Pricing", "CZ", "Production"));

Console.WriteLine("Consuming.");
Console.ReadLine();
return;


async Task DoSomeWork(RabbitMessage message)
{
    await Task.Delay(2000);
}



