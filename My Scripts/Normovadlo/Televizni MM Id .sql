/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[MediaTypeId]
  FROM [MediaData3Auto].[Media].[MediaMessage] WHERE Media.MediaMessage.MediaTypeId = 2 AND Media.MediaMessage.AdvertisedFrom > '20181201'