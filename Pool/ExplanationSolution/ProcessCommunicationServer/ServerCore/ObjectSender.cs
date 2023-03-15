using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommonLibs.XSerialization;
using Microsoft.Extensions.Hosting;
using Pricing.Core;
using Pricing.Core.Serialization;

namespace ServerCore;

public class ObjectSender : BackgroundService
{
    private readonly DataContractSerializer m_serializer = new(typeof(PriceList), XmlPriceListItemSerializer.KnownTypes.Union(new[]
    {
        typeof(PriceListItemWrapper)
    }));

    private NamedPipeServerStream? m_pipeServer;
    private NamedPipeServerStream m_holderServer;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        m_pipeServer = new NamedPipeServerStream("ImmediatellyPipe", PipeDirection.Out);
        m_holderServer = new NamedPipeServerStream("HolderPipe", PipeDirection.Out);

        // await m_pipeServer.WaitForConnectionAsync(stoppingToken);
        // await Task.Delay(TimeSpan.FromHours(10), stoppingToken);
    }
    
    public async Task SendObject(CommonObjectsHolder commonObjectsHolder)
    {
        var buffer = XSerializator.StoreToByteArray(commonObjectsHolder);
        await Send(buffer, 1);
    }
    
    private async Task Send(byte[] buffer, int serializator)
    {
        if (m_holderServer is { IsConnected: false })
            await m_holderServer.WaitForConnectionAsync(new CancellationToken());

       
        m_holderServer.Write(BitConverter.GetBytes(buffer.Length), 0, sizeof(int)); // Write metadata indicating the length of the serialized data
        m_holderServer.Write(BitConverter.GetBytes(serializator), 0, sizeof(int)); // Write metadata indicating that the first serializer was used
        m_holderServer.Write(buffer, 0, buffer.Length);
        m_holderServer.Flush();
        
        m_holderServer.Disconnect();  // !!! toto drzi pipu. Po odeslani se musi disconnectnout
    }

    public async Task SendObject()
    {
        // create a new instance of MyObject
        var str = SerializeWholePriceList(GetEmptyPriceList());
        var buffer = Encoding.UTF8.GetBytes(str);

        if (m_pipeServer is { IsConnected: false })
            await m_pipeServer.WaitForConnectionAsync(new CancellationToken());

        m_pipeServer?.Write(buffer, 0, buffer.Length);
        m_pipeServer?.Flush();
    }

    private string SerializeWholePriceList(PriceList priceList)
    {
        using var memoryStream = new MemoryStream();
        m_serializer.WriteObject(memoryStream, priceList);
        memoryStream.Flush();
        memoryStream.Seek(0, SeekOrigin.Begin);

        var serializedPriceList = Encoding.UTF8.GetString(memoryStream.ToArray());
        return serializedPriceList;
    }

    private static PriceList GetEmptyPriceList()
    {
        var pl = new PriceList(Guid.NewGuid(), true) { Name = "New empty pricelist" };
        var group = new PriceListItemGroup { Name = "New group" };
        var wrapper = new PriceListItemWrapper { Name = "New pricelist" };
        wrapper.PriceListItem = new PriceListItem(wrapper.Id);
        group.Items.Add(wrapper);
        pl.Items.Add(group);
        return pl;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (m_pipeServer is { IsConnected: true })
            m_pipeServer.Disconnect();

        // m_pipeServer?.Close();  // pipa se musi jen disconnectnout, ne zavrit !! 
        await base.StopAsync(cancellationToken);
    }


}