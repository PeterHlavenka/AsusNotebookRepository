using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using RabbitCommon;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Threading;


namespace DotVVM_table;

public class RabbitMqService
{
    private static readonly ILogger<RabbitMqService> m_log = new Logger<RabbitMqService>(new LoggerFactory()); // todo
    private readonly string m_dbPath;

    public RabbitMqService()
    {
        m_dbPath = GetDatabasePath();
        InitializeDatabase();
        Initialize().FireAndForgetSafeAsync(m_log.LogError, false);
    }

    public List<RabbitMessage> RawMessages { get; } = new();
    public List<RabbitMessage> RabbitMessages { get; } = new();

    private static string GetDatabasePath()
    {
        var folder = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, "messages.db");
    }

    private void InitializeDatabase()
    {
        if (File.Exists(m_dbPath)) return;
        SQLitePCL.Batteries.Init();
        using var connection = new SqliteConnection($"Data Source={m_dbPath}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"
                CREATE TABLE RabbitMessages (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Country TEXT,
                    Environment TEXT,
                    DataTypes TEXT,
                    ImportDate TEXT
                );
            ";
        tableCmd.ExecuteNonQuery();
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
            await SaveMessageToDatabase(rabbitMessage);
        };

        await channel.BasicConsumeAsync(queueName, true, consumer);
    }

    private async Task SaveMessageToDatabase(RabbitMessage message)
    {
        try
        {
            await using var connection = new SqliteConnection($"Data Source={m_dbPath}");
            await connection.OpenAsync();

            var insertCmd = connection.CreateCommand();
            insertCmd.CommandText =
                @"
                INSERT INTO RabbitMessages (Country, Environment, DataTypes, ImportDate)
                VALUES ($country, $environment, $dataTypes, $importDate);
            ";

            insertCmd.Parameters.AddWithValue("$country", message.Country);
            insertCmd.Parameters.AddWithValue("$environment", message.Environment);
            insertCmd.Parameters.AddWithValue("$dataTypes", message.DataTypesString);
            insertCmd.Parameters.AddWithValue("$importDate", message.ImportDate);

            await insertCmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            m_log.LogError(ex, "Failed to save message to database");
        }
    }
}