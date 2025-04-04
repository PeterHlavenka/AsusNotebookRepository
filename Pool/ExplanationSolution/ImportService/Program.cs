using System.Text.Json;
using ImportService;
using RabbitCommon;
using RabbitMQ.Client;

// country, environment, dataType, service
string[][] queues =
[
    // fronty musi byt definovany vcetne sluzby, ktera to ma vzit..
    ["CZ","Production", "aggregations"],
    ["CZ","Production", "reports"],
    ["CZ","Production", "pricing"],
    ["SK","Production", "pricing"],
    ["CZ","RC", "reports"],
    ["SK","RC", "aggregations"]
    // web is autmatically added in SendMessage
];

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

// 1) Deklarujeme exchange
var exchangeName = "importExchange";  
await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, false);

// 2) Deklarace vsech front a jejich binding na exchange:
await DeclareAndBindQueues();




await SendMultipleMessages();

while (true)
{
    if (Console.ReadLine()?.ToLower() == "n")
        await SendMessage("CZ", "Production", GenerateRandomDateTime(), AdwDataIds.DataCzCsProTrend2);
    if (Console.ReadLine()?.ToLower() == "m")
        await SendMultipleMessages();
}



DateTime GenerateRandomDateTime()
{
    // generata random date and time between 2023-01-01 and 2023-12-31 
    Random random = new Random();
    int year = 2023;
    int month = random.Next(1, 13);
    int day = random.Next(1, DateTime.DaysInMonth(year, month) + 1);
    int hour = random.Next(0, 24);
    int minute = random.Next(0, 60);
    int second = random.Next(0, 60);
    return new DateTime(year, month, day, hour, minute, second);
}

async Task SendMessage(string country, string environment, DateTime importDateTime, string adwDataId)
{
    // Send message to the exchange with routing key "import.{environment}.{country}"
    var routingKey = $"{environment}.{country}";
    var rabbitMessage = new RabbitMessage
    {
        Country = country, 
        Environment = environment, 
        DataType = adwDataId, 
        ImportDate = importDateTime.ToString("yyyy-MM-dd HH:mm:ss")
    };
    var message = JsonSerializer.Serialize(rabbitMessage);
    var body = System.Text.Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchangeName, routingKey, body);
    
    // a stejnou zpravu poslu na webovou stranku
    var web = "webPage";
    routingKey = $"{environment}.{country}.{web}";
    Console.WriteLine("Sending message to web page");
    await channel.BasicPublishAsync(exchangeName, routingKey, body);
}

async Task SendMultipleMessages()
{
    var dateTime = GenerateRandomDateTime();
    // CZ
    await SendMessage("CZ", "Production", dateTime, AdwDataIds.DataCzCsProTrend2);
    await SendMessage("CZ", "Production", dateTime, AdwDataIds.DataCzMrTvIndivid);
    await SendMessage("CZ", "Production", dateTime, AdwDataIds.DataCzPemd);
    await SendMessage("CZ", "RC", dateTime, AdwDataIds.DataCzAdCross);
    // SK
    await SendMessage("SK", "Production", dateTime, AdwDataIds.DataSkKantarMonitoring);
    await SendMessage("SK", "RC", dateTime, AdwDataIds.DataSkKantarTvIndivid);
}

async Task DeclareAndBindQueues()
{
    foreach (var queue in queues)
    {
        var queueName = $"{queue[0]}_{queue[1]}_{queue[2]}Queue";
        await channel.QueueDeclareAsync(queueName, true, false, false);
        // v routovacim klici nebude definovany consumer (to je zname v deklaraci fronty). Message jde na vsechny consumery.
        var routingKey = $"{queue[0]}.{queue[1]}"; 
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);
    }
}

