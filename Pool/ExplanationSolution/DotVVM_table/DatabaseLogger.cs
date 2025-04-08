using System;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace DotVVM_table;

public class DatabaseLogger<T> : ILogger<T>
{
    private readonly string m_dbPath;

    public DatabaseLogger(string dbPath)
    {
        m_dbPath = dbPath;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
        // log everything
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        var message = formatter(state, exception);
        var timestamp = DateTime.UtcNow.ToString("o");
        var level = logLevel.ToString();

        using var connection = new SqliteConnection($"Data Source={m_dbPath}");
        connection.Open();

        var insert = connection.CreateCommand();
        insert.CommandText =
            @"
                INSERT INTO Logs (Timestamp, Level, Message, Exception)
                VALUES ($timestamp, $level, $message, $exception);
            ";

        insert.Parameters.AddWithValue("$timestamp", timestamp);
        insert.Parameters.AddWithValue("$level", level);
        insert.Parameters.AddWithValue("$message", message);
        insert.Parameters.AddWithValue("$exception", exception?.ToString() ?? "");

        insert.ExecuteNonQuery();
    }
}