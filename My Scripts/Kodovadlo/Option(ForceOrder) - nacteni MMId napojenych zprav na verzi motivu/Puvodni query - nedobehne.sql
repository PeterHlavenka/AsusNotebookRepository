
-- jeden motiv (resp. motiveVersion), muze byt navazany na velke mnozstvi kreativ. Tyto kreativy muzou byt Normou. Pravdepodobne existuje vetsi mnozstvi MM ktere maji tuto normu (NormCreativeId)

-- MotiveVersionDao.SelectAssociatedMessagesReprezentativeId(int motiveVersionId)  se prelozi takto:
exec sp_executesql N'SELECT
	[message].[NormCreativeId], motiveVersion.Id
FROM
	[Media].[MediaMessage] [message]
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND [motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom]
GROUP BY
	[message].[NormCreativeId], motiveVersion.Id
',N'@motiveVersionId int',@motiveVersionId= 262133 --306149 -- 1513434  --1066283




-- v druhe fazi vezmeme vysledek horni query a vlozime ho jako parametr do nasledujici query spolu s parametrem metody (int motiveversionId)
-- dostaneme zpravy, ktere maji stejnou normu a motiv, jejich motiv je aktivni a vybereme nejmensi (nejstarsi MM.Id )
exec sp_executesql N'SELECT
	
	Min([message].[Id])
FROM
	[Media].[MediaMessage] [message]
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND
	[motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom] AND
	[message].[NormCreativeId] = @normCreativeId
	option(force order)
',N'@motiveVersionId int,@normCreativeId int',@motiveVersionId=262133,@normCreativeId=411684

exec sp_executesql N'SELECT
	
	Min([message].[Id])
FROM
	[Media].[MediaMessage] [message]
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND
	[motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom] AND
	[message].[NormCreativeId] = @normCreativeId
	option(force order)
',N'@motiveVersionId int,@normCreativeId int',@motiveVersionId=262133,@normCreativeId=413800

exec sp_executesql N'SELECT
	
	Min([message].[Id])
FROM
	[Media].[MediaMessage] [message]
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND
	[motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom] AND
	[message].[NormCreativeId] = @normCreativeId
	option(force order)
',N'@motiveVersionId int,@normCreativeId int',@motiveVersionId=262133,@normCreativeId=1092904


--------------------
-- nalezeni skupin podle motiveVersionId ktere maji vice kreativ
SELECT TOP 100 mv.Id MotiveVersionId,
count(c.Id) creativeIdCount
FROM 
Media.MotiveVersion mv 
JOIN Creative.Creative c ON c.MotiveId = mv.MotiveId
GROUP BY mv.Id
HAVING count(c.Id) > 1

-- nalezeni kreativ, ktere maji stejny motiv, zadany pres Id
SELECT TOP 1000 mv.Id motiveVersionId,
c.Id CreativeId
FROM 
Media.MotiveVersion mv 
JOIN Creative.Creative c ON c.MotiveId = mv.MotiveId
GROUP BY mv.Id, c.Id
HAVING mv.Id = 306149

-- vyhledani vsech MM ktere maji kreativy se stejnym motivem (id z query vyse)
SELECT * FROM Media.MediaMessage mm WHERE mm.NormCreativeId IN 
(
	SELECT TOP 1000 c.Id CreativeId
	FROM Media.MotiveVersion mv 
	JOIN Creative.Creative c ON c.MotiveId = mv.MotiveId
	WHERE mv.Id = 306149
)








