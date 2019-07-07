SELECT
	[self].[OwnerId],
	[self].[MediumId],
	[self].[ActiveFrom],
	[self].[ActiveTo],
	count(self.OwnerId) 
FROM
	[Media].[SelfPromotionDefinition] [self]
		INNER JOIN [Media].[Owner] [t1] ON [self].[OwnerId] = [t1].[Id]
		INNER JOIN [Media].[Medium] [t2] ON [self].[MediumId] = [t2].[Id]
		INNER JOIN [Security].[User] [t3] ON [self].[CreatedBy] = [t3].[Id]
		LEFT JOIN [Security].[User] [t4] ON [self].[ModifiedBy] = [t4].[Id]
		INNER JOIN [Media].[MediumVersion] [version]
			LEFT JOIN [Media].[Publisher] [t5] ON [version].[PublisherId] = [t5].[Id]
		ON [self].[MediumId] = [version].[MediumId]
	
	GROUP BY self.OwnerId, self.MediumId, self.ActiveFrom, self.ActiveTo

	