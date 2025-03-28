using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);


// Sending part:
// CZ
await SendMessage("CZ", "production");
await SendMessage("CZ", "production");
await SendMessage("CZ", "RC");

// SK
await SendMessage("SK", "production");
await SendMessage("SK", "RC");

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

async Task SendMessage(string environment, string country)
{
    // Send message to the exchange with routing key "import.{environment}.{country}"
    var routingKey = $"{environment}.{country}";
    var importDateTime = GenerateRandomDateTime();
    var message = $"Imported date {importDateTime:yyyy-MM-dd HH:mm:ss} for {routingKey}";
    var body = System.Text.Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync("importExchange", routingKey, body);
    Console.WriteLine($@"Sending message: {message}");
}