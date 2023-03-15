using System;
using CommonLibs.XSerialization;
using JetBrains.Annotations;

namespace ServerCore;

[XSerializable("Wind.MainWindow.Backstage.UserData.PriceLists.ExternalPricing.CommonObject")]
public class CommonObject : IXSerializable
{
    [ForSerialization]
    public CommonObject(string type, int id, string name, DateTime? activeFrom = null, DateTime? activeTo = null)
    {
        Type = type;
        Id = id;
        Name = name;
        ValidFrom = activeFrom;
        ValidTo = activeTo;
    }

    [ForSerialization]
    public CommonObject(XStreamReader reader, int version)
    {
        Type = reader.ReadString();
        Id = reader.ReadInt32();
        Name = reader.ReadString();
        ValidFrom = reader.ReadNullableDateTime();
        ValidTo = reader.ReadNullableDateTime();
    }

    [ForSerialization]
    public CommonObject()
    {
    }

    public string Type { get; private set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public void Store(XStreamWriter writer)
    {
        writer.Write(Type);
        writer.Write(Id);
        writer.Write(Name);
        writer.WriteNullable(ValidFrom);
        writer.WriteNullable(ValidTo);
    }

    public void Load(XStreamReader reader, int version)
    {
        Type = reader.ReadString();
        Id = reader.ReadInt32();
        Name = reader.ReadString();
        ValidFrom = reader.ReadNullableDateTime();
        ValidTo = reader.ReadNullableDateTime();
    }
}