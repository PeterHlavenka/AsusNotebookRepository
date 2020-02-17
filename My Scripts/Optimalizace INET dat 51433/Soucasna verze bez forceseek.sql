exec sp_executesql N'SELECT
	[im].[Section] as [Section1],
	[im].[ContentTypes],
	[im].[Width],
	[im].[Height],
	[im].[Metatag],
	[im].[ReportId],
	[im].[Md5Sum],
	[im].[InternetImportItemId],
	[im].[PricingType],
	[im].[Performance],
	[im].[Impressions],
	[im].[CampaignId],
	[im].[Id],
	[mm].[ImportId],
	[mm].[MediaTypeId],
	[mm].[AdvertisedFrom],
	[mm].[AdvertisedTo],
	[mm].[CodingHintContextId],
	[mm].[CodingPlausibilityId],
	[mm].[Ready],
	[mm].[MediumId],
	[mm].[PriceValue],
	[mm].[PriceScopeId],
	[mm].[IsPriceUnknown],
	[mm].[PlacementId],
	[mm].[PremiereMessageId],
	[mm].[Note],
	[mm].[NormCreativeId],
	[mm].[Created],
	[mm].[CreatedBy],
	[mm].[Modified],
	[mm].[ModifiedBy],
	[mm].[OriginalId],
	[mm].[CreativeId],
	[mm].[IsSelfPromotion],
	[mm].[AdvertisementTypeId],
	[mm].[AdvertisementSourceId],
	[mm].[IsPriceManually],
	[mm].[Version],
	[mm].[Id] as [Id1],
	[cd].[AdvertiserCrn],
	[cd].[AdvertiserName],
	[cd].[AgencyCrn],
	[cd].[AgencyName],
	[cd].[CampaignName],
	[cd].[CompanyBrandName],
	[cd].[ProductBrandName],
	[cd].[ProductCategoryCode],
	[cd].[ProductCategoryName],
	[cd].[ProductSpecification],
	[cd].[MotiveName],
	[cd].[GenderName],
	[cd].[MarketName],
	[cd].[OwnershipName],
	[cd].[PlatformName],
	[cd].[TelcoName],
	[cd].[Id] as [Id2],
	[iii].[PositionId],
	[iii].[Metatag] as [Metatag1],
	[iii].[Md5Sum] as [Md5Sum1],
	(
		SELECT
			Count(*)
		FROM
			[Creative].[CreativeToCreativeItem] [c]
		WHERE
			[mm].[NormCreativeId] = [c].[CreativeId]
	) as [c1]
FROM
	[Media].[InternetMediaMessage] [im]
		INNER JOIN [Media].[MediaMessage] [mm]  ON [im].[Id] = [mm].[Id]
		INNER JOIN [Import].[InternetImportItem] [iii] ON [im].[InternetImportItemId] = [iii].[Id]
		INNER JOIN [Media].[CodingHintContext] [cd] ON [mm].[CodingHintContextId] = [cd].[Id]
WHERE
	[mm].[MediaTypeId] = @mediaType AND
	[iii].[AdvertisedTo] >= @importFrom AND
	[iii].[AdvertisedFrom] < @importTo AND
	[mm].[AdvertisedTo] >= @p1 AND
	[mm].[AdvertisedFrom] < @importTo1 AND
	[iii].[ReportId] = @p2
',N'@mediaType smallint,@importFrom datetime,@importTo datetime,@p1 datetime,@importTo1 datetime,@p2 smallint',@mediaType=32,@importFrom='2019-01-01 00:00:00',@importTo='2019-01-31 23:59:59',@p1='2019-01-01 00:00:00',@importTo1='2019-01-31 23:59:59',@p2=145

