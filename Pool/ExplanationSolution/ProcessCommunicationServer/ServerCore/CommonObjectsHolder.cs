using System.Collections.Generic;
using CommonLibs.XSerialization;
using JetBrains.Annotations;

namespace ServerCore;

[XSerializable("Adwind.UI.ExternalPricing.CommonObjectsHolder")]
public class CommonObjectsHolder : IXSerializable
{
    [ForSerialization]
    public CommonObjectsHolder(List<CommonObject> mediaOwners, List<CommonObject> media, List<CommonObject> targetGroups, List<CommonObject> mediaTypes, List<CommonObject> advertisementTypes, List<CommonObject> placements, List<CommonObject> pressPeriodicities)
    {
        MediaOwners = mediaOwners;
        Media = media;
        TargetGroups = targetGroups;
        MediaTypes = mediaTypes;
        AdvertisementTypes = advertisementTypes;
        Placements = placements;
        PressPeriodicities = pressPeriodicities;
    }

    public CommonObjectsHolder(XStreamReader reader, int version)
    {
        MediaOwners = reader.ReadObject<List<CommonObject>>();
        Media = reader.ReadObject<List<CommonObject>>();
        TargetGroups = reader.ReadObject<List<CommonObject>>();
        MediaTypes = reader.ReadObject<List<CommonObject>>();
        AdvertisementTypes = reader.ReadObject<List<CommonObject>>();
        Placements = reader.ReadObject<List<CommonObject>>();
        PressPeriodicities = reader.ReadObject<List<CommonObject>>();
    }

    public List<CommonObject> MediaOwners { get; set; }

    public List<CommonObject> Media { get; private set; }

    public List<CommonObject> TargetGroups { get; private set; }

    public List<CommonObject> MediaTypes { get; private set; }

    public List<CommonObject> AdvertisementTypes { get; private set; }

    public List<CommonObject> Placements { get; private set; }

    public List<CommonObject> PressPeriodicities { get; private set; }

    public void Store(XStreamWriter writer)
    {
        writer.WriteObject(MediaOwners);
        writer.WriteObject(Media);
        writer.WriteObject(TargetGroups);
        writer.WriteObject(MediaTypes);
        writer.WriteObject(AdvertisementTypes);
        writer.WriteObject(Placements);
        writer.WriteObject(PressPeriodicities);
    }

    public void Load(XStreamReader reader, int version)
    {
        MediaOwners = reader.ReadObject<List<CommonObject>>();
        Media = reader.ReadObject<List<CommonObject>>();
        TargetGroups = reader.ReadObject<List<CommonObject>>();
        MediaTypes = reader.ReadObject<List<CommonObject>>();
        AdvertisementTypes = reader.ReadObject<List<CommonObject>>();
        Placements = reader.ReadObject<List<CommonObject>>();
        PressPeriodicities = reader.ReadObject<List<CommonObject>>();
    }
}