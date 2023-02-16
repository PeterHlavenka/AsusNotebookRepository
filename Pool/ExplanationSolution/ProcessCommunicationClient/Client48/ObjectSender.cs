using System;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Client48;

public class ObjectSender : BackgroundService
{
    private NamedPipeServerStream m_pipeServer;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("ObjectPipe", PipeDirection.Out);

        await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }

    public void SendObject()
    {
        // create a new instance of MyObject
        var obj = new CommonObject { Id = new Random().Next() };

        // serialize the object into a JSON string
        var jsonString = JsonSerializer.Serialize(obj);
        var buffer = Encoding.UTF8.GetBytes(jsonString);
        
        if (!m_pipeServer.IsConnected) return;

        m_pipeServer.Write(buffer, 0, buffer.Length);
        m_pipeServer.Flush();
    }
}