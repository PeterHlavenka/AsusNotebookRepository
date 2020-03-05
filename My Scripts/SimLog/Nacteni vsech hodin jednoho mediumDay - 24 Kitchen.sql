DECLARE @mediumId int;
DECLARE @dayFrom datetime;
DECLARE @dayTo datetime;
SET @dayFrom = '20190524 06:00:00'
SET @dayTo = '20190525 06:00:00'
SET @mediumId = 26  -- 24 Kitchen

SELECT vp.Id, mv.Name as MediumName, vp.VideoDateTime, DATEADD(ms, vp.Shift, vp.VideoDateTime) AS RealVideoDateTime, vp.TvStorageOutputRequestId,
                    vp.TvStorageChannelId AS ChannelId, vp.TvStorageLocationId, m.Id AS MediumId, vp.Shift,                   
                    vps.Name AS VideoProcessStatusName, vps.Id AS VideoProcessStatusId, u.UserName AS ModifiedBy, vp.Modified, scv.Id as SlicedChannelVersionId, vp.VideoLength/1000 as VideoLength
					FROM creative.VideoProcess vp
					JOIN creative.VideoProcessStatus vps on vp.VideoProcessStatusId = vps.Id
					JOIN media.TvMedium tm ON vp.TvStorageChannelId = tm.TvStorageChannelId
					JOIN media.Medium m ON tm.Id = m.Id
					JOIN media.MediumVersion mv on m.Id = mv.MediumId AND mv.ActiveFrom <= @dayFrom AND mv.ActiveTo > @dayFrom
					LEFT JOIN Security.[User] u on vp.ModifiedBy = u.Id
					LEFT JOIN media.SlicedChannelVersion scv ON m.Id = scv.MediumId 
					AND (vp.VideoDateTime <= scv.ValidFrom AND scv.ValidFrom < DATEADD(hh, 1, vp.VideoDateTime)
					OR scv.ValidFrom <= vp.VideoDateTime AND vp.VideoDateTime < scv.ValidTo)
					WHERE m.Id = @mediumId AND vp.VideoDateTime >= @dayFrom AND vp.VideoDateTime < @dayTo
					ORDER BY vp.VideoDateTime