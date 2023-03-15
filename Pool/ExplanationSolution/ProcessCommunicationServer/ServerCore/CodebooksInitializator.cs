using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServerCore;

public static class PricingCodebooksInitializator
{
    public static Task InitializeCodebooks(out CommonObjectsHolder commonObjectsHolder)
    {
        var mediaOwners = new List<CommonObject>() ;
        var media = new List<CommonObject>();
        var targetGroups = new List<CommonObject>();
        var mediaTypes = new List<CommonObject>();
        var advertisementTypes = new List<CommonObject>();
        var placements = new List<CommonObject>();
        var pressPeriodicities = new List<CommonObject>();

      


        commonObjectsHolder = new CommonObjectsHolder(
            mediaOwners,
            media,
            targetGroups,
            mediaTypes,
            advertisementTypes,
            placements,
            pressPeriodicities);

        return Task.CompletedTask;
    }
}