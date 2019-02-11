/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[OriginalId]
      ,[Name]
      ,[Created]
      ,[CreatedBy]
      ,[Modified]
      ,[ModifiedBy]
      ,[Version]
      ,[CRN]
  FROM [MediaData3Auto].[Media].[Publisher]

  -- Admin - zalozka Repricing - nacteni Publischeru (Medialnich domu) do comboBoxu MedialHouse (Miroslav)
  -- Podivej se na mediumVersion ktera zna Publischera i Medium, vyber jen aktivni, koukni na medium ktere zna mediaTyp a vyber jen spravny mediaTyp. 
select distinct p.* from [Media].[Publisher] [p]
	inner join [Media].[MediumVersion] [mv] on [mv].[PublisherId] = [p].[Id] and mv.[ActiveTo] > getdate()
	inner join [Media].[Medium] [m] on [m].[Id] = [mv].[MediumId]
where m.[MediaTypeId] = 4 /* 1 - press, 4 - radio*/


SELECT TOP 100 *
FROM Media.MediaMessage mm


-- Vrati pocet MM pro Radio -  Cesky rozhlas, ktere jsou mezi zadanym datumem, a nejsou not priced (checkbox v UI). Vrati 1892 zaznamu. Misto * dat select Count(mm.Id) as count
exec sp_executesql N'SELECT mm.Id as MMId, mm.MediaTypeId, mv.Name as MediumName, p.Name as publisherName, p.Id publisherId, mm.AdvertisedFrom advFrom, mm.AdvertisedTo advTo
		FROM Media.MediaMessage mm
        JOIN Media.Medium m ON mm.MediumId = m.Id
        JOIN Media.MediumVersion mv ON m.Id = mv.MediumId
        JOIN Media.Publisher p ON mv.PublisherId = p.Id
		WHERE 
		mm.AdvertisedFrom >= @from 
		AND 
		mm.AdvertisedFrom < @to 
		AND 
		mm.MediaTypeId = @mediaTypeId 
		AND 
		(@isPriceNullOnly = 0 OR mm.PriceValue is null) 
		AND 
		p.Id = @publisherId'
		
		,N'@from datetime,@to datetime,@mediaTypeId smallint,@isPriceNullOnly bit,@publisherId smallint'
		,@from='2018-12-01 00:00:00',@to='2019-01-29 00:00:00',@mediaTypeId=4,@isPriceNullOnly=0,@publisherId=1434