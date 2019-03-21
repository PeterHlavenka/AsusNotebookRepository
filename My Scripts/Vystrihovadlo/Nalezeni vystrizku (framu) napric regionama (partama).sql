-- Nalezeni vystrizku (framu) napric regionama (partama)

-- divny, databaze mi tvrdi, ze v ty parte mas jeden vystrizek
-- a vic partu (regionu) ta publikace nema

select * from Media.PressMessageFrame     -- vyber vsechny framy
where PrintStoragePageId in (select PartId from PrintStorageAuto.dbo.Page		--ktere maji PageId IN (vyber vsechny PartId z dbo.Page, ktere maji PartId – 49784)
where PartId = 49784) 


--VYBER VSECHNY FRAMY PODLE PART ID
select * from Media.PressMessageFrame
where PrintStoragePageId in (
select Id from PrintStorageAuto.dbo.Page where PartId = 49784      -- vybere Id stran podle Partu
) 


--Jakub Sýkora 15:15: 
--to medium je aktivni...
select * from PrintStorageAuto.dbo.Region
where MediumId = 3054 or OriginalMediumId = 3054 

-- JESTLI JE MEDIUM AKTIVNI SE DA ZJISTIT NA MEDIA DATA 3 AUTO Z TABULKY MEDIUM VERSION
SELECT * FROM Media.Medium m 
JOIN Media.MediumVersion mv ON m.Id = mv.MediumId
WHERE m.Id = 3054

-- UPRAVENI PLATNOSTI MEDIA
--UPDATE Media.MediumVersion
--SET
--    Media.MediumVersion.ActiveFrom = '2019-01-01 15:13:32'
--where Media.MediumVersion.MediumId = 3054