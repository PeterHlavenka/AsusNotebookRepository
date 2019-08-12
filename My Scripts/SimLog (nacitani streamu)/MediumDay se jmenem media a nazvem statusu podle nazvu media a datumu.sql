
DECLARE @From  datetime;
DECLARE @mediumName varchar(100);
SELECT @from = '2019-07-13 00:00:00.000'
SET @mediumName = 'Discovery Channel'

-- Statusy muzou byt InProgress, done, undone

SELECT TOP (1000) md.[Id] AS MediumDayId
      ,mv.[MediumId]
	  , mv.Name
      ,[DateTime]
      ,[MediumDayStatusId]
	  ,mds.Name

  FROM [SimLog].[MediumDay] md
  JOIN media.Medium m ON md.MediumId = m.Id
  JOIN media.MediumVersion mv ON mv.MediumId = m.Id
  JOIN SimLog.MediumDayStatus mds ON md.MediumDayStatusId = mds.Id

  WHERE md.DateTime = @from
  AND mv.Name = @mediumName


  --UPDATE md set md.MediumDayStatusId  = 2 
  --FROM [SimLog].[MediumDay] md
  --JOIN media.Medium m ON md.MediumId = m.Id
  --JOIN media.MediumVersion mv ON mv.MediumId = m.Id
  --WHERE md.DateTime = @From AND mv.Name = @mediumName