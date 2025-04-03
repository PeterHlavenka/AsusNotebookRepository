using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Threading;

namespace DotVVM_table;

public class RabbitMqService
{
    private const string Separator = "_";
    private static readonly ILogger<RabbitMqService> m_log = new Logger<RabbitMqService>(new LoggerFactory());

    public RabbitMqService()
    {
        Initialize().FireAndForgetSafeAsync(m_log.LogError, false);
    }

    public List<string> RawMessages { get; set; } = new();
    public List<RabbitMessage> RabbitMessages { get; set; } = new();

    private async Task Initialize()
    {
        var factory = new ConnectionFactory
        {
            // HostName = "localhost:31361/adw-test" ,
            Uri = new Uri("amqp://phlavenka:LLykoat3J9HbDBUAjVW3@localhost:55350/adw-test")
        };
        var connection = await factory.CreateConnectionAsync(); // todo dispose / close connection
        var channel = await connection.CreateChannelAsync();

        // 1) deklarujeme exchange
        // await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);
        //
        // // 2) deklarace fronty
        const string queueName = "webPageQueue";
        // await channel.QueueDeclareAsync(queueName, true, false, false);
        //
        // // 3) musime frontu nabindovat na exchange
        // await channel.QueueBindAsync(queueName, "importExchange", "#.webPage");


        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            RawMessages.Add(message);

            // todo co by se melo zobrazovat - datum z posledni prijate message, nebo nejnovejsi message?  (co kdyz se bude preimportovat starsi den ?)
            var parts = message.Split(Separator);
            var same = RabbitMessages.SingleOrDefault(d => d.Country == parts[1] && d.Environment == parts[2] && d.ServiceName == parts[3] && d.DataType == parts[4]);
            if (same != null)
                RabbitMessages.Remove(same);
            RabbitMessages.Add(new RabbitMessage
            {
                ImportDate = parts[0],
                Country = parts[1],
                Environment = parts[2],
                ServiceName = parts[3],
                DataType = parts[4]
            });

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }
}