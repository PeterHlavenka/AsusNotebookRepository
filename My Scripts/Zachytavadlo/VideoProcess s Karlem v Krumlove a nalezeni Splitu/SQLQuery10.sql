select top (1000)* from [Creative].[VideoProcess] where [VideoDateTime] > '20180928 13:00:00' and [VideoDateTime] < '20180928 13:00:01' and TvStorageChannelId = 41

select * from Media.TvMedium tvm
join Media.MediumVersion mv on tvm.Id = mv.MediumId where name = 'National Geographic Channel'


select * from [Creative].[VideoProcess] vp
join Media.TvMedium tvm on vp.TvStorageChannelId = tvm.TvStorageChannelId
join Media.MediumVersion mv on tvm.Id = mv.MediumId where 
 [VideoLength] < 4530000 and [VideoDateTime]  > '20181120'

 select top (1000)* from [Creative].[VideoProcess] vp

 select * from [dbo].[OutputRequest] where Id = 73864531