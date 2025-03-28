using RabbitMQ.Client;

// nejprve zkousim  ExchangeType.DIRECT !!!!

var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync("mytopicexchange", ExchangeType.Topic, true, false);

var userPaymentMessage = "CZ user paid for someting";
var userpaymentsBody = System.Text.Encoding.UTF8.GetBytes(userPaymentMessage);
await channel.BasicPublishAsync("mytopicexchange", "user.CZ.payments", userpaymentsBody);
Console.WriteLine($@"Send message: {userpaymentsBody}");

var bussinesOrderMessage = "CZ user bussines ordered goods";
var bussinesOrderBody = System.Text.Encoding.UTF8.GetBytes(bussinesOrderMessage);
await channel.BasicPublishAsync("mytopicexchange", "user.CZ.order", bussinesOrderBody);
Console.WriteLine($@"Send message: {bussinesOrderBody}");

Console.ReadLine();