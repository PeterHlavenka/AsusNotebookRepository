

exec sp_executesql N'DECLARE @TempCatchingMessages TABLE(Id int, NormCreativeId INT, CreativeId INT, AdvertisedFrom DATETIME, AdvertisedTo DATETIME, CreatedBy TINYINT, ModifiedBy TINYINT, Footage SMALLINT, Modified DATETIME, MediumId SMALLINT, Medium NVARCHAR(255),
                    TapeCode VARCHAR(50), TapeName VARCHAR(100), PlacementId SMALLINT, Note VARCHAR(2000), ProgrammeBefore NVARCHAR(200), Motive varchar(500), ProgrammeAfter VARCHAR(200), AdvertisementTypeId TINYINT, UserCreatedByName VARCHAR(100), 
                    UserModifiedByName VARCHAR(100), PlacementName VARCHAR(255), OwnerName NVARCHAR(200), AdvertisementTypeName NVARCHAR(255), CreativeItemId INT, CompanyBrand NVARCHAR(200), ProductBrand NVARCHAR(200), CodingPlausibility VARCHAR(25), MotiveId INT)

                    INSERT INTO @TempCatchingMessages (Id,NormCreativeId, CreativeId, AdvertisedFrom,AdvertisedTo, CreatedBy, ModifiedBy,Footage, Modified,
                    MediumId,Medium,TapeCode,TapeName,PlacementId,Note,ProgrammeBefore,Motive,ProgrammeAfter,AdvertisementTypeId,UserCreatedByName,
                    UserModifiedByName,PlacementName,OwnerName,AdvertisementTypeName,CreativeItemId,CompanyBrand,ProductBrand,CodingPlausibility,MotiveId)
                    SELECT mm.Id, mm.NormCreativeId, mm.CreativeId, mm.AdvertisedFrom, mm.AdvertisedTo, mm.CreatedBy, mm.ModifiedBy, tmm.Footage, mm.Modified, mm.MediumId, mdv.Name AS Medium, ti.TapeCode, ti.TapeName, mm.PlacementId, mm.Note, pbl.ProgrammeBefore, mv.Name AS Motive,
						pbl.ProgrammeAfter, mm.AdvertisementTypeId, uc.UserName AS UserCreatedByName, um.UserName AS UserModifiedByName, p.Name AS PlacementName, o.Name AS OwnerName, at.Name AS AdvertisementTypeName, ctci.CreativeItemId, cb.Name AS CompanyBrand, pb.Name AS ProductBrand, cp.Name AS CodingPlausibility,
						mv.MotiveId AS MotiveId
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
					LEFT JOIN Creative.Creative c ON mm.CreativeId = c.Id 
					LEFT JOIN Creative.CreativeToCreativeItem ctci ON ctci.CreativeId = c.Id 	
					LEFT JOIN Media.Motive m ON m.Id = c.MotiveId
					LEFT JOIN Media.MotiveVersion mv ON mv.MotiveId = m.Id AND mv.ActiveFrom <= @from AND mv.ActiveTo >= @to 
					LEFT JOIN Media.Motivlet ml ON mv.PrimaryMotivletId = ml.Id 
					LEFT JOIN Media.Owner o ON ml.OwnerId = o.Id 
					LEFT JOIN Media.CompanyBrand cb ON ml.CompanyBrandId = cb.Id 
					LEFT JOIN Media.ProductBrand pb ON ml.ProductBrandId = pb.Id	
					LEFT JOIN Media.SponsoredProgramme sp ON tmm.SponsoredProgrammeId = sp.Id					
					WHERE mm.AdvertisedFrom >= @fromHelper  AND mm.AdvertisedFrom < @to AND mm.AdvertisedTo > @from AND mm.AdvertisedTo < @to					
                    AND mm.MediumId IN (491)                    
                    ;WITH A AS -- #42544 Nahraj pro kazdou MM TapeCode od normy. Vezme se TapeCode od nejstarsi zpravy normy
                    (
	                    SELECT temp.*, ti2.TapeCode AS NormsTapeCode,
	                    ROW_NUMBER() OVER (PARTITION BY mm2.NormCreativeId, temp.Id ORDER BY mm2.Created) AS RN
	                    FROM @TempCatchingMessages temp 
	                    JOIN Media.MediaMessage mm2 ON mm2.NormCreativeId = temp.NormCreativeId
	                    JOIN Media.TvMediaMessage tmm2 ON mm2.Id = tmm2.Id
	                    LEFT JOIN Media.TapeInfo ti2 ON tmm2.TapeInfoId = ti2.Id		                    
                    )					
                    SELECT * FROM A WHERE a.RN = 1',N'@from datetime,@to datetime,@fromHelper datetime',@from='2018-08-18 06:59:55.600',@to='2018-08-18 08:15:25.600',@fromHelper='2018-08-18 05:59:55.600'