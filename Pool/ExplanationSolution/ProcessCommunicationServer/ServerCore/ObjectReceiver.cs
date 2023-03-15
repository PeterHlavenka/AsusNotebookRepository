using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using Microsoft.Extensions.Hosting;
using Pricing.Core;
using Pricing.Core.Serialization;

namespace ServerCore;

public class ObjectReceiver : BackgroundService
{
    private readonly TextBox m_serverTextBox;
    private readonly DataContractSerializer m_serializer = new(typeof(PriceList), XmlPriceListItemSerializer.KnownTypes.Union(new[]
    {
        typeof(PriceListItemWrapper)
    }));

    public ObjectReceiver(TextBox serverTextBox)
    {
        m_serverTextBox = serverTextBox;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var pipeClient = new NamedPipeClientStream(".", "ObjectPipe", PipeDirection.In);
        while (true)
        {
            // Connect to the pipe or wait until the pipe is available.
            if (!pipeClient.IsConnected)
                await pipeClient.ConnectAsync(new CancellationToken());

            try
            {
                var buffer = new byte[30000];
                var read = await pipeClient.ReadAsync(buffer, 0, buffer.Length); // nesmi tu byt await !!

                var jsonString2 = Encoding.UTF8.GetString(buffer).TrimEnd('\0');

                var obj = GetDeserializedPriceList(jsonString2);
                // var obj = JsonSerializer.Deserialize<PriceList>(jsonString2);

                m_serverTextBox.Text = obj?.Name + " "+ new Random().Next();
            }
            catch (Exception exception) // kdyz se pipa zavre a JsonSerializer je v pulce procesu
            {
                Console.WriteLine(exception);
            }
        }
    }
    
    private PriceList? GetDeserializedPriceList(string serializedPriceList)
    {
        using StringReader stringReader = new StringReader(serializedPriceList);
        using XmlReader xmlReader = XmlReader.Create(stringReader);

        if (m_serializer.ReadObject(xmlReader) is PriceList priceList)
        {
            return priceList;
        }
        throw new ArgumentException();
    }
}