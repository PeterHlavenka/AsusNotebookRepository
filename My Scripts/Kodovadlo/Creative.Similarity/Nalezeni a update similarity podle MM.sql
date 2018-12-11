/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [NormCreativeItemId]
      ,[ComparedCreativeItemId]
      ,[SimilarityInPercent]
      ,[Score]
      ,[ScoreUpperBound]
      ,[OverlayA]
      ,[OverlayB]
      ,[DiffPxlsRateA]
      ,[DiffPxlsRateB]
      ,[DuplicityConfidence]
      ,[Confidence]
      ,[CodingResultProcessId]
      ,[SimilarityKindId]
      ,[SimilarityResolvedStatusId]
      ,[MaskImg]
      ,[Modified]
      ,[ModifiedBy]
      ,[Created]
      ,[CreatedBy]
      ,[Order]
  FROM [MediaData3RC].[Creative].[CreativeSimilarity] WHERE Creative.CreativeSimilarity.ComparedCreativeItemId = 10922745

  --DELETE FROM  [Creative].[CreativeSimilarity]  WHERE Creative.CreativeSimilarity.ComparedCreativeItemId = 10922745
  UPDATE [MediaData3RC].[Creative].[CreativeSimilarity] SET [Creative].[CreativeSimilarity].MaskImg = NULL WHERE Creative.CreativeSimilarity.ComparedCreativeItemId = 10922745


  -- Mam MM , potrebuju najit CreativeId, dale pres vazebni tabuli CreativeItem. Ten pak vrazim do  horni query. 
  SELECT * FROM media.MediaMessage mm WHERE mm.id = 106329185
  SELECT * FROM creative.Creative c WHERE id = 15750882
  SELECT * FROM creative.CreativeToCreativeItem ctci WHERE ctci.CreativeId = 15750882
  SELECT * FROM creative.CreativeItem ci WHERE ci.Id = 10922745