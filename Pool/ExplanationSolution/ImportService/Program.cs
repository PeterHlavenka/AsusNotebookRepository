using Adwind.Rabbit.Messaging;
using ImportService;

// country, environment, dataType, service


var producer = new RabbitMessageProducer();
await producer.Initialize();

await producer.SendMessage("CZ", "Production", GenerateRandomDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);

while (true)
{
    var pismeno = Console.ReadLine();
    if (pismeno == "m")
        await SendMultipleMessages(producer);

    if (pismeno == "n")
        await producer.SendMessage("CZ", "Production", GenerateRandomDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
}


DateTime GenerateRandomDateTime()
{
    // generata random date and time between 2023-01-01 and 2023-12-31 
    var random = new Random();
    var year = 2025;
    var month = random.Next(1, 13);
    var day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
    var hour = random.Next(0, 24);
    var minute = random.Next(0, 60);
    var second = random.Next(0, 60);
    return new DateTime(year, month, day, hour, minute, second);
}


async Task SendMultipleMessages(RabbitMessageProducer sender)
{
    var dateTime = GenerateRandomDateTime();
    // CZ
    await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
    await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzMrTvIndivid]);
    await sender.SendMessage("CZ", "Production", dateTime, [AdwDataIds.DataCzPemd]);
    await sender.SendMessage("CZ", "RC", dateTime, [AdwDataIds.DataCzAdCross]);
// neco.
    await sender.SendMessage("SK", "Production", dateTime, [AdwDataIds.DataSkKantarMonitoring]);
    await sender.SendMessage("SK", "RC", dateTime, [AdwDataIds.DataSkKantarTvIndivid]);
}