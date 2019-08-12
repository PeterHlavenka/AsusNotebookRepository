-- pokud chces ranni hodiny, musis hledat datum - 1 den.
--Timhle se daji najit splity:           AND a.VideoLength < 4500
--A timto dostanes zacatky a konce splitu pro jeden medium den:              AND a.VideoDateTime > @dayFrom AND a.VideoDateTime <= @dayTo AND a.MediumId =@mediumId

DECLARE @mediumId int;
DECLARE @dayFrom datetime;
DECLARE @dayTo datetime;
DECLARE @mediumName varchar(100);
SET @dayFrom = '20190701 05:59:00'
SET @dayTo = '20190702 06:00:00'
SET @mediumName = 'Disney Channel'--  'Discovery Channel' -- 
SET @mediumId = 16  -- DisneyChannel
--26  -- 24 Kitchen

SELECT TOP (1000) a.Id, VideoDateTime, DATEADD(ss, a.VideoLength, a.VideoDateTime) AS EndTime, DATEADD(ms, Shift, VideoDateTime) AS RealVideoDateTime, 
			DATEADD(ms, Shift, DATEADD(ss, a.VideoLength, a.VideoDateTime)) AS RealEndTime, TvStorageOutputRequestId,
						a.ChannelId, TvStorageLocationId, a.MediumId, a.MediumName, a.Shift, a.VideoLength, CONVERT(VARCHAR, DATEADD(second, a.VideoLength, 0),108) AS ValidLenght, a.VideoProcessStatusId
					FROM 
						(SELECT vp.Id, VideoDateTime, TvStorageOutputRequestId,	vp.TvStorageChannelId AS ChannelId, vp.VideoLength/1000 as VideoLength,
							TvStorageLocationId, m.Id AS MediumId, mdv.Name AS MediumName, vp.Shift, vp.TvStorageChannelId, scv.MediumId AS SlicedMedium,
							vp.VideoProcessStatusId									
						FROM Creative.VideoProcess vp
						JOIN media.TvMedium tm ON tm.TvStorageChannelId = vp.TvStorageChannelId
						JOIN media.Medium m ON tm.Id = m.Id
						JOIN media.MediumVersion mdv ON m.Id = mdv.MediumId AND mdv.ActiveFrom <= vp.VideoDateTime AND mdv.ActiveTo > vp.VideoDateTime
						LEFT JOIN media.SlicedChannelVersion scv ON m.Id = scv.MediumId
						AND (vp.VideoDateTime <= scv.ValidFrom AND scv.ValidFrom < DATEADD(hh, 1, vp.VideoDateTime)
						OR scv.ValidFrom <= vp.VideoDateTime AND vp.VideoDateTime < scv.ValidTo)
						--WHERE vp.VideoProcessStatusId = 2
						) a

					WHERE 
					((SELECT COUNT(Id) FROM Media.TvMedium WHERE TvStorageChannelId = a.TvStorageChannelId) > 1 AND a.SlicedMedium IS NOT NULL) 
					 OR 
					((SELECT COUNT(Id) FROM Media.TvMedium WHERE TvStorageChannelId = a.TvStorageChannelId) = 1) 
					--AND a.VideoLength < 4500										
					AND  a.MediumName = @mediumName -- AND a.MediumId =@mediumId
					AND a.VideoDateTime > @dayFrom AND a.VideoDateTime < @dayTo
					--AND a.TvStorageOutputRequestId = 80213874
					--AND a.Shift > 1000 AND a.Shift < 2000
					ORDER BY a.VideoDateTime, a.MediumName DESC

	-- Pozor stream na TvStorage muze mit jiny shift. Casy na TvStorage jsou v UTC case (jediny spravny) a proto jsou o 3 hodiny zpet.
	SELECT *	FROM [TVStorage2BGAuto].[dbo].[OutputRequest]	WHERE Id in(88640221)	 
	--UPDATE 	[TVStorage2BGAuto].[dbo].[OutputRequest] SET [TVStorage2BGAuto].[dbo].[OutputRequest].Shift = -11320	WHERE Id in(89217340, 89217341)

	SELECT TOP (100) *	FROM [TVStorage2BGAuto].[dbo].[OutputRequest] where TVStorage2BGAuto.dbo.OutputRequest.Length <4500