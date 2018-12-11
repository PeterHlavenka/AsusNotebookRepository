exec sp_executesql N'
                    SELECT mm.Id, mm.NormCreativeId, mm.CreativeId, mm.AdvertisedFrom, mm.AdvertisedTo, mm.CreatedBy, mm.ModifiedBy, tmm.Footage, mm.Modified, mm.MediumId, mdv.Name AS Medium, 
					mm.PlacementId, mm.Note,
						 mm.AdvertisementTypeId, cp.Name AS CodingPlausibility
					FROM Media.MediaMessage mm 
					INNER JOIN Media.TvMediaMessage tmm ON mm.Id = tmm.Id 
					INNER JOIN Media.Medium md ON mm.MediumId = md.Id
					INNER JOIN Media.MediumVersion mdv ON md.Id = mdv.MediumId AND mdv.ActiveFrom <= mm.AdvertisedFrom AND mdv.ActiveTo > mm.AdvertisedFrom	
					INNER JOIN Media.CodingPlausibility cp ON mm.CodingPlausibilityId = cp.Id						
					WHERE mm.AdvertisedFrom >= @fromHelper  AND mm.AdvertisedFrom < @to AND mm.AdvertisedTo > @from AND mm.AdvertisedTo < @to					
                    AND mm.MediumId IN (491) 

					
					                   
                    --;WITH A AS -- #42544 Nahraj pro kazdou MM TapeCode od normy. Vezme se TapeCode od nejstarsi zpravy normy
                    --(
	                   -- SELECT temp.*, ti2.TapeCode AS NormsTapeCode,
	                   -- ROW_NUMBER() OVER (PARTITION BY mm2.NormCreativeId, temp.Id ORDER BY mm2.Created) AS RN
	                   -- FROM @TempCatchingMessages temp 
	                   -- JOIN Media.MediaMessage mm2 ON mm2.NormCreativeId = temp.NormCreativeId
	                   -- JOIN Media.TvMediaMessage tmm2 ON mm2.Id = tmm2.Id
	                   -- LEFT JOIN Media.TapeInfo ti2 ON tmm2.TapeInfoId = ti2.Id		                    
                    --)					
                    --SELECT * FROM A WHERE a.RN = 1
					',N'@from datetime,@to datetime,@fromHelper datetime',
					@from='2018-08-18 06:59:55.600',@to='2018-08-18 08:15:25.600',@fromHelper='2018-08-18 05:59:55.600'
					

					--TEST


					CREATE TABLE ##TempCatchingMessages (Id int, NormCreativeId INT, CreativeId INT, AdvertisedFrom DATETIME, AdvertisedTo DATETIME, CreatedBy TINYINT, ModifiedBy TINYINT, Footage SMALLINT, Modified DATETIME, MediumId SMALLINT, Medium NVARCHAR(255),
                    TapeCode VARCHAR(50), TapeName VARCHAR(100), PlacementId SMALLINT, Note VARCHAR(2000), ProgrammeBefore NVARCHAR(200), Motive varchar(500), ProgrammeAfter VARCHAR(200), AdvertisementTypeId TINYINT, UserCreatedByName VARCHAR(100), 
                    UserModifiedByName VARCHAR(100), PlacementName VARCHAR(255), OwnerName NVARCHAR(200), AdvertisementTypeName NVARCHAR(255), CreativeItemId INT, CompanyBrand NVARCHAR(200), ProductBrand NVARCHAR(200), CodingPlausibility VARCHAR(25), MotiveId INT, ProgrammeBlockId INT)

					DECLARE @fromHelper datetime
					, @to datetime, @from datetime

						select	@from='2018-08-18 06:59:55.600',@to='2018-08-18 08:15:25.600',@fromHelper='2018-08-18 05:59:55.600'

              INSERT INTO ##TempCatchingMessages (Id,NormCreativeId, CreativeId, AdvertisedFrom,AdvertisedTo, CreatedBy, ModifiedBy, Footage, Modified,
                    MediumId, Medium, PlacementId, Note, AdvertisementTypeId, CodingPlausibility, ProgrammeBlockId)
              

SELECT mm.Id, mm.NormCreativeId, mm.CreativeId, mm.AdvertisedFrom, mm.AdvertisedTo, mm.CreatedBy, mm.ModifiedBy, tmm.Footage, mm.Modified, mm.MediumId, mdv.Name AS Medium, 
					mm.PlacementId, mm.Note,
					mm.AdvertisementTypeId,
                    cp.Name AS CodingPlausibility,
					tmm.ProgrammeBlockId
					FROM Media.MediaMessage mm 
					INNER JOIN Media.TvMediaMessage tmm ON mm.Id = tmm.Id 
					INNER JOIN Media.Medium md ON mm.MediumId = md.Id
					INNER JOIN Media.MediumVersion mdv ON md.Id = mdv.MediumId AND mdv.ActiveFrom <= mm.AdvertisedFrom AND mdv.ActiveTo > mm.AdvertisedFrom	
					INNER JOIN Media.CodingPlausibility cp ON mm.CodingPlausibilityId = cp.Id	
					WHERE mm.AdvertisedFrom >= @fromHelper  AND mm.AdvertisedFrom < @to AND mm.AdvertisedTo > @from AND mm.AdvertisedTo < @to					
                    AND mm.MediumId IN (491)

			

					SELECT * FROM ##TempCatchingMessages tcm
					DROP table ##TempCatchingMessages 


--exec sp_executesql N'
--                    CREATE TABLE ##TempCatchingMessages (Id int, NormCreativeId INT, CreativeId INT, AdvertisedFrom DATETIME, AdvertisedTo DATETIME, CreatedBy TINYINT, ModifiedBy TINYINT, Footage SMALLINT, Modified DATETIME, MediumId SMALLINT, Medium NVARCHAR(255),
--                    TapeCode VARCHAR(50), TapeName VARCHAR(100), PlacementId SMALLINT, Note VARCHAR(2000), ProgrammeBefore NVARCHAR(200), Motive varchar(500), ProgrammeAfter VARCHAR(200), AdvertisementTypeId TINYINT, UserCreatedByName VARCHAR(100), 
--                    UserModifiedByName VARCHAR(100), PlacementName VARCHAR(255), OwnerName NVARCHAR(200), AdvertisementTypeName NVARCHAR(255), CreativeItemId INT, CompanyBrand NVARCHAR(200), ProductBrand NVARCHAR(200), CodingPlausibility VARCHAR(25), MotiveId INT)

--              INSERT INTO ##TempCatchingMessages (Id,NormCreativeId, CreativeId, AdvertisedFrom,AdvertisedTo, CreatedBy, ModifiedBy, Footage, Modified,
--                    MediumId, Medium, PlacementId, Note, AdvertisementTypeId, CodingPlausibility)
              

--SELECT mm.Id, mm.NormCreativeId, mm.CreativeId, mm.AdvertisedFrom, mm.AdvertisedTo, mm.CreatedBy, mm.ModifiedBy, tmm.Footage, mm.Modified, mm.MediumId, mdv.Name AS Medium, 
--					mm.PlacementId, mm.Note,
--					mm.AdvertisementTypeId,
--                    cp.Name AS CodingPlausibility,

--					FROM Media.MediaMessage mm 
--					INNER JOIN Media.TvMediaMessage tmm ON mm.Id = tmm.Id 
--					INNER JOIN Media.Medium md ON mm.MediumId = md.Id
--					INNER JOIN Media.MediumVersion mdv ON md.Id = mdv.MediumId AND mdv.ActiveFrom <= mm.AdvertisedFrom AND mdv.ActiveTo > mm.AdvertisedFrom	
--					INNER JOIN Media.CodingPlausibility cp ON mm.CodingPlausibilityId = cp.Id	
--					WHERE mm.AdvertisedFrom >= @fromHelper  AND mm.AdvertisedFrom < @to AND mm.AdvertisedTo > @from AND mm.AdvertisedTo < @to					
--                    AND mm.MediumId IN (491)
--					',N'@from datetime,@to datetime,@fromHelper datetime',
--					@from='2018-08-18 06:59:55.600',@to='2018-08-18 08:15:25.600',@fromHelper='2018-08-18 05:59:55.600'
	
	
	-----------------------------------
--2
						
	UPDATE  ##TempCatchingMessages set 

	ProgrammeBefore = pbl.ProgrammeBefore,
	ProgrammeAfter = pbl.ProgrammeAfter, 
	UserCreatedByName = uc.UserName, 
	UserModifiedByName = um.UserName, 
	PlacementName = p.Name,
	AdvertisementTypeName = at.Name	

    FROM ##TempCatchingMessages tcm 

				     
					 JOIN Media.ProgrammeBlock pbl ON tcm.ProgrammeBlockId = pbl.Id
					 --JOIN Media.TapeInfo ti ON tcm.TapeInfoId = ti.Id
					 JOIN Security.[User] uc ON tcm.CreatedBy = uc.Id 
					 JOIN Media.AdvertisementType at ON tcm.AdvertisementTypeId = at.Id
					 JOIN Security.[User] um ON tcm.ModifiedBy = um.Id 
					 LEFT JOIN Media.Placement p ON tcm.PlacementId = p.Id 


					
SELECT * FROM MEDIA.ProgrammeBlock pb WHERE ID = 19405305			
 	
--------------------------------------
	--3  najoinuju ctci a updatnu z ni creativeItem v temp. Problem je, kdyz bude vice creativeItemu pro jedno CreativeId. Pak se mi tabulka rozroste. 

	
	UPDATE  tcm set 	 
	tcm.CreativeItemId = ctci.CreativeItemId
	from ##TempCatchingMessages as tcm
	JOIN Creative.CreativeToCreativeItem ctci on tcm.CreativeId = ctci.CreativeId	

    SELECT * FROM ##TempCatchingMessages tcm 
				     
									