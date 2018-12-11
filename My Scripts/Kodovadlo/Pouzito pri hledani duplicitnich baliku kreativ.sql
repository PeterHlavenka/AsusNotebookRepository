SELECT * FROM History.CreativeToCreativeItem
WHERE CreativeId IN (15345174, 15345172)

select HistoryDate, Id MessageId, NormCreativeId, CreativeId from History.MediaMessage where CREATIVEID IN (15345174, 15345172)  order by HistoryDate 

select Id MessageId, NormCreativeId, CreativeId from media.MediaMessage where CREATIVEID IN (15345174, 15345172)

-- Ukaz message ktere ukazuji na tyto dve kreativy a ukaz vazby ve kterych jsou tyto kreativy 
------------------------------------------
select Id MessageId, NormCreativeId, CreativeId, Modified, ModifiedBy from media.MediaMessage where CREATIVEID IN (15345174, 15345172)

SELECT * FROM Creative.CreativeToCreativeItem
WHERE CreativeId IN (15345174, 15345172)
----------------------------------------------

-- Uprava pouzivanych kreativ pro opetovne pouziti
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345172, 10587409)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345174, 10587399)

insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345172, 1000000)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345174, 1000000)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345172, 1000003)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345174, 1000003)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345172, 1000007)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (15345174, 1000007)
select top (3) Id from Creative.CreativeItem

delete from Creative.CreativeToCreativeItem where CreativeItemId in (1000003, 1000007) and CreativeId in (15345172)

-- vraceni messagi do puvodniho stavu
update Media.MediaMessage set NormCreativeId = 123456, CreativeId = 15345174  where Id in (97149134, 97149135, 97149136, 97149137) 
update Media.MediaMessage set NormCreativeId = 1 where NormCreativeId = 15345172

-- najde itemy ktere jsou na nejvice kreativach
select top(20) it.Id, count(cre.Id) from Creative.CreativeItem it
join Creative.CreativeToCreativeItem ctci on it.Id = ctci.CreativeItemId
join Creative.Creative cre on ctci.CreativeId = cre.Id
group by it.Id
having count(cre.Id) = 3
order by count(cre.Id) desc


