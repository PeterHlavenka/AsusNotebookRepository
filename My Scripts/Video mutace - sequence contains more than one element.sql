
--Az se to stane priste (samozrejme upravit parametry):

SET TRAN ISOLATION LEVEL snapshot
BEGIN TRANSACTION 
CREATE TABLE #res ( MotiveId int, AdvertisementTypeId tinyint, Footage smallint, NormCreativeId int, MediaMessageId int, CreativeItemId int, PlacementId smallint, Transcription NVARCHAR(MAX), DESCRIPTION NVARCHAR(MAX))
INSERT INTO #res EXEC Media.proc_Repair_DuplicityNorm @Od='2019-06-03 06:00:00',@Do='2019-09-03 06:00:00',@ExcludedMotives=N'1001828',@NotResolvedOnly=1,@DebugLevel=DEFAULT

SELECT r.MotiveId, r.NormCreativeId, r.AdvertisementTypeId, COUNT(1)
FROM #res r
GROUP BY r.MotiveId, r.NormCreativeId, r.AdvertisementTypeId
HAVING COUNT(1) > 1

--SELECT *
--FROM #res r
--WHERE r.MotiveId = 1195942

ROLLBACK

SELECT tmm.Footage, *
FROM Media.MediaMessage mm
JOIN Media.TvMediaMessage tmm ON tmm.Id = mm.Id
JOIN Creative.CreativeToCreativeItem ctci ON ctci.CreativeId = mm.CreativeId
WHERE ctci.CreativeItemId = 11399632

BEGIN TRANSACTION 
UPDATE Media.MediaMessage SET Modified = GETDATE(), ModifiedBy = 106 WHERE id = 122042862
UPDATE Media.TvMediaMessage SET Footage = 40 WHERE id = 122042862
COMMIT
