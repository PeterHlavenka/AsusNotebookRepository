using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

                                // 1) Deklarujeme exchange
var exchangeName = "importExchange";  // exchange na jednotlive sluzby - routing key rekne ktera sluzba ma zpracovat zpravu
await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, false);

// webPage
var webExchangeName = "webPageExchange"; // exchange na webovou stranku - routing key bude # a vsechny zpravu pujdou i sem, ale jen jednou..
await channel.ExchangeDeclareAsync(webExchangeName, ExchangeType.Topic, true, false);


                                // 2) Deklarace front:
// Production:
await channel.QueueDeclareAsync("CZ_Production_defaultConsumerQueue", true, false, false);
await channel.QueueDeclareAsync("CZ_Production_AggregationsQueue", true, false, false);
await channel.QueueDeclareAsync("SK_Production_defaultConsumerQueue", true, false, false);
// RC:
await channel.QueueDeclareAsync("CZ_RC_defaultConsumerQueue", true, false, false);
await channel.QueueDeclareAsync("SK_RC_defaultConsumerQueue", true, false, false);
// webPage
// await channel.QueueDeclareAsync("webPageQueue", true, false, false);


                            // 3) Bind fronty na exchange
// Production:
await channel.QueueBindAsync("CZ_Production_defaultConsumerQueue", exchangeName, "CZ.production.default");
await channel.QueueBindAsync("CZ_Production_AggregationsQueue", exchangeName, "CZ.production.aggregations");
await channel.QueueBindAsync("SK_Production_defaultConsumerQueue", exchangeName, "SK.production.default");
// RC:
await channel.QueueBindAsync("CZ_RC_defaultConsumerQueue", exchangeName, "CZ.RC.default");
await channel.QueueBindAsync("SK_RC_defaultConsumerQueue", exchangeName, "SK.RC.default");
// webPage
await channel.QueueBindAsync("webPageQueue", exchangeName, "#.webPage"); 

// Sending part:
var dateTime = GenerateRandomDateTime();

// // CZ
await SendMessage("CZ", "production", "aggregations", dateTime);
await SendMessage("CZ", "production", "default", dateTime);
await SendMessage("CZ", "RC", "default", dateTime);

// SK
await SendMessage("SK", "production", "default", dateTime);
await SendMessage("SK", "RC", "default", dateTime);

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