
--m_broadcastingDescriptionDao.SelectByParams

exec sp_executesql N'SELECT TOP (2147483647)
	[broadcastingDescr].[LevelId],
	[broadcastingDescr].[MediumId],
	[broadcastingDescr].[DateTimeFrom],
	[broadcastingDescr].[DateTimeTo],
	[broadcastingDescr].[ProgrammeId],
	[broadcastingDescr].[ParentId],
	[broadcastingDescr].[Created],
	[broadcastingDescr].[CreatedBy],
	[broadcastingDescr].[Modified],
	[broadcastingDescr].[ModifiedBy],
	[broadcastingDescr].[Id],
	[programme].[PrgTypeId],
	[programme].[AttributeId],
	[programme].[Created] as [Created1],
	[programme].[CreatedBy] as [CreatedBy1],
	[programme].[Modified] as [Modified1],
	[programme].[ModifiedBy] as [ModifiedBy1],
	[programme].[Name],
	[programme].[Id] as [Id1],
	[t3].[Id] as [Id2],
	[t3].[Local] as [Local1],
	[t4].[ExternalId],
	[t4].[Local] as [Local2],
	[t4].[En],
	[t4].[Id] as [Id3],
	[t5].[Login],
	[t5].[UserName],
	[t5].[UserTypeId],
	[t5].[Id] as [Id4],
	[t6].[Login] as [Login1],
	[t6].[UserName] as [UserName1],
	[t6].[UserTypeId] as [UserTypeId1],
	[t6].[Id] as [Id5],
	[tran].[ProgrammeId] as [TranslatedEntityId],
	[tran].[LocalizationLanguageId],
	[tran].[Name] as [Name1],
	[tran].[IsReviewed],
	[tran].[Created] as [TranslationCreated],
	[tran].[CreatedBy] as [TranslationCreatedBy],
	[tran].[Modified] as [TranslationModified],
	[tran].[ModifiedBy] as [TranslationModifiedBy],
	[t1].[Login] as [Login2],
	[t1].[UserName] as [UserName2],
	[t1].[UserTypeId] as [UserTypeId2],
	[t1].[Id] as [Id6],
	[t2].[Login] as [Login3],
	[t2].[UserName] as [UserName3],
	[t2].[UserTypeId] as [UserTypeId3],
	[t2].[Id] as [Id7],
	[mv].[Name] as [Name2],
	[mv].[PublisherId],
	[mv].[MediumId] as [MediumId1],
	[mv].[PressPeriodicityId],
	[mv].[IsMinorChannel],
	[mv].[TvlogShift],
	[mv].[FocusId],
	[mv].[ActiveFrom],
	[mv].[ActiveTo],
	[mv].[Created] as [Created2],
	[mv].[CreatedBy] as [CreatedBy2],
	[mv].[Modified] as [Modified2],
	[mv].[ModifiedBy] as [ModifiedBy2],
	[mv].[NoAggregationTargetGroupId],
	[mv].[PageOfEar],
	[mv].[Id] as [Id8]
FROM
	[SimLog].[BroadcastingDescription] [broadcastingDescr]
		INNER JOIN [Security].[User] [t1] ON [broadcastingDescr].[CreatedBy] = [t1].[Id]
		LEFT JOIN [Security].[User] [t2] ON [broadcastingDescr].[ModifiedBy] = [t2].[Id]
		INNER JOIN [SimLog].[Programme] [programme]
			INNER JOIN [SimLog].[Attribute] [t3] ON [programme].[AttributeId] = [t3].[Id]
			INNER JOIN [SimLog].[PrgType] [t4] ON [programme].[PrgTypeId] = [t4].[Id]
			INNER JOIN [Security].[User] [t5] ON [programme].[CreatedBy] = [t5].[Id]
			LEFT JOIN [Security].[User] [t6] ON [programme].[ModifiedBy] = [t6].[Id]
		ON [broadcastingDescr].[ProgrammeId] = [programme].[Id]
		INNER JOIN [Media].[MediumVersion] [mv] ON [broadcastingDescr].[MediumId] = [mv].[MediumId]
		LEFT JOIN [SimLog].[ProgrammeTranslation] [tran] ON [programme].[Id] = [tran].[ProgrammeId]
WHERE
	([tran].[LocalizationLanguageId] IS NULL AND [tran].[ProgrammeId] IS NULL OR [tran].[LocalizationLanguageId] = @localizationLanguageId1) AND
	[mv].[ActiveFrom] <= [broadcastingDescr].[DateTimeFrom] AND
	[mv].[ActiveTo] > [broadcastingDescr].[DateTimeFrom] AND
	[broadcastingDescr].[DateTimeFrom] >= @from1 AND
	[broadcastingDescr].[DateTimeFrom] < @to1 AND
	[broadcastingDescr].[MediumId] IN (491)
ORDER BY
	[broadcastingDescr].[DateTimeFrom] DESC
',N'@localizationLanguageId1 tinyint,@from1 datetime,@to1 datetime',@localizationLanguageId1=2,@from1='2018-07-20 08:59:55.600',@to1='2018-07-20 10:15:25.600'