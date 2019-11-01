
-- Nacteni MM do Zachytavadla - vrati 24 radku

exec sp_executesql N'CREATE TABLE #TempCatchingMessages (Id int, NormCreativeId INT, CreativeId INT, AdvertisedFrom DATETIME, AdvertisedTo DATETIME, CreatedBy TINYINT, ModifiedBy TINYINT, Footage SMALLINT, Modified DATETIME, MediumId SMALLINT, Medium NVARCHAR(255),
                    TapeCode VARCHAR(50), TapeName VARCHAR(100), PlacementId SMALLINT, Note VARCHAR(2000), BroadcastingDescriptionIdBefore INT NULL, ProgrammeBefore NVARCHAR(200), Motive nvarchar(500), ProgrammeAfter nVARCHAR(200), BroadcastingDescriptionIdAfter INT NULL, AdvertisementTypeId TINYINT, UserCreatedByName VARCHAR(100), 
                    UserModifiedByName VARCHAR(100), PlacementName VARCHAR(255), OwnerName NVARCHAR(200), AdvertisementTypeName NVARCHAR(255), CreativeItemId INT, CompanyBrand NVARCHAR(200), ProductBrand NVARCHAR(200), CodingPlausibility VARCHAR(25), 
                    MotiveId INT, NormsTapeCode VARCHAR(50))

                    INSERT INTO #TempCatchingMessages (Id,NormCreativeId, CreativeId, AdvertisedFrom,AdvertisedTo, CreatedBy, ModifiedBy,Footage, Modified,
                    MediumId, Medium, TapeCode, TapeName, PlacementId, Note, BroadcastingDescriptionIdBefore, BroadcastingDescriptionIdAfter, AdvertisementTypeId, UserCreatedByName,
                    UserModifiedByName, PlacementName, AdvertisementTypeName, CodingPlausibility)

                    SELECT mm.Id, mm.NormCreativeId, mm.CreativeId, mm.AdvertisedFrom, mm.AdvertisedTo, mm.CreatedBy, mm.ModifiedBy, tmm.Footage, mm.Modified, 
						mm.MediumId, mdv.Name AS Medium, ti.TapeCode, ti.TapeName, mm.PlacementId, mm.Note, pbl.BroadcastingDescriptionIdBefore,  
						pbl.BroadcastingDescriptionIdAfter, mm.AdvertisementTypeId, uc.UserName AS UserCreatedByName, um.UserName AS UserModifiedByName, p.Name AS PlacementName, 
						 at.Name AS AdvertisementTypeName, cp.Name AS CodingPlausibility
						
					FROM Media.MediaMessage mm 
					INNER JOIN Media.TvMediaMessage tmm ON mm.Id = tmm.Id 
					INNER JOIN Media.Medium md ON mm.MediumId = md.Id
					INNER JOIN Media.MediumVersion mdv ON md.Id = mdv.MediumId AND mdv.ActiveFrom <= mm.AdvertisedFrom AND mdv.ActiveTo > mm.AdvertisedFrom	
					INNER JOIN Media.CodingPlausibility cp ON mm.CodingPlausibilityId = cp.Id	

					LEFT JOIN Media.ProgrammeBlock pbl ON tmm.ProgrammeBlockId = pbl.Id
					LEFT JOIN Media.TapeInfo ti ON tmm.TapeInfoId = ti.Id
					LEFT JOIN Security.[User] uc ON mm.CreatedBy = uc.Id 
					LEFT JOIN Media.AdvertisementType at ON mm.AdvertisementTypeId = at.Id
					LEFT JOIN Security.[User] um ON mm.ModifiedBy = um.Id 
					LEFT JOIN Media.Placement p ON mm.PlacementId = p.Id 

					WHERE mm.AdvertisedFrom >= @fromHelper  AND mm.AdvertisedFrom < @to AND mm.AdvertisedTo > @from AND mm.AdvertisedTo < @to					
                    AND mm.MediumId IN (26)

                   ;WITH A AS -- #42544 Nahraj pro kazdou MM TapeCode od normy. Vezme se TapeCode od nejstarsi zpravy normy
                   (
                    SELECT temp.*, ti2.TapeCode AS CteNormsTapeCode,
                    ROW_NUMBER() OVER (PARTITION BY mm2.NormCreativeId, temp.Id ORDER BY mm2.Created) AS RN
                    FROM #TempCatchingMessages temp 
                    JOIN Media.MediaMessage mm2 ON mm2.NormCreativeId = temp.NormCreativeId
                    JOIN Media.TvMediaMessage tmm2 ON mm2.Id = tmm2.Id
                    LEFT JOIN Media.TapeInfo ti2 ON tmm2.TapeInfoId = ti2.Id		                    
                   )

				   UPDATE A SET  A.NormsTapeCode = A.CteNormsTapeCode  WHERE A.RN = 1	

				   ;WITH B AS -- Zjisti ProgrammeBefore a ProgrammeAfter na zaklade ProgrammeBlock.BroadcastingDescriptionIdBefore/After
                   (
                    SELECT temp.*, prBefore.Name as CTE_prBefore, prAfter.Name as CTE_prAfter
                    FROM #TempCatchingMessages temp 
					LEFT JOIN SimLog.BroadcastingDescription bdBefore on temp.BroadcastingDescriptionIdBefore = bdBefore.Id
					LEFT JOIN SimLog.Programme prBefore on bdBefore.ProgrammeId = prBefore.Id
					LEFT JOIN SimLog.BroadcastingDescription bdAfter on temp.BroadcastingDescriptionIdAfter = bdAfter.Id
					LEFT JOIN SimLog.Programme prAfter on bdAfter.ProgrammeId = prAfter.Id                   	                     
                   )
			   		
				   UPDATE B SET  B.ProgrammeBefore = B.CTE_prBefore, B.ProgrammeAfter = B.CTE_prAfter

INSERT INTO #TempCatchingMessages (Id,NormCreativeId, CreativeId, AdvertisedFrom, AdvertisedTo, CreatedBy, ModifiedBy, Footage, Modified, MediumId, Medium,
                    TapeCode, TapeName, PlacementId, Note, ProgrammeBefore, Motive, ProgrammeAfter, AdvertisementTypeId, UserCreatedByName, 
                    UserModifiedByName, PlacementName, OwnerName, AdvertisementTypeName, CompanyBrand, ProductBrand, CodingPlausibility, 
                    NormsTapeCode, MotiveId, CreativeItemId)
SELECT 
        tcm.Id, tcm.NormCreativeId, tcm.CreativeId, tcm.AdvertisedFrom, tcm.AdvertisedTo, tcm.CreatedBy, tcm.ModifiedBy, tcm.Footage, tcm.Modified, tcm.MediumId, tcm.Medium,
                    tcm.TapeCode, tcm.TapeName, tcm.PlacementId, tcm.Note, tcm.ProgrammeBefore, tcm.Motive, tcm.ProgrammeAfter, tcm.AdvertisementTypeId, tcm.UserCreatedByName, 
                    tcm.UserModifiedByName, tcm.PlacementName, tcm.OwnerName, tcm.AdvertisementTypeName, tcm.CompanyBrand, tcm.ProductBrand, tcm.CodingPlausibility, tcm.NormsTapeCode, 
                    cre.MotiveId, ctci.CreativeItemId

        FROM #TempCatchingMessages tcm 
        JOIN Creative.Creative cre on tcm.CreativeId = cre.Id
	    JOIN Creative.CreativeToCreativeItem ctci on tcm.CreativeId = ctci.CreativeId

DELETE FROM #TempCatchingMessages WHERE CreativeItemId IS NULL AND MotiveId IS NULL 
 
UPDATE tcm 
SET 
tcm.Motive = mv.Name,
tcm.OwnerName = o.Name,
tcm.CompanyBrand = cb.Name,
tcm.ProductBrand = pb.Name
FROM #TempCatchingMessages AS tcm 
					 JOIN Media.Motive m ON m.Id = tcm.MotiveId
					 JOIN Media.MotiveVersion mv ON mv.MotiveId = m.Id AND mv.ActiveFrom <= @from AND mv.ActiveTo >= @to 
					 JOIN Media.Motivlet ml ON mv.PrimaryMotivletId = ml.Id 
					 JOIN Media.Owner o ON ml.OwnerId = o.Id 
					 JOIN Media.CompanyBrand cb ON ml.CompanyBrandId = cb.Id 
					 JOIN Media.ProductBrand pb ON ml.ProductBrandId = pb.Id
SELECT * FROM #TempCatchingMessages',N'@from datetime,@to datetime,@fromHelper datetime',@from='2019-08-25 23:59:57',@to='2019-08-26 01:15:27',@fromHelper='2019-08-25 22:59:57'
