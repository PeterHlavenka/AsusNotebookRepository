/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Creative1Id]
      ,[Creative2Id]
      ,[InsertedDateTime]
      ,[Corrected]
      ,[IsDeletedFromCache]
      ,[CreatedBy]
      ,[Version]
      ,[ThresholdId]
      ,[Modified]
      ,[ModifiedBy]
  FROM [MediaData3Auto].[Creative].[NormDuplicity] ORDER BY Creative.NormDuplicity.InsertedDateTime DESC

  -- Updatne Corrected u deseti zaznamu
  UPDATE [Creative].[NormDuplicity] SET [Creative].[NormDuplicity].Corrected = 0 WHERE [Creative].[NormDuplicity].Id in 
  (SELECT TOP (10) Id FROM Creative.NormDuplicity nd ORDER BY nd.InsertedDateTime DESC)