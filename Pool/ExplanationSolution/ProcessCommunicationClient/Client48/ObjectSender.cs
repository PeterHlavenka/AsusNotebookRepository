using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Pricing.Core;
using Pricing.Core.Serialization;

namespace Client48;

public class ObjectSender : BackgroundService
{
    private NamedPipeServerStream m_pipeServer;
    private readonly DataContractSerializer m_serializer = new(typeof(PriceList), XmlPriceListItemSerializer.KnownTypes.Union(new[]
    {
        typeof(PriceListItemWrapper)
    }));
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("ObjectPipe", PipeDirection.Out);

        await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }

    public void SendObject()
    {
        // create a new instance of MyObject
        var str = SerializeWholePriceList(GetEmptyPriceList());
        var buffer = Encoding.UTF8.GetBytes(str);
        
        if (!m_pipeServer.IsConnected) return;

        m_pipeServer.Write(buffer, 0, buffer.Length);
        m_pipeServer.Flush();
    }

    private string SerializeWholePriceList(PriceList priceList)
    {
        using MemoryStream memoryStream = new MemoryStream();
        m_serializer.WriteObject(memoryStream, priceList);
        memoryStream.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        string serializedPriceList = Encoding.UTF8.GetString(memoryStream.ToArray());
        return serializedPriceList;
    }

    private static PriceList GetEmptyPriceList()
    {
        var pl = new PriceList(Guid.NewGuid(), true) {Name = "New empty pricelist"};
        var group = new PriceListItemGroup {Name = "New group"};
        var wrapper = new PriceListItemWrapper {Name = "New pricelist"};
        wrapper.PriceListItem = new PriceListItem(wrapper.Id);
        group.Items.Add(wrapper);
        pl.Items.Add(group);
        return pl;
    }
}