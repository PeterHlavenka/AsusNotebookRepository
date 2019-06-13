/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) md.[Id] AS MediumDayId
      ,mv.[MediumId]
	  , mv.Name
      ,[DateTime]
      ,[MediumDayStatusId]
	  ,mds.Name

  FROM [MediaData3BGRC].[SimLog].[MediumDay] md
  JOIN media.Medium m ON md.MediumId = m.Id
  JOIN media.MediumVersion mv ON mv.MediumId = m.Id
  JOIN SimLog.MediumDayStatus mds ON md.MediumDayStatusId = mds.Id

  WHERE md.DateTime = '2019-05-09 00:00:00.000'
  AND mv.Name = '24 Kitchen'