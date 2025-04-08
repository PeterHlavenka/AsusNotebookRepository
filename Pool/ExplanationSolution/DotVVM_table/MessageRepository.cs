using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using RabbitCommon;
using SQLitePCL;

namespace DotVVM_table;

public class MessageRepository
{
    private readonly string m_dbPath;
    private readonly DatabaseLogger<MessageRepository> m_log;

    public MessageRepository(DatabaseLogger<MessageRepository> logger)
    {
        m_log = logger;
        m_dbPath = GetDatabasePath();
        InitializeDatabase();
    }

    public static string GetDatabasePath()
    {
        var folder = Path.Combine(AppContext.BaseDirectory, "Data");
        Directory.CreateDirectory(folder);
        return Path.Combine(folder, "messages.db");
    }

    private void InitializeDatabase()
    {
        if (File.Exists(m_dbPath)) return;

        Batteries.Init();
        using var connection = new SqliteConnection($"Data Source={m_dbPath}");
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            @"
            CREATE TABLE RabbitMessages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Timestamp TEXT,
                Country TEXT,
                Environment TEXT,
                DataTypes TEXT,
                ImportDate TEXT
            );
        ";
        tableCmd.ExecuteNonQuery();

        var createLogsTable = connection.CreateCommand();
        createLogsTable.CommandText =
            @"
        CREATE TABLE Logs (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Timestamp TEXT,
            Level TEXT,
            Message TEXT,
            Exception TEXT
        );
        ";
        createLogsTable.ExecuteNonQuery();
    }

    public async Task Save(RabbitMessage message)
    {
        try
        {
            await using var connection = new SqliteConnection($"Data Source={m_dbPath}");
            await connection.OpenAsync();

            var insertCmd = connection.CreateCommand();
            var timestamp = DateTime.UtcNow.ToString("o");
            insertCmd.CommandText =
                @"
                INSERT INTO RabbitMessages (Timestamp, TimeCountry, Environment, DataTypes, ImportDate)
                VALUES ($timestamp, $country, $environment, $dataTypes, $importDate);
            ";

            insertCmd.Parameters.AddWithValue("$timestamp", timestamp);
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

    public List<RabbitMessage> Load()
    {
        var messages = new List<RabbitMessage>();
        try
        {
            using var connection = new SqliteConnection($"Data Source={m_dbPath}");
            connection.Open();

            var selectCmd = connection.CreateCommand();
            selectCmd.CommandText = "SELECT Country, Environment, DataTypes, ImportDate FROM RabbitMessages";

            using var reader = selectCmd.ExecuteReader();
            while (reader.Read())
            {
                var message = new RabbitMessage
                {
                    Country = reader.GetString(0),
                    Environment = reader.GetString(1),
                    DataTypes = reader.GetString(2).Split(", ").ToArray(),
                    ImportDate = reader.GetString(3)
                };
                messages.Add(message);
            }

            m_log.LogInformation("Loaded {Count} messages from database", messages.Count);
        }
        catch (Exception ex)
        {
            m_log.LogError(ex, "Failed to load messages from database");
        }

        return messages;
    }
}