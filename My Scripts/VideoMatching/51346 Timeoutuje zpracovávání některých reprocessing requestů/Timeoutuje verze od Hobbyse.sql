exec sp_executesql N'SELECT TOP (1)
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
       [mm].[Id],
       [vp].[TvStorageOutputRequestId],
       [vp].[VideoProcessStatusId],
       [vp].[Modified] as [Modified1],
       [vp].[ModifiedBy] as [ModifiedBy1],
       [vp].[Created] as [Created1],
       [vp].[CreatedBy] as [CreatedBy1],
       [vp].[VdtDataIsImported],
       [vp].[VdtDataExist],
       [vp].[PeaksDataExist],
       [vp].[MatchingTypeReady],
       [vp].[VideoDateTime],
       [vp].[Shift],
       [vp].[VideoLength],
       [vp].[TvStorageChannelId],
       [vp].[TvStorageLocationId],
       [vp].[Id] as [Id1]
FROM
       [Media].[MediaMessage] [mm]
             INNER JOIN [Media].[Medium] [me] ON [mm].[MediumId] = [me].[Id]
             INNER JOIN [Media].[TvMedium] [tv] ON [me].[Id] = [tv].[Id]
             INNER JOIN [Creative].[CreativeToCreativeItem] [ctci] ON [mm].[NormCreativeId] = [ctci].[CreativeId]
             INNER JOIN [Creative].[VideoProcess] [vp] WITH (INDEX(IX_TvStorageChannelId_VideoDateTime_Shift)) ON [tv].[TVStorageChannelId] = [vp].[TvStorageChannelId]
WHERE
       [ctci].[CreativeItemId] = @creativeItemId AND
--     DateAdd(Millisecond, Convert(Float, [vp].[Shift]), [vp].[VideoDateTime]) <= [mm].[AdvertisedFrom] AND
--     [mm].[AdvertisedFrom] < DateAdd(Hour, 1, DateAdd(Millisecond, Convert(Float, [vp].[Shift]), [vp].[VideoDateTime])) AND
       convert(bigint, 1000) * (DATEDIFF(s, [vp].[VideoDateTime], [mm].[AdvertisedFrom])) < 3600000 + Convert(Float, [vp].[Shift])
ORDER BY
       [mm].[Id]
',N'@creativeItemId int',@creativeItemId=10572388


 -- VZOR:
 -- convert(bigint, 1000) * (DATEDIFF(s, [vp].[VideoDateTime], [mm].[AdvertisedFrom])) < 3600000 + Convert(Float, [vp].[Shift])

 -- PREKLAD:
 -- POCET MILISEKUND, KTERE UBEHLY OD POCATECNIHO CASU DO VYSKYTU ZPRAVY  < POCET MILISEKUND JEDNE CELE HODINY BEZ SHIFTU (-6000)





   --SELECT    convert(bigint, 1000) * (DATEDIFF(s, [vp].[VideoDateTime], [mm].[AdvertisedFrom]))
     SELECT    1000 * (DATEDIFF(s, '2018-06-04 10:00:00.000', '2018-06-04 10:10:25.520'))     -- -> VRATI 625 000, COZ JE POCET MILISEKUND, KTERE UBEHLY OD POCATECNIHO CASU DO VYSKYTU ZPRAVY.
  

   --SELECT  3600000 + Convert(Float, [vp].[Shift])
     SELECT 3600000 -6000  -- -> VRATI 3 594 000    (POCET MILISEKUND JEDNE CELE HODINY BEZ SHIFTU (-6000))

	 SELECT    convert(bigint, 1000)
	 SELECT  (DATEDIFF(s, '2018-06-04 10:00:00.000', '2018-06-04 10:10:25.520'))  -- VRATI ROZDIL V SEKUNDACH MEZI TEMITO DVEMA CASY -> 625 SEKUND