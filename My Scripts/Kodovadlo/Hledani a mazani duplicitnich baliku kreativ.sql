select top (1000) CreativeId, count(CreativeItemId) from Creative.CreativeToCreativeItem
group by CreativeId 
order by count(CreativeItemId) desc

select  CreativeItemId, count(CreativeId) PocetKreativNaKterychJeTentoItem from Creative.CreativeToCreativeItem
group by CreativeItemId 
having count(CreativeId) > 1
order by count(CreativeId) desc


select * from Creative.CreativeToCreativeItem where CreativeId = 13406002

select * from Creative.CreativeToCreativeItem where CreativeItemId = 2180086

select * from Creative.CreativeToCreativeItem where CreativeId in (583316, 539993)



select * from Creative.CreativeToCreativeItem where CreativeItemId in (2179869,
2180086,
2181304)

-- vyber vazby, na kterych jsou tyto creativeItemId
select * from Creative.CreativeToCreativeItem ctci where CreativeItemId in (2069690, 2071086, 2180086)


-- vyber vazby, na kterych jsou tyto creativeItemId as seskup je podle creativeId
select ctci.CreativeId, count(ctci.CreativeItemId) CountOfCreativeItemId from Creative.CreativeToCreativeItem ctci where CreativeItemId in (2069690, 2071086, 2180086)
group by ctci.CreativeId

-- pridam do baliku kde je creativeId 583316, itemy tak, aby byly stejne jako v baliku 539993 a jeden navic. Ten navic pak zkusim odmazat
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (539993, 2071086) 
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (539993, 2069690) 
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (539993, 2180086) 

-- ted mam dve stejne creativy. 
-- vyber vazby, na kterych jsou tyto creativeItemId
select * from Creative.CreativeToCreativeItem ctci where CreativeId in (539993, 583316)  order by CreativeId, creativeItemId

delete from Creative.CreativeToCreativeItem where CreativeId = 1154179 and CreativeItemId = 2069690

select top (100)* from Media.MediaMessage where CreativeId != NormCreativeId


--TEST
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 4187945)  --navic
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 2071086)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3472095) 
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3472255)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3472291)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3472295)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3472811)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3491751)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3492106)
insert into Creative.CreativeToCreativeItem (CreativeId, CreativeItemId) values (1154179, 3833943)

update media.MediaMessage set CreativeId = 1154179 where Id in(18526866, 17444846, 18634610, 17946648, 15952070, 17962483, 16018443, 18629072, 15980324, 15980329, 16046563, 18636402) 
update media.MediaMessage set NormCreativeId = 12345 where Id in(18526866, 17444846, 18634610, 17946648, 15952070, 17962483, 16018443, 18629072, 15980324, 15980329, 16046563, 18636402) 


select * from Creative.CreativeToCreativeItem where CreativeId in (1154179, 1152211)order by CreativeId, creativeItemId
select Id, CreativeId, NormCreativeId from media.MediaMessage where CreativeId = 1154179
select Id, CreativeId, NormCreativeId from media.MediaMessage where Id in (18526866, 17444846)
