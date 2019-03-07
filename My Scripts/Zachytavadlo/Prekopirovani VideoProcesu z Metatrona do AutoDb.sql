-- Pokud potrebuju nejaky otevrit nejaky stream ktery jeste neni na AutoDb, (je konec tydne a od preklopeni ostrej ubehlo par dni), musim si insertout VideoProces z ostej Db.
-- Vezme VideoProcess z Metatrona a vlozi ho do MediaData3Auto. Zachytavadlo pouziva ostrou TvStorage2 takze stream existuje. Staci mi dostat do autoDb videoProcess.
-- Musi se pustit nad db do ktere chceme insertovat, ne nad masterem. Viz MojePoznamky - Zachytavadlo - Jak dostat VideoProcess z Metatrona na Stoupu

-- Zjistim na Metatronovi jake jsou skutecne VideoDateTiem casy ktere potrebuju. Tady napr Po 21. hodine
SELECT * from [METATRON].[MediaData3].[Creative].[VideoProcess] WHERE TvStorageChannelId = 25 AND VideoDateTime > '2019-02-25 21:00:00.000'


USE MediaData3Auto

insert into [Creative].[VideoProcess] 
( 
[VideoProcessStatusId], 
[VdtData], 
[VdtDataIsImported], 
[VdtDataExist],
[PeaksDataExist], 
[PeaksData],
[VideoDateTime], 
[Shift],
[VideoLength], 
[TvStorageChannelId],
[TvStorageOutputRequestId],
[TvStorageLocationId],
[MatchingTypeReady], 
[CreatedBy],
[Created] 
)
 select 
 [VideoProcessStatusId], 
 [VdtData],
 [VdtDataIsImported], 
 [VdtDataExist], 
 [PeaksDataExist], 
 [PeaksData],
 [VideoDateTime], 
 [Shift], 
 [VideoLength], 
 [TvStorageChannelId],
 [TvStorageOutputRequestId], 
 [TvStorageLocationId],
 [MatchingTypeReady], 
 [CreatedBy], 
 [Created]

from [METATRON].[MediaData3].[Creative].[VideoProcess] WHERE TvStorageChannelId = 25 AND VideoDateTime in ('2019-02-25 21:22:01.660', '2019-02-25 21:25:00.153')

-- Aby bylo mozne vratit VideoProcess do  Zachytavadla pomoci zalozky Editation, musim tomuto VideoProcessu zmenit status na Done.
-- Zjistim si Id insertovaneho VideoProcessu 
SELECT * FROM Creative.VideoProcess vp WHERE TvStorageChannelId = 25 AND VideoDateTime in( '2019-02-25 23:01:00.583', '2019-02-20 23:00:00.000')

-- Updatnu mu VideoProcessStatus na Done = 4
UPDATE Creative.VideoProcess
SET    
    Creative.VideoProcess.VideoProcessStatusId = 4
WHERE Creative.VideoProcess.Id = 72555580

-------------
-- Tohle sem uplne nepatri: nacita to Sources do comba vlevo nahore. 
SELECT a.Id, VideoDateTime, DATEADD(ms, Shift, VideoDateTime) AS RealVideoDateTime, TvStorageOutputRequestId,
						a.ChannelId, TvStorageLocationId, a.MediumId, a.MediumName, a.Shift, a.VideoLength, a.VideoProcessStatusId
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
						WHERE vp.VideoProcessStatusId = 2) a
					WHERE 
					((SELECT COUNT(Id) FROM Media.TvMedium WHERE TvStorageChannelId = a.TvStorageChannelId) > 1 AND a.SlicedMedium IS NOT NULL) 
					 OR 
					((SELECT COUNT(Id) FROM Media.TvMedium WHERE TvStorageChannelId = a.TvStorageChannelId) = 1) 
					ORDER BY a.VideoDateTime, a.MediumName