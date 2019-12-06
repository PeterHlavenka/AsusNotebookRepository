
-- prvni cast query executnuta z linq kvuli toList(), vrati pro toto Id 560 radku.





exec sp_executesql N'


SELECT
	[message].[NormCreativeId]
INTO #test     -- vytvorim temp tabuli a do ni isertnu tento select (mm.NormCreativeIds):
FROM
	[Media].[MediaMessage] [message]
	INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
	INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND 
	[motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom]
GROUP BY
	[message].[NormCreativeId]





SELECT  Min(mm.Id)  FROM #test																		-- vezmu #temp tabuli a k ni prijoinuju dalsi tabule on NormCreativeId
INNER JOIN Media.MediaMessage mm ON #test.NormCreativeId = mm.NormCreativeId
INNER JOIN [Creative].[Creative] [creative] ON #test.[NormCreativeId] = [creative].[Id]
INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND
	[motiveVersion].[ActiveFrom] <= mm.[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > mm.[AdvertisedFrom]
GROUP BY mm.NormCreativeId 
order by mm.NormCreativeId
    option(force order)
',N'@motiveVersionId int',@motiveVersionId= 306149						-- vstupem je motiveversionId ktere pouziju na to, abych do temp tabule dostal NormCreativeIds.  Vystupem je List<int> mm.Ids, ktere si vezme accessor






	





