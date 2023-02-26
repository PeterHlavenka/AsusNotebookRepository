using System.Collections.Generic;
using System.Linq;
using Pricing.Core.Configuration;
using Pricing.Entities;

namespace Client48;

internal static class PriceListCodebooksInitializeFactory
{
    internal static void InitializeCodebooks(CommonObjectsHolder holder)
    {
        SetAdsCodebooks(holder);
        SetTvmCodebooks(holder);
        SetCommonCodebooks();
    }

    private static void SetCommonCodebooks()
    {
        PricingConfiguration.Current.RatingKinds = new List<RatingKind>
        {
            new() { Id = 1, Name = "Rating spotu" }, //Translator.Translate("Rating spotu")},  todo translator
            new() { Id = 2, Name = "Rating bloku" } //Translator.Translate("Rating bloku")}
        };
    }

    private static void SetTvmCodebooks(CommonObjectsHolder holder)
    {
        // GroupId kanálů v adw je jiné Id, jak PublisherId v Ads, tzn. musím používat GroupId, to je uloženo v Adw cenících ! Stejný problém jako s IdSpotu...
        PricingConfiguration.Current.MediaOwners =
            holder.MediaOwners.Select(d => new MediaOwner { Id = (short)d.Id, Name = d.Name }).ToList();

        PricingConfiguration.Current.Media = holder.Media
            .Select(channel => new Medium
            {
                Id = channel.Id,
                Name = channel.Name,
                ActiveFrom = channel.ValidFrom ?? default,
                ActiveTo = channel.ValidTo ?? default
            }).ToList();

        PricingConfiguration.Current.TargetGroups = holder.TargetGroups
            .Select(tg => new TargetGroup { Id = (byte)tg.Id, Name = tg.Name }).ToList();
    }

    private static void SetAdsCodebooks(CommonObjectsHolder holder)
    {
        PricingConfiguration.Current.MediaTypes = holder.MediaTypes
            .Select(mt => new MediaType { Id = mt.Id, Name = mt.Name }).ToList();

        PricingConfiguration.Current.AdvertisementTypes = holder.AdvertisementTypes
            .Select(advType => new AdvertisementType { Id = (byte)advType.Id, Name = advType.Name }).ToList();

        PricingConfiguration.Current.Placements = holder.Placements
            .Select(pl => new Placement { Id = (byte)pl.Id, Name = pl.Name }).ToList();

        PricingConfiguration.Current.Periodicities = holder.PressPeriodicities
            .Select(p => new Periodicity { Id = (short)p.Id, Name = p.Name }).ToList();
    }
}