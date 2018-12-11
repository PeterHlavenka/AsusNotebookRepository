/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]      
      ,[AdvertisedFrom]
      ,[AdvertisedTo]      
      ,[MediumId]      
  FROM [MediaData3Auto].[Media].[MediaMessage] where MediumId = 491 and AdvertisedFrom >= '2012-5-30 6:0:0' AND AdvertisedTo < '2012-5-31 6:0:0'
  -- plati pro zadanou message s Id = 9566145 