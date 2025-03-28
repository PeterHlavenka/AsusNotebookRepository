using RabbitMQ.Client;

// nejprve zkousim  ExchangeType.DIRECT !!!!

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("myroutingexchange", ExchangeType.Direct, true, false);

var analyticsMessage = "Message for analytics";
var analyticsBody = System.Text.Encoding.UTF8.GetBytes(analyticsMessage);
var paymentsMessage = "Message for payments";
var paymentsBody = System.Text.Encoding.UTF8.GetBytes(paymentsMessage);

await channel.BasicPublishAsync("myroutingexchange", "analyticsonly", analyticsBody);

await channel.BasicPublishAsync("myroutingexchange", "paymentsonly", paymentsBody);
await channel.BasicPublishAsync("myroutingexchange", "paymentsonly", paymentsBody);
Console.WriteLine($" [x] Sent.. ");

Console.ReadLine();