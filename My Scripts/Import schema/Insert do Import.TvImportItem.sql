SELECT TOP 100 *
FROM Import.TvImportItem tii WHERE Id = 71719211

ALTER TABLE Import.TvImportItem DROP COLUMN IsHidden
ALTER TABLE History.TvImportItem DROP COLUMN IsHidden

SELECT  top 1 *  from [dbo].[DbSchemaVersion] where [DbSchemaVersion] = 161
DELETE FROM [dbo].[DbSchemaVersion] where [DbSchemaVersion] = 161

SELECT max(Id) FROM import.TvImportItem tii
SELECT * FROM Import.TvImportItem tii WHERE tii.OriginalImportItemId = 416189 AND  tii.AdvertisementName = 'IsHiddenTest2'

SELECT TOP 100 *
FROM History.TvImportItem tii WHERE tii.Id = 71719212

SELECT HistoryId, tii.HistoryCode, tii.HistoryDate, tii.HistoryBy, tii.Id, tii.ImportId, tii.MediaMessageId, tii.IsHidden from History.TvImportItem tii WHERE tii.Id = 71719213
SELECT * FROM [MediaData3Auto].[History].[TvImportItem]WHERE Id = 71719213

UPDATE Import.TvImportItem
SET Import.TvImportItem.IsHidden = 0 WHERE Import.TvImportItem.Id = 71719213

DELETE FROM Import.TvImportItem WHERE Import.TvImportItem.Id = 71719213

INSERT INTO Import.TvImportItem
(
    --Id - this column value is auto-generated
    ImportId,
    ModificationStatusId,
    OriginalImportItemId,
    MediaMessageId,
    AdvertisementTypeId,
    AdvertisedFrom,
    AdvertisedTo,
    MediumId,
    Footage,
    AdvertisementName,
    AdvertiserName,
    AgencyName,
    CompanyBrandName,
    ProductCode,
    ProductName,
    BlockIdent,
    BlockCode,
    BlockEnd,
    BlockPosition,
    BlockRating,
    BlockStart,
    BlockUnits,
    PoolingIdentifier,
    ProgrammeAfter,
    ProgrammeBefore,
    ProgrammeTypeAfter,
    ProgrammeTypeBefore,
    ProgrammeTypeAfterCode,
    ProgrammeTypeBeforeCode,
    TapeCode,
    TapeLength,
    TapeName,
    TapeAgencyName,
    Rating,
    Created,
    CreatedBy,
    Modified,
    ModifiedBy,
    Note,
    IsHidden
)
VALUES
(
    -- Id - int
    229, -- ImportId - int
    2, -- ModificationStatusId - tinyint
    416189, -- OriginalImportItemId - int
    5863634, -- MediaMessageId - int
    1, -- AdvertisementTypeId - tinyint
    '2018-11-16 10:47:19', -- AdvertisedFrom - datetime
    '2018-11-16 10:47:19', -- AdvertisedTo - datetime
    '', -- MediumId - varchar
    0, -- Footage - decimal
    'IsHiddenTest3', -- AdvertisementName - varchar
    '', -- AdvertiserName - varchar
    '', -- AgencyName - varchar
    '', -- CompanyBrandName - varchar
    '', -- ProductCode - varchar
    '', -- ProductName - varchar
    '', -- BlockIdent - varchar
    '', -- BlockCode - varchar
    '2018-11-16 10:47:19', -- BlockEnd - datetime
    0, -- BlockPosition - int
    0, -- BlockRating - decimal
    '2018-11-16 10:47:19', -- BlockStart - datetime
    0, -- BlockUnits - int
    '', -- PoolingIdentifier - varchar
    '', -- ProgrammeAfter - varchar
    '', -- ProgrammeBefore - varchar
    '', -- ProgrammeTypeAfter - varchar
    '', -- ProgrammeTypeBefore - varchar
    '', -- ProgrammeTypeAfterCode - varchar
    '', -- ProgrammeTypeBeforeCode - varchar
    '', -- TapeCode - varchar
    0, -- TapeLength - decimal
    '', -- TapeName - varchar
    '', -- TapeAgencyName - varchar
    0, -- Rating - decimal
    '2018-11-16 10:47:19', -- Created - datetime
    160, -- CreatedBy - tinyint
    '2018-11-16 10:47:19', -- Modified - datetime
    160, -- ModifiedBy - tinyint
    '', -- Note - varchar
    1 -- IsHidden - bit
)


