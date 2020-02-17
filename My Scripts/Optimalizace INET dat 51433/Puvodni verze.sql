
--Ultimatni zrychleni:

 

--                    [Creative].[CreativeToCreativeItem] [c] WITH (FORCESEEK)

--             WHERE

--                    [mm].[NormCreativeId] = [c].[CreativeId]

--       ) as [c1]

--FROM

--       [Media].[InternetMediaMessage] [im] WITH (FORCESEEK)

--100ms
 
--On je blb, on si mysli ze bude tech messagi importovat moc, tak si radeji imm a ctci scanuje misto seeku ... ten trubera vubec nevi, ze to importujem po jednom :)
 
--         Hobbys

 



--From: Miroslav Špaèek <Miroslav.Spacek@admosphere.cz> 
--Sent: Wednesday, April 24, 2019 9:10 PM
--To: Michal Koníèek <Michal.Konicek@admosphere.cz>
--Subject: RE: Oprava PRG dat

 

--…nedá mi to a pošlu Ti to…sice to funguje a není tøeba to hrotit…ale kdyby se Ti tato query podaøila zrychlit by tøeba jen o pùl vteøiny, ušetøilo by to celkvì tøeba až ? hodiny èasu importu…protože takových queries jsou položeny bìhem importu tisíce, M.

 

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

             INNER JOIN [Media].[MediaMessage] [mm] ON [im].[Id] = [mm].[Id]

             INNER JOIN [Import].[InternetImportItem] [iii] ON [im].[InternetImportItemId] = [iii].[Id]

             INNER JOIN [Media].[CodingHintContext] [cd] ON [mm].[CodingHintContextId] = [cd].[Id]

WHERE

       [iii].[AdvertisedTo] >= @importFrom AND

       [iii].[AdvertisedFrom] < @importTo AND

       [mm].[AdvertisedTo] >= @p1 AND

       [mm].[AdvertisedFrom] < @importTo1 AND

       [iii].[ReportId] = @p2 AND

       ([iii].[Metatag] = @Metatag1 AND [iii].[Md5Sum] IS NULL AND [iii].[PositionId] IS NULL OR [iii].[Metatag] IS NULL AND [iii].[Md5Sum] IS NULL AND [iii].[PositionId] IS NULL)

',N'@importFrom datetime,@importTo datetime,@p1 datetime,@importTo1 datetime,@p2 smallint,@Metatag1 nvarchar(35)',@importFrom='2019-01-01 00:00:00',@importTo='2019-01-31 23:59:59',@p1='2019-01-01 00:00:00',@importTo1='2019-01-31 23:59:59',@p2=145,@Metatag1=N'477_1550065843309_20041.1.5.4_PRG_1'

