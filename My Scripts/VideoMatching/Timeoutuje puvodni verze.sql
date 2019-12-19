--Timeoutuje videoProcessDao.GetOriginVideoProcessByCreativeItemId(norm.CreativeItemId)

--Hobbys to prepsal z 
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

     DateAdd(Millisecond, Convert(Float, [vp].[Shift]), [vp].[VideoDateTime]) <= [mm].[AdvertisedFrom] AND

     [mm].[AdvertisedFrom] < DateAdd(Hour, 1, DateAdd(Millisecond, Convert(Float, [vp].[Shift]), [vp].[VideoDateTime])) 

ORDER BY

       [mm].[Id]

',N'@creativeItemId int',@creativeItemId=10572388