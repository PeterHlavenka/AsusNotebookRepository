
-------------------------------------------------------------------------------------------------------------------------------------------- 
--											PRVNI KROK - VYPNOUT A DISABLOVAT DUPLICITY KILLERA (SLUZBA SCHEDULE) NA SPRAVNEM SERVERU.
--------------------------------------------------------------------------------------------------------------------------------------------


-------------------------------------------------------------------------------------------------------------------------------------------- 
-- Update similarity na zaklade vlozeneho MM.Id.  Vhodne na vraceni prave resene mediaMessage v Kodovadle. (znam MM.Id)
--------------------------------------------------------------------------------------------------------------------------------------------
DECLARE @MediaMessageId int;
SELECT @MediaMessageId = 20669937 -- vlozit MM.Id

DECLARE @CreativeItemId int;
SELECT @CreativeItemId = 
(
	SELECT ci.Id FROM Media.MediaMessage mm 
	JOIN Creative.CreativeToCreativeItem ctci ON mm.CreativeId = ctci.CreativeId
	JOIN Creative.CreativeItem ci ON ctci.CreativeItemId = ci.Id
	WHERE mm.Id = @MediaMessageId
)
UPDATE Creative.CreativeSimilarity 
SET Creative.CreativeSimilarity.SimilarityResolvedStatusId = 0, Creative.CreativeSimilarity.CodingResultProcessId = null 
WHERE Creative.CreativeSimilarity.NormCreativeItemId = 590461 OR Creative.CreativeSimilarity.ComparedCreativeItemId = @CreativeItemId




 SELECT * FROM Media.ProductBrand pb WHERE pb.Name LIKE'%specifie%'


								
-------------------------------------------------------------------------------------------------------------------------------------------- 
-- VRACENI DO KODOVADLA UPDATNUTIM TECH SIMILARIT U KTERYCH SE ROZHODLO, ZE JSOU ROZDILNE
-------------------------------------------------------------------------------------------------------------------------------------------- 

-- Vytvorim temp table, nainsertuji do ni nejnovejsi similarity u kterych se rozhodlo, ze jsou rozdilne:
CREATE TABLE #TempCreativeSimilarity
(
NormCreativeItemId int NOT NULL,
ComparedCreativeItemId int NOT NULL 
)
INSERT INTO #TempCreativeSimilarity
SELECT TOP 10 cs.NormCreativeItemId, cs.ComparedCreativeItemId FROM Creative.CreativeSimilarity cs 
WHERE 
cs.SimilarityKindId = 3  OR cs.SimilarityKindId = 5 -- video
ORDER BY cs.Modified DESC

-- Updatnu SimilarityResolvedStatusId na 0 - New
UPDATE cs
SET cs.SimilarityResolvedStatusId = 0, cs.CodingResultProcessId = null 
FROM #TempCreativeSimilarity t INNER JOIN Creative.CreativeSimilarity cs 
ON t.NormCreativeItemId = cs.NormCreativeItemId AND t.ComparedCreativeItemId = cs.ComparedCreativeItemId

-- Kontrola (seskupi radky podle cs.NormCreativeItemId, cs.ComparedCreativeItemId  a z kazde skupiny vezme jen jeden zaznam (Normalne join roznasobi radky, protoze najde vice MM ktere odpovidaji temto creativeItemId) ):
;WITH ##KontrolaCTE AS
(
	SELECT MM.Id MediaMessageId, MM.CodingPlausibilityId, cs.*,
	ROW_NUMBER() OVER (PARTITION BY cs.NormCreativeItemId, cs.ComparedCreativeItemId ORDER BY cs.NormCreativeItemId) AS RowNumber
	FROM  #TempCreativeSimilarity t 
	INNER JOIN Creative.CreativeSimilarity cs ON t.NormCreativeItemId = cs.NormCreativeItemId AND t.ComparedCreativeItemId = cs.ComparedCreativeItemId
	JOIN Creative.CreativeToCreativeItem ctci ON ctci.CreativeItemId = cs.ComparedCreativeItemId
	JOIN Media.MediaMessage MM ON ctci.CreativeId = MM.CreativeId
)
SELECT * FROM ##KontrolaCTE kc WHERE kc.RowNumber = 1
DROP TABLE #TempCreativeSimilarity




-------------------------------------------------------------------------------------------------------------------------------------------- 
-- Pomucka - update plausibility pro nalezeneId Id:
-------------------------------------------------------------------------------------------------------------------------------------------- 
UPDATE Media.MediaMessage SET Media.MediaMessage.CodingPlausibilityId = 1 WHERE Id IN (
20613823,
20677744,
20630968)
SELECT mm.Id, mm.CodingPlausibilityId FROM Media.MediaMessage mm WHERE Id IN  (
20613823,
20677744,
20630968)

-------------------------------------------------------------------------------------------------------------------------------------------- 
-- Obsah tabulek:
-------------------------------------------------------------------------------------------------------------------------------------------- 
  SELECT * FROM Creative.SimilarityKind sk
	--  Id			Name						Description
	--	1			Normal						Běžná PM podobnost
	--	2			ImageDuplicityHunter		Podobnost z PM duplicity huntera
	--	3			VideoDuplicityHunter		Podobnost z VM duplicity huntera
	--	4			PMTest-EyeSearch2Mirror		Test matchovani pred prechodem na novou verzi enginu 2016-08
	--	5			VideoDHIncremental			Podobnost z inkrementálního VM duplicity huntera

  SELECT * FROM Creative.SimilarityResolvedStatus srs
	--  Id	Name				
	--	0	New					
	--	1	DoneDuplicityHunter	

-- Sloupec Creative.CreativeSimilarity.CodingResultProcessid vede sem:
SELECT * FROM Creative.CreativeMatchingProcess cmp
	--Id	Name						Description
	--1		Sure						Proces porovnání našel normu pro tuto kreativu
	--2		Unsure						Proces porovnání našel normu pro tuto kreativu, ale shoda není jistá
	--3		NotFound					Proces porovnání nenašel normu pro tuto kreativu
	--4		NotNormNorming				Kreativa není norma podle normovadla
	--5		NewNorm						Nová norma
	--6		NotNormCoding				Kreativa byla okódována a není to norma
	--7		DuplicitySuspicion			Proces porovnání vyhodnotil normu jako duplicity jiné normy
	--8		NewNormSure					Nová norma, která určitě není duplicita jiné normy
	--9		MultipleCreative			Nová kreativa, která je součástí media message s více kreativami a je potřeba ji okódovat ručně
	--10	RemovedNorm					Kreativa, která byla původně normou, ale momentálně už není
	--11	MatchingSkipped				Kreativa, u které byl proces porovnání přeskočen
	--12	MatchingError				Nastala chyba při matchování
	--13	SmallFormatNew				Malý formát, který ještě neprošel PM
	--14	SmallFormatSure				Malý formát, který byl vyhodnocený jako stejný s jiným
	--15	SmallFormatNorm				Malý formát, pro který nebyl nalezen žádný stejný
	--16	SmallFormatNormSure			NULL
	--17	SmallFormatRemoved			NULL
	--18	NotMatchingSubjectCoding	Uživatel Kódovadla označil kreativu jako NotMatchingSubject
	--19	HunterDuplicitySuspicion	DuplicityHunter označil kreativu za duplicitní
	--20	PreliminarySure				Sure kreativa z PM celostran, která zatím neprošla Vystřihovadlem
	--21	PreliminaryUnsure			Unsure kreativa z PM celostran, která zatím neprošla Vystřihovadlem
	--22	ConfirmedUnsure				Unsure kreativa z PM celostran, která prošla Vystřihovadlem
	--23	UserConfirmedUnsure			Unsure kreativa z PM celostran, kterou uživatel ve Vystřihovadle potvrdil

	--BTW...pokud mám ještě nějaké na rozhodnutí a potřebuješ rozhodovat "shodné" (not new norm)...tak si vypni ScheduleSlužbu(dam běží DuplicityKiller) a on je nebude mazat z db

