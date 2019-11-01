 --SELECT * FROM Media.TvMediaMessage mm WHERE mm.Id = 17701371
 --SELECT * FROM Media.ProgrammeBlock pb WHERE pb.Id = 13590636
 --UPDATE Media.ProgrammeBlock  SET BroadcastingDescriptionIdBefore = 11208397 where Id = 13590636

 --SELECT * FROM SimLog.BroadcastingDescription bd WHERE bd.Id = 11208397

 --SELECT * FROM SimLog.Programme p WHERE p.Name like '%Крайбрежната одисея на Джино Д%'

 --UPDATE [Import].[TvImportItem] SET BroadcastingDescriptionIdBefore  = 11208397 WHERE Id = 5142296

 --SELECT * FROM SimLog.PrgType pt WHERE Id = 30

 --UPDATE [SimLog].[PrgType] SET Code = 'abc' WHERE id = 30

 SELECT * FROM Media.ProgrammeBlock pb WHERE Id IN (13590597, 
13593517,
13593828,
13592514)

UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdBefore = 11216273 WHERE Id = 13590597
UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdIn = 11209613 WHERE Id = 13590597
UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdAfter = 11215697 WHERE Id = 13590597

UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdIn = 11216273 WHERE Id = 13592514
UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdIn = 11215697 WHERE Id = 13593517
UPDATE Media.ProgrammeBlock SET Media.ProgrammeBlock.BroadcastingDescriptionIdIn = 11216099 WHERE Id = 13593828

SELECT * FROM Import.TvImportItem tii WHERE iD = 5090850

UPDATE Import.TvImportItem
SET [BroadcastingDescriptionIdBefore] = 11216273,
[BroadcastingDescriptionIdAfter] = 11215697,
[BroadcastingDescriptionIdIn] = 11216099
 WHERE Import.TvImportItem.Id = 5090850







