using System.Text;
using System.Text.Json;
using ImportService;
using RabbitCommon;
using RabbitMQ.Client;
using RabbitSender;

// country, environment, dataType, service
string[][] queues =
[
    // fronty musi byt definovany vcetne sluzby, ktera to ma vzit..
    ["CZ", "Production", "aggregations"],
    ["CZ", "Production", "reports"],
    ["CZ", "Production", "pricing"],
    ["SK", "Production", "pricing"],
    ["CZ", "RC", "reports"],
    ["SK", "RC", "aggregations"]
    // web is autmatically added in SendMessage
];

var neco = new RabbitMessageSender();
await neco.Initialize(queues);

await neco.SendMessage("CZ", "Production", GenerateRandomDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);

while (true)
{
    var pismeno = Console.ReadLine();
    if (pismeno == "m")
        await SendMultipleMessages(neco);
    
    if (pismeno == "n")
        await neco.SendMessage("CZ", "Production", GenerateRandomDateTime(), [AdwDataIds.DataCzCsProTrend2, AdwDataIds.DataCzAdCross]);
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



async Task SendMultipleMessages(RabbitMessageSender sender)
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