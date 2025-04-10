using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SQLitePCL;


namespace Adwind.Rabbit.ImportViewer;

public class RabbitMqService
{
    private const string DataQueueName = "webPageQueue";
    private readonly DatabaseLogger<MessageRepository> m_log;
    private readonly MessageRepository m_messageRepository;

    public RabbitMqService()
    {
        Batteries.Init();
        //m_log = new DatabaseLogger<MessageRepository>(MessageRepository.GetDatabasePath());
        // m_messageRepository = new MessageRepository(m_log);
        // RawMessages = m_messageRepository.Load();
        RabbitMessages = GetLatestMessages();
        Initialize(); //.FireAndForgetSafeAsync(m_log.LogError, false);
    }

    public List<RabbitMessage> RawMessages { get; } = [];
    public List<RabbitMessage> RabbitMessages { get; private set; }
    public event Action MessagesChanged;

    private async Task Initialize()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
            // Uri = new Uri("amqp://phlavenka:LLykoat3J9HbDBUAjVW3@localhost:55350/adw-test")
        };
        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        // await channel.ExchangeDeclareAsync("importExchange", ExchangeType.Topic, true, false);

        // await channel.QueueDeclareAsync(queueName, true, false, false);
        // await channel.QueueBindAsync(queueName, "importExchange", "#.webPage");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += (_, ea) =>
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

            //await m_messageRepository.Save(rabbitMessage);
            RabbitMessages = GetLatestMessages();
            MessagesChanged?.Invoke();
            return Task.CompletedTask;
        };

        await channel.BasicConsumeAsync(DataQueueName, true, consumer);
    }

    /// Provides filtered messages for the view, where only the latest message for each combination of country, environment, and data type is shown.
    private List<RabbitMessage> GetLatestMessages()
    {
        var distinctMessages = RawMessages
            .SelectMany(m => m.DataTypes.Select(dataType => new RabbitMessage
            {
                Country = m.Country,
                Environment = m.Environment,
                DataTypes = [dataType],
                ImportDate = m.ImportDate
            }))
            .GroupBy(m => new { m.Country, m.Environment, m.DataTypesString })
            .Select(g => g.OrderByDescending(m => m.ImportDate).First())
            .ToList();

        return distinctMessages;
    }
}