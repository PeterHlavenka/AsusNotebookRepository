/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) pa.[Id]
      ,[PublicationId]
      ,[NameId]
      , p.MediumId 
	  	  , m.Name	  
	  , p.[Date] AS DatumVydani      
      ,[AddInfo]
      ,[InsertTypeId]
      ,[PageCount]
      ,[PartStatusId]
	  , pn.Name
	  ,[OrderId]
	  
	  

  FROM [PrintStorageAuto].[dbo].[Part] pa 
  JOIN [dbo].[PartName]pn ON pa.NameId = pn.Id 
  JOIN dbo.Publication p ON pa.PublicationId = p.Id
  JOIN dbo.Medium m ON p.MediumId = m.Id
  WHERE m.Name = 'ARCHITECT+'  AND p.[Date] = '2019-06-28 00:00:00.000'