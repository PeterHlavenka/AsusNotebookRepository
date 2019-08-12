/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) m.[Id]
      ,[TvStorageChannelId]
	  , mv.Name
  FROM [MediaData3BGAuto].[Media].[TvMedium]
  JOIN media.Medium m ON [MediaData3BGAuto].[Media].[TvMedium].Id = m.Id
  JOIN media.MediumVersion mv ON m.Id = mv.MediumId