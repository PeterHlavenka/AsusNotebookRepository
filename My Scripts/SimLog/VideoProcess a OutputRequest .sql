 -- VideoProcess ma na sobe property TvStorageChannelId a TvStorageOutputRequestId 
 select top (1000)* from [MediaData3BGAuto].[Creative].[VideoProcess] vp
 -- OutputRequest zase zna StreamId a StorageServerId
 select * from [TVStorage2BGAuto].[dbo].[OutputRequest] where Id = 73864531