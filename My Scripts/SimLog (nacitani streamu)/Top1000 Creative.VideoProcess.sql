/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[VideoProcessStatusId]
      ,[VdtData]
      ,[Modified]
      ,[ModifiedBy]
      ,[VdtDataIsImported]
      ,[VdtDataExist]
      ,[PeaksDataExist]
      ,[PeaksData]
      ,[Shift]
      ,[VideoDateTime]
      ,[VideoLength]
      ,[TvStorageChannelId]
      ,[TvStorageOutputRequestId]
      ,[TvStorageLocationId]
      ,[MatchingTypeReady]
      ,[CreatedBy]
      ,[Created]
  FROM [MediaData3BGAuto].[Creative].[VideoProcess]