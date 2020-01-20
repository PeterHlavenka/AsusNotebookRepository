--Takto se nacitaji CreativeItemy do Normovadla. Zalozka PictureMatching - duplicity.

--Takto se prelozi nacteni duplicit (odchyceno profilerem):
  exec sp_executesql N'SELECT
	[unsureCreative].[Id],
	[unsureCreative].[ContentTypeId],
	[ctci].[CreativeId]
FROM
	[Creative].[CreativeItem] [unsureCreative]
		INNER JOIN [Creative].[CreativeToCreativeItem] [ctci] ON [unsureCreative].[Id] = [ctci].[CreativeItemId]
WHERE
	Convert(Int, [unsureCreative].[CreativeMatchingStatusId]) = @p1 AND
	[unsureCreative].[CreativeMatchingProcessId] = @p2
',N'@p1 int,@p2 int',@p1=5,@p2=7



--1) Najdi top 10 similarit:
-------------------------------------------------------------------------------------------------------------------------------------------
SELECT TOP 10 cx.* FROM creative.CreativeSimilarity cx				-- najdi mi deset zaznamu v tabulce CreativeSimilarity,
WHERE cx.SimilarityKindId = 2										-- kde podobnost je od ImageDuplicityHuntera (SimilarityKind = 2)
ORDER BY cx.Modified DESC      
-------------------------------------------------------------------------------------------------------------------------------------------

-- 2) Updatni CreativeItemum  (jejichz Id je NORM_CreativeItemId z radku v predchozi query) CMS a CMP:
UPDATE Creative.CreativeItem
SET  
    Creative.CreativeItem.CreativeMatchingStatusId = 5, -- tinyint
    Creative.CreativeItem.CreativeMatchingProcessId = 7 -- tinyint
	--Creative.CreativeItem.CreativeUri = '\\192.168.0.2\documents\Groups\11-Admosphere\04-Doc\PRODUKCE\DEKLARACE\KREATIVA\2017\Outdoor\Super poster\201706_SP_kreativy\OC_CHODOV_06.jpg'
  WHERE Creative.CreativeItem.Id IN (
										SELECT TOP 10 cx.NormCreativeItemId FROM creative.CreativeSimilarity cx				 -- najdi mi deset zaznamu v tabulce CreativeSimilarity,
										WHERE cx.SimilarityKindId = 2														 -- kde podobnost je od ImageDuplicityHuntera (SimilarityKind = 2)
										ORDER BY cx.Modified DESC
									)
-- 3) Updatni CreativeItemum  (jejichz Id je COMPARED_CreativeItemId z radku v predchozi query) CMS a CMP :
UPDATE Creative.CreativeItem
SET  
    Creative.CreativeItem.CreativeMatchingStatusId = 5, -- tinyint
    Creative.CreativeItem.CreativeMatchingProcessId = 7 -- tinyint
	--Creative.CreativeItem.CreativeUri = '\\192.168.0.2\documents\Groups\11-Admosphere\04-Doc\PRODUKCE\DEKLARACE\KREATIVA\2017\Outdoor\Super poster\201706_SP_kreativy\OC_CHODOV_06.jpg'
  WHERE Creative.CreativeItem.Id IN (
										SELECT TOP 10 cx.ComparedCreativeItemId FROM creative.CreativeSimilarity cx				 -- najdi mi deset zaznamu v tabulce CreativeSimilarity,
										WHERE cx.SimilarityKindId = 2															 -- kde podobnost je od ImageDuplicityHuntera (SimilarityKind = 2)
										ORDER BY cx.Modified DESC
									)

									-- TO BY MELO STACIT. NORMOVADLO NA ZALOZCE PICTURE MATCHING DUPLICITY NYNI MUSI NACIST DUPLICITY