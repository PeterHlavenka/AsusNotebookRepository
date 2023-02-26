using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using CommonLibs.XSerialization;
using Microsoft.Extensions.Hosting;
using Pricing.Core;
using Pricing.Core.Serialization;

namespace Client48;

public class ObjectReceiver : BackgroundService
{
    private readonly TextBox m_clientTextBox;
    private readonly DataContractSerializer m_serializer = new(typeof(PriceList), XmlPriceListItemSerializer.KnownTypes.Union(new[]
    {
        typeof(PriceListItemWrapper)
    }));

    public ObjectReceiver(TextBox clientTextBox)
    {
        m_clientTextBox = clientTextBox;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var holderClient = new NamedPipeClientStream(".", "HolderPipe", PipeDirection.In);
        var pipeClient = new NamedPipeClientStream(".", "ImmediatellyPipe", PipeDirection.In);
        while (true)
        {
            // Connect to the pipe or wait until the pipe is available.
            if (!holderClient.IsConnected)
                await holderClient.ConnectAsync(new CancellationToken());

            try
            {
                var lengthBuffer = new byte[sizeof(int)];
                var result = await holderClient.ReadAsync(lengthBuffer, 0, sizeof(int));

                var length = BitConverter.ToInt32(lengthBuffer, 0);

                // Read metadata indicating which serializer was used
                var serializerBuffer = new byte[sizeof(int)];
               int neco = await holderClient.ReadAsync(serializerBuffer, 0, sizeof(int));
                var serializer = BitConverter.ToInt32(serializerBuffer, 0);

                // Read the serialized data from the pipe
                var serializedData = new byte[length];
               var dva = await  holderClient.ReadAsync(serializedData, 0, length);

                // Deserialize the object using the appropriate deserializer based on the metadata
                if (serializer == 1)
                {
                    var obj = XSerializator.LoadObject(serializedData);

                    if (obj is CommonObjectsHolder commonObjectsHolder)
                    {
                        PriceListCodebooksInitializeFactory.InitializeCodebooks(commonObjectsHolder);
                        m_clientTextBox.Text =  new Random().Next() + " " + commonObjectsHolder.Media;
                    }
                    
                }
                //
                // var buffer = new byte[30000];
                // var read = await pipeClient.ReadAsync(buffer, 0, buffer.Length); // nesmi tu byt await !!
                //
                // var str = Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                // var obj1 = GetDeserializedPriceList(str);
                //
                // m_clientTextBox.Text = obj1?.Name + " "+ new Random().Next();
                
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