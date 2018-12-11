--m_tvImportItemDao.GetTvLogs

exec sp_executesql N'SELECT
	[tvImportItem].[ImportId],
	[tvImportItem].[ModificationStatusId],
	[tvImportItem].[OriginalImportItemId],
	[tvImportItem].[MediaMessageId],
	[tvImportItem].[AdvertisementTypeId],
	[tvImportItem].[AdvertisedFrom],
	[tvImportItem].[AdvertisedTo],
	[tvImportItem].[MediumId],
	[tvImportItem].[Footage],
	[tvImportItem].[AdvertisementName],
	[tvImportItem].[AdvertiserName],
	[tvImportItem].[AgencyName],
	[tvImportItem].[CompanyBrandName],
	[tvImportItem].[ProductCode],
	[tvImportItem].[ProductName],
	[tvImportItem].[BlockCode],
	[tvImportItem].[BlockEnd],
	[tvImportItem].[BlockPosition],
	[tvImportItem].[BlockRating],
	[tvImportItem].[BlockStart],
	[tvImportItem].[BlockUnits],
	[tvImportItem].[PoolingIdentifier],
	[tvImportItem].[ProgrammeAfter],
	[tvImportItem].[ProgrammeBefore],
	[tvImportItem].[ProgrammeTypeAfter],
	[tvImportItem].[ProgrammeTypeBefore],
	[tvImportItem].[ProgrammeTypeAfterCode],
	[tvImportItem].[ProgrammeTypeBeforeCode],
	[tvImportItem].[TapeCode],
	[tvImportItem].[TapeLength],
	[tvImportItem].[TapeName],
	[tvImportItem].[TapeAgencyName],
	[tvImportItem].[Rating],
	[tvImportItem].[Created],
	[tvImportItem].[CreatedBy],
	[tvImportItem].[Modified],
	[tvImportItem].[ModifiedBy],
	[tvImportItem].[BlockIdent],
	[tvImportItem].[Note],
	[tvImportItem].[Id],
	[t1].[Name],
	[t1].[OriginalId],
	[t1].[FootageThreshold],
	[t1].[Id] as [Id1]
FROM
	[Import].[TvImportItem] [tvImportItem]
		INNER JOIN [Media].[AdvertisementType] [t1] ON [tvImportItem].[AdvertisementTypeId] = [t1].[Id]
		INNER JOIN [Media].[Medium] [medium] ON [tvImportItem].[MediumId] = [medium].[OriginalId]
WHERE
	[tvImportItem].[AdvertisedFrom] > @boundary AND
	Convert(Int, [tvImportItem].[AdvertisementTypeId]) <> 4 AND
	[tvImportItem].[AdvertisedTo] > @p1 AND
	[tvImportItem].[AdvertisedFrom] < @dateTimeTo AND
	[medium].[Id] IN (491)
ORDER BY
	[tvImportItem].[AdvertisedFrom]
',N'@boundary datetime,@p1 datetime,@dateTimeTo datetime',@boundary='2018-07-20 07:59:55.600',@p1='2018-07-20 08:59:55.600',@dateTimeTo='2018-07-20 10:15:25.600'