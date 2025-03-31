using RabbitMQ.Client;

// country, environment, dataType, service
string[][] queues =
[
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




// Sending part:
var dateTime = GenerateRandomDateTime();
// CZ
await SendMessage("CZ", "Production", "aggregations", dateTime);
await SendMessage("CZ", "Production", "reports", dateTime);
await SendMessage("CZ", "Production", "pricing", dateTime);
await SendMessage("CZ", "RC", "reports", dateTime);
// SK
await SendMessage("SK", "Production", "pricing", dateTime);
await SendMessage("SK", "RC", "aggregations", dateTime);
Console.ReadLine();





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

async Task SendMessage(string environment, string country, string consumerName, DateTime importDateTime)
{
    // Send message to the exchange with routing key "import.{environment}.{country}"
    var routingKey = $"{environment}.{country}.{consumerName}";
    var message = $"Imported date {importDateTime:yyyy-MM-dd HH:mm:ss} for {routingKey}";
    var body = System.Text.Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchangeName, routingKey, body);
    Console.WriteLine($@"Sending message: {message}");
    
    // a stejnou zpravu poslu na webovou stranku
    var web = "webPage";
    routingKey = $"{environment}.{country}.{web}";
    await channel.BasicPublishAsync(exchangeName, routingKey, body);
}

async Task DeclareAndBindQueues()
{
    foreach (var queue in queues)
    {
        var queueName = $"{queue[0]}_{queue[1]}_{queue[2]}Queue";
        await channel.QueueDeclareAsync(queueName, true, false, false);
        var routingKey = $"{queue[0]}.{queue[1]}.{queue[2]}";
        await channel.QueueBindAsync(queueName, exchangeName, routingKey);
    }
}

