--Takto se nacitaji CreativeItemy do Normovadla. Zalozka PictureMatching - duplicity.

--Takkto se prelozi nacteni duplicit:
  exec sp_executesql N'SELECT
	[unsureCreative].[Id],
	[unsureCreative].[ContentTypeId],
	[ctci].[CreativeId]
FROM
	[Creative].[CreativeItem] [unsureCreative]
		INNER JOIN [Creative].[CreativeToCreativeItem] [ctci] ON [unsureCreative].[Id] = [ctci].[CreativeItemId]
WHERE
	Convert(Int, [unsureCreative].[CreativeMatchingStatusId]) = @p1 AND
	[unsureCreative].[CreativeMatchingProcessId] = @p2
',N'@p1 int,@p2 int',@p1=5,@p2=7


--Najdu top 100 creativeItemu
Select top 100 * from creative.CreativeItem order by Id desc

--Updatnu jim property tak, aby se nacetly do normovadla
update [Creative].[CreativeItem]  Set CreativeMatchingStatusId = 5 , CreativeMatchingProcessId = 7 where Id = 11024540