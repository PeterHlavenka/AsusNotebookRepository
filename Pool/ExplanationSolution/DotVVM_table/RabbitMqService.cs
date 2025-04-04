using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitCommon;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Threading;

namespace DotVVM_table;

public class RabbitMqService
{
    private static readonly ILogger<RabbitMqService> m_log = new Logger<RabbitMqService>(new LoggerFactory());

    public RabbitMqService()
    {
        Initialize().FireAndForgetSafeAsync(m_log.LogError, false);
    }

    public List<RabbitMessage> RawMessages { get; set; } = new();
    public List<RabbitMessage> RabbitMessages { get; set; } = new();

    private async Task Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
            //Uri = new Uri("amqp://phlavenka:LLykoat3J9HbDBUAjVW3@localhost:55350/adw-test")
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        // 1) deklarujeme exchange
        await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);

        // // 2) deklarace fronty
        const string queueName = "webPageQueue";
        await channel.QueueDeclareAsync(queueName, true, false, false);

        // // 3) musime frontu nabindovat na exchange
        await channel.QueueBindAsync(queueName, "importExchange", "#.webPage");


        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            var rabbitMessage = JsonSerializer.Deserialize<RabbitMessage>(message);
            if (rabbitMessage == null)
            {
                m_log.LogError("Failed to deserialize RabbitMessage: {Message}", message);
                return Task.CompletedTask;
            }
            RawMessages.Add(rabbitMessage);
            var same = RabbitMessages.SingleOrDefault(d => d.Country == rabbitMessage.Country && d.Environment == rabbitMessage.Environment && d.DataTypes == rabbitMessage.DataTypes);
            if (same != null)
            {
                RabbitMessages.Remove(same);
                rabbitMessage.ImportDate = DateTime.Parse(rabbitMessage.ImportDate) > DateTime.Parse(same.ImportDate) ? rabbitMessage.ImportDate : same.ImportDate;
            }

            RabbitMessages.Add(rabbitMessage);

            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }
}