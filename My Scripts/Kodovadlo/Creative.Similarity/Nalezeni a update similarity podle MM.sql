/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [NormCreativeItemId]
      ,[ComparedCreativeItemId]
      ,[SimilarityInPercent]      
      ,[SimilarityKindId]
      ,[SimilarityResolvedStatusId]     
  FROM [Creative].[CreativeSimilarity] WHERE 
  Creative.CreativeSimilarity.NormCreativeItemId = 11570032 AND    -- leva strana DH okna
  Creative.CreativeSimilarity.ComparedCreativeItemId IN (11570034)  -- prava strana DH okna

  select  ci.CreativeMatchingStatusId, ci.CreativeMatchingProcessId, * from media.mediamessage mm
inner join Creative.CreativeToCreativeItem ctci on ctci.CreativeId=mm.NormCreativeId
inner join Creative.CreativeItem ci on ci.id = ctci.CreativeItemId
where ci.id= 11569933

SELECT * FROM Media.MediaMessage mm WHERE Id = 127967970

--SELECT * FROM Creative.CreativeToCreativeItem ctci WHERE ctci.CreativeItemId IN (11570013, 11570035) 
--SELECT * FROM Media.MediaMessage mm WHERE mm.CreativeId in(16606253, 16606273)










								-- VRACENI DO KODOVADLA UPDATNUTIM TECH SIMILARIT U KTERYCH SE ROZHODLO, ZE JSOU ROZDILNE

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

-- Kontrola:
SELECT * FROM  #TempCreativeSimilarity t INNER JOIN Creative.CreativeSimilarity cs 
ON t.NormCreativeItemId = cs.NormCreativeItemId AND t.ComparedCreativeItemId = cs.ComparedCreativeItemId

DROP TABLE #TempCreativeSimilarity













-- Mira:
update Creative.CreativeSimilarity set SimilarityResolvedStatusId=0 where NormCreativeItemId=11470941 and ComparedCreativeItemId=11605471

select  ci.CreativeMatchingStatusId, ci.CreativeMatchingProcessId, * from media.mediamessage mm
inner join Creative.CreativeToCreativeItem ctci on ctci.CreativeId=mm.NormCreativeId
inner join Creative.CreativeItem ci on ci.id = ctci.CreativeItemId
where ci.id= 11605471

update Media.MediaMessage set CodingPlausibilityId = 1 where id = 32421024
--







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

