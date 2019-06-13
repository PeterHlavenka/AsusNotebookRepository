/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[MediumId]
      ,[DateTime]
      ,[MediumDayStatusId]
      ,[Created]
      ,[CreatedBy]
      ,[Modified]
      ,[ModifiedBy]
      ,[Version]
  FROM [MediaData3BGRC].[SimLog].[MediumDay] WHERE MediumId = 25 AND SimLog.MediumDay.DateTime = '2019-05-09 00:00:00.000'

  UPDATE [MediaData3BGRC].[SimLog].[MediumDay] SET [SimLog].[MediumDay].MediumDayStatusId = 2  WHERE MediumId = 25 AND SimLog.MediumDay.DateTime = '2019-05-09 00:00:00.000'