using System;
using System.Collections.Generic;
using System.IO;
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
    private readonly DatabaseLogger<MessageRepository> m_log;
    private readonly MessageRepository m_messageRepository;

    public List<RabbitMessage> RawMessages { get; } = new();
    public List<RabbitMessage> RabbitMessages { get; } = new();

    public RabbitMqService()
    {
        SQLitePCL.Batteries.Init(); 
        m_log = new DatabaseLogger<MessageRepository>(MessageRepository.GetDatabasePath());
        m_messageRepository = new MessageRepository(m_log);
        m_messageRepository.Load();
        Initialize().FireAndForgetSafeAsync(m_log.LogError, false);
    }

    private async Task Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);
        const string queueName = "webPageQueue";
        await channel.QueueDeclareAsync(queueName, true, false, false);
        await channel.QueueBindAsync(queueName, "importExchange", "#.webPage");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            var rabbitMessage = JsonSerializer.Deserialize<RabbitMessage>(message);
            if (rabbitMessage == null)
            {
                m_log.LogError("Failed to deserialize RabbitMessage: {Message}", message);
                return;
            }

            RawMessages.Add(rabbitMessage);
            var same = RabbitMessages.SingleOrDefault(d =>
                d.Country == rabbitMessage.Country &&
                d.Environment == rabbitMessage.Environment &&
                d.DataTypes == rabbitMessage.DataTypes);

            if (same != null)
            {
                RabbitMessages.Remove(same);
                rabbitMessage.ImportDate = DateTime.Parse(rabbitMessage.ImportDate) > DateTime.Parse(same.ImportDate)
                    ? rabbitMessage.ImportDate
                    : same.ImportDate;
            }

            RabbitMessages.Add(rabbitMessage);
            await m_messageRepository.Save(rabbitMessage);
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }
}
