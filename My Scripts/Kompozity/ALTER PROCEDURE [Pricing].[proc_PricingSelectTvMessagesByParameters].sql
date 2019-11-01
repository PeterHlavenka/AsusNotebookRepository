USE [MediaData3BGRC]
GO

/****** Object:  StoredProcedure [Pricing].[proc_PricingSelectTvMessagesByParameters]    Script Date: 10.10.2019 12:02:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
Autor: Petr Dobe�
Description: Proced�ra pro na�ten� zpr�v do cen�kovadla
History:
  - 29.05.2019 - Petr Dobe� - TFS #51353 Cen�kovadlo - nesmazalo po sob� do�asnou tabulku
  - 26.06.2019 - Petr Dobe� - TFS #51377 Cen�n� �iv�ch po�ad�
*/

ALTER PROCEDURE [Pricing].[proc_PricingSelectTvMessagesByParameters]
(
	@From DATETIME,
	@To DATETIME,
	@MediumId SMALLINT = NULL,
	@AdvertisementTypeId INT = NULL
)
AS
BEGIN
	-- MESSAGE
	CREATE TABLE #Pricing_MessageTempTable (Id INT, AdvertisedFrom DATETIME, AdvertisedTo DATETIME, CodingPlausibilityId TINYINT,
		MediumId SMALLINT, PlacementId SMALLINT, PriceValue MONEY, MediaTypeId SMALLINT, AdvertisementTypeId TINYINT, NormCreativeId INT, MotiveId INT,
		Placement VARCHAR(255), MediumOriginalId NVARCHAR(255), Medium NVARCHAR(255), PublisherId SMALLINT)

	INSERT INTO #Pricing_MessageTempTable (Id, AdvertisedFrom, AdvertisedTo, CodingPlausibilityId, MediumId, PlacementId, PriceValue, MediaTypeId, AdvertisementTypeId, NormCreativeId, MotiveId, Placement, MediumOriginalId, Medium, PublisherId)

	SELECT mm.Id, mm.AdvertisedFrom, mm.AdvertisedTo,mm.CodingPlausibilityId, mm.MediumId,
		   mm.PlacementId,mm.PriceValue,mm.MediaTypeId,mm.AdvertisementTypeId, mm.NormCreativeId, 
		c.MotiveId, p.Name, m.OriginalId, mdv.Name, mdv.PublisherId
	FROM Media.MediaMessage mm 
	JOIN Media.Medium m ON mm.MediumId = m.Id
	JOIN Media.MediumVersion mdv ON mdv.MediumId = m.Id AND mdv.ActiveFrom <= mm.AdvertisedFrom AND mdv.ActiveTo > mm.AdvertisedFrom
	JOIN Creative.Creative c on c.Id=mm.NormCreativeId
	LEFT JOIN Media.Placement p ON p.Id = mm.PlacementId	
	WHERE mm.AdvertisedFrom >= @From
		AND mm.AdvertisedFrom < @To
		AND mm.MediaTypeId = 2
		AND mm.CodingPlausibilityId = 8
		AND (@MediumId IS NULL OR mm.MediumId = @mediumId)
		AND AdvertisementTypeId <> 6
		AND (AdvertisementTypeId=@advertisementTypeId OR @advertisementTypeId IS NULL)
	
	-- TV MESSAGE
	CREATE TABLE #Pricing_TvMessageTempTable (Id INT, BlockPosition TINYINT, Footage SMALLINT, Rating DECIMAL(10,4), SponsoredProgrammeId INT, IsSponsoredProgrammeUserDefined BIT,
		TapeAgencyName VARCHAR(200), TapeCode VARCHAR(50), TapeLength SMALLINT, TapeName VARCHAR(100), SponsoredProgrammeName VARCHAR(200),
		BlockId INT, BlockEnd DATETIME, BlockIdent VARCHAR(18), BlockRating DECIMAL(10,4), BlockStart DATETIME, BlockUnits TINYINT, BlockCode VARCHAR(20),		
		ProgrammeAfter NVARCHAR(200), ProgrammeTypeIdAfter SMALLINT, ProgrammeBefore NVARCHAR(100), ProgrammeTypeIdBefore SMALLINT, ProgrammeIn NVARCHAR(200), ProgrammeTypeIdIn SMALLINT, 
		BroadcastingDescriptionIdBefore int NULL , BroadcastingDescriptionIdIn int null, BroadcastingDescriptionIdAfter int null )

	INSERT INTO #Pricing_TvMessageTempTable (Id, BlockPosition, Footage, Rating, SponsoredProgrammeId, IsSponsoredProgrammeUserDefined,
		TapeAgencyName, TapeCode, TapeLength, TapeName, SponsoredProgrammeName,
		BlockId, BlockEnd, BlockIdent, BlockRating, BlockStart, BlockUnits, BlockCode, ProgrammeAfter, ProgrammeTypeIdAfter, ProgrammeBefore, ProgrammeTypeIdBefore, ProgrammeIn, ProgrammeTypeIdIn, pb.BroadcastingDescriptionIdBefore, pb.BroadcastingDescriptionIdIn, pb.BroadcastingDescriptionIdAfter)

	SELECT tmm.Id, tmm.BlockPosition, tmm.Footage, tmm.Rating, tmm.SponsoredProgrammeId, tmm.IsSponsoredProgrammeUserDefined,
		ti.TapeAgencyName, ti.TapeCode, ti.TapeLength, ti.TapeName, sp.Name,
		pb.Id,pb.BlockEnd,pb.BlockIdent,pb.BlockRating,pb.BlockStart,pb.BlockUnits,pb.BlockCode, pb.ProgrammeAfter,pb.ProgrammeTypeIdAfter,pb.ProgrammeBefore, pb.ProgrammeTypeIdBefore,pb.ProgrammeIn, pb.ProgrammeTypeIdIn, pb.BroadcastingDescriptionIdBefore, pb.BroadcastingDescriptionIdIn, pb.BroadcastingDescriptionIdAfter
	FROM Media.TvMediaMessage tmm 
	LEFT JOIN Media.SponsoredProgramme sp ON tmm.SponsoredProgrammeId = sp.Id
	LEFT JOIN Media.ProgrammeBlock pb on tmm.ProgrammeBlockId=pb.Id
	LEFT JOIN Media.TapeInfo ti on tmm.TapeInfoId=ti.Id
	JOIN #Pricing_MessageTempTable pmtt ON pmtt.Id = tmm.Id	
	
	-- Motive
	CREATE TABLE #Pricing_MotiveTempTable (MotiveId INT, MotiveVersionId INT, MotiveName NVARCHAR(500), OwnerId INT, OwnerName NVARCHAR(200), ProductBrandName NVARCHAR(200))

	INSERT INTO #Pricing_MotiveTempTable (MotiveId, MotiveVersionId, MotiveName, OwnerId, OwnerName, ProductBrandName)
	SELECT DISTINCT mot.Id, mv.Id, mv.Name, o.Id, o.Name, pb.Name 
	FROM Media.Motive mot
	LEFT JOIN Media.MotiveVersion mv ON mv.MotiveId=mot.Id
	LEFT JOIN Media.Motivlet ml ON mv.PrimaryMotivletId=ml.Id
	LEFT JOIN Media.[Owner] o ON o.Id=ml.OwnerId
	LEFT JOIN Media.[ProductBrand] pb ON pb.Id = ml.ProductBrandId
	JOIN #Pricing_MessageTempTable pmtt ON pmtt.MotiveId = mot.Id AND mv.ActiveFrom<=pmtt.AdvertisedFrom and pmtt.AdvertisedFrom<mv.ActiveTo
	
	-- V�sledn� select
	SELECT 
		msg.Id,
		msg.AdvertisedFrom,
		msg.AdvertisedTo,
		msg.CodingPlausibilityId,
		msg.MediumId,
		msg.PlacementId,
		msg.PriceValue,
		msg.MediaTypeId,
		msg.AdvertisementTypeId,
		msg.NormCreativeId,
		msg.MediumId AS 'Medium.Id',
		msg.Medium AS 'Medium.MediumVersion.Name',
		msg.MediumOriginalId AS 'Medium.OriginalId',
		msg.PublisherId	AS 'Medium.MediumVersion.PublisherId',
		msg.PlacementId AS 'Placement.Id',
		msg.Placement AS 'Placement.Name',				
		tvmsg.BlockPosition,
		tvmsg.Footage,
		tvmsg.Rating,
		tvmsg.SponsoredProgrammeId,
		tvmsg.IsSponsoredProgrammeUserDefined,
		tvmsg.TapeAgencyName AS 'TapeInfo.TapeAgencyName',
		tvmsg.TapeCode AS 'TapeInfo.TapeCode',
		tvmsg.TapeLength AS 'TapeInfo.TapeLength',
		tvmsg.TapeName AS 'TapeInfo.TapeName',
		tvmsg.BlockId AS 'ProgrammeBlock.Id',
		tvmsg.BlockEnd AS 'ProgrammeBlock.BlockEnd',
		tvmsg.BlockIdent AS 'ProgrammeBlock.BlockIdent',
		tvmsg.BlockRating AS 'ProgrammeBlock.BlockRating',
		tvmsg.BlockStart AS 'ProgrammeBlock.BlockStart',
		tvmsg.BlockUnits  AS 'ProgrammeBlock.BlockUnits',
		tvmsg.BlockCode AS 'ProgrammeBlock.BlockCode',
		tvmsg.BlockIdent AS 'ProgrammeBlock.BlockIdent',
		tvmsg.ProgrammeAfter AS 'ProgrammeBlock.ProgrammeAfter',
		tvmsg.ProgrammeTypeIdAfter AS 'ProgrammeBlock.ProgrammeTypeIdAfter',
		tvmsg.ProgrammeBefore AS 'ProgrammeBlock.BroadcastingDescriptionBefore.Programme.Name',
		tvmsg.ProgrammeTypeIdBefore AS 'ProgrammeBlock.ProgrammeTypeIdBefore',		
		tvmsg.ProgrammeIn  AS 'ProgrammeBlock.ProgrammeIn',
		tvmsg.SponsoredProgrammeId AS 'SponsoredProgramme.Id',
		tvmsg.SponsoredProgrammeName AS 'SponsoredProgramme.Name',		
		tvmsg.BroadcastingDescriptionIdBefore AS 'ProgrammeBlock.BroadcastingDescriptionIdBefore',
		tvmsg.BroadcastingDescriptionIdIn AS 'ProgrammeBlock.BroadcastingDescriptionIdIn',
		tvmsg.BroadcastingDescriptionIdAfter AS 'ProgrammeBlock.BroadcastingDescriptionIdAfter',		
		mot.MotiveVersionId AS 'MotiveVersion.Id',
		mot.MotiveVersionId AS 'MotiveVersionId',
		mot.MotiveName AS 'MotiveVersion.Name',
		mot.OwnerId,
		mot.OwnerName,
		mot.ProductBrandName,		
		pt.en AS PrgType,
		bd.BroadcastingContentTypeId
	FROM #Pricing_MessageTempTable msg
	JOIN #Pricing_TvMessageTempTable tvmsg ON msg.Id = tvmsg.Id
	JOIN #Pricing_MotiveTempTable mot ON msg.MotiveId = mot.MotiveId	
	LEFT JOIN SimLog.PrgType pt ON pt.Id = tvmsg.[ProgrammeTypeIdIn]
	LEFT JOIN SimLog.BroadcastingDescription bd ON bd.Id = tvmsg.BroadcastingDescriptionIdIn
	
	-- Drop temp tabulek 
	DROP TABLE #Pricing_MessageTempTable
	DROP TABLE #Pricing_TvMessageTempTable
	DROP TABLE #Pricing_MotiveTempTable

END
