 SELECT bd.Id bdId, p.Id prId, p.Name, ptran.Name programmeTran, pt.local prTypeLocal, pt.en prTypeEn FROM SimLog.BroadcastingDescription bd
 JOIN SimLog.Programme p ON bd.ProgrammeId = p.Id
 JOIN SimLog.PrgType pt ON p.PrgTypeId = pt.Id
 LEFT JOIN SimLog.ProgrammeTranslation ptran ON p.Id = ptran.ProgrammeId 
 WHERE bd.Id IN (302897,
302898,
302899)


 SELECT * FROM Import.TvImportItem tii WHERE tii.BroadcastingDescriptionIdBefore = 570492
 SELECT * FROM SimLog.BroadcastingDescription bd WHERE iD = 570492

 SELECT * FROM Media.ProgrammeBlock pb WHERE Id = 24957604

 SELECT TOP 100 *
FROM SimLog.BroadcastingDescription bd

UPDATE Media.ProgrammeBlock
SET
    Media.ProgrammeBlock.BroadcastingDescriptionIdIn = 302897, -- int
    Media.ProgrammeBlock.BroadcastingDescriptionIdBefore = 302898, -- int
    Media.ProgrammeBlock.BroadcastingDescriptionIdAfter = 302899 -- int
	where Id = 24957604

	

