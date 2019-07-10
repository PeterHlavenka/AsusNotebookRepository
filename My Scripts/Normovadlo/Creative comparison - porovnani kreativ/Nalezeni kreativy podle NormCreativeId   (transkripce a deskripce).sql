/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) c.Id
      ,[ImportId]
      ,[ImportSourceId]
      ,[CreativeDescriptionId]
      ,[MotiveId]      
      ,[Description]
      ,[Transcription]
      ,[IsMutationResolved]

	  ,cd.OriginalFileName
	  ,cd.MediaTypeId
	  ,cd.Footage

  FROM [MediaData3Auto].[Creative].[Creative] c
  left JOIN creative.CreativeDescription cd ON c.CreativeDescriptionId = cd.Id
  WHERE c.Id = 15971801  -- NormCreativeId
  
  WHERE c.Description IS NOT NULL -- AND c.Transcription IS NOT NULL -- transkripce jen na BG