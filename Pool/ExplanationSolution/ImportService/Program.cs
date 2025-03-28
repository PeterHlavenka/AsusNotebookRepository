using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

                                // 1) Deklarujeme exchange
var exchangeName = "importExchange";
await channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Topic, true, false);


                                // 2) Deklarace front:
// Production:
await channel.QueueDeclareAsync("CZ_Production_defaultConsumerQueue", true, false, false);
await channel.QueueDeclareAsync("CZ_Production_AggregationsQueue", true, false, false);
await channel.QueueDeclareAsync("SK_Production_defaultConsumerQueue", true, false, false);
// RC:
await channel.QueueDeclareAsync("CZ_RC_defaultConsumerQueue", true, false, false);
await channel.QueueDeclareAsync("SK_RC_defaultConsumerQueue", true, false, false);


                            // 3) Bind fronty na exchange
// Production:
await channel.QueueBindAsync("CZ_Production_defaultConsumerQueue", exchangeName, "CZ.production");
await channel.QueueBindAsync("CZ_Production_AggregationsQueue", exchangeName, "CZ.production");
await channel.QueueBindAsync("SK_Production_defaultConsumerQueue", exchangeName, "SK.production");
// RC:
await channel.QueueBindAsync("CZ_RC_defaultConsumerQueue", exchangeName, "CZ.RC");
await channel.QueueBindAsync("SK_RC_defaultConsumerQueue", exchangeName, "SK.RC");


// Sending part:
var dateTime = GenerateRandomDateTime();

// CZ
await SendMessage("CZ", "production", dateTime);
await SendMessage("CZ", "production", dateTime);
await SendMessage("CZ", "RC", dateTime);

// SK
await SendMessage("SK", "production", dateTime);
await SendMessage("SK", "RC", dateTime);

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

async Task SendMessage(string environment, string country, DateTime importDateTime)
{
    // Send message to the exchange with routing key "import.{environment}.{country}"
    var routingKey = $"{environment}.{country}";
    var message = $"Imported date {importDateTime:yyyy-MM-dd HH:mm:ss} for {routingKey}";
    var body = System.Text.Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(exchangeName, routingKey, body);
    Console.WriteLine($@"Sending message: {message}");
}