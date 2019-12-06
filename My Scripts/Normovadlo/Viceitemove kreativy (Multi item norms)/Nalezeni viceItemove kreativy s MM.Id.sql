


 -- viceitemovky od Miry -  Nalezeni viceItemove kreativy s MM.Id . Image kreativa muze mit neomezene itemu. 

select top 100 [A].[CreativeId], A.[Items], max(mm.id) as repreId
from (
	select ctci.[CreativeId], count(*) as Items 
	from [Creative].[CreativeItem] [ci]
		inner join [Creative].[CreativeToCreativeItem] [ctci] on [ctci].[CreativeItemId] = [ci].[Id]
	where [ci].[ContentTypeId] = 4
	group by [ctci].[CreativeId]
	having count(*) > 1
) A
	inner join [Media].[MediaMessage] [mm] on mm.[NormCreativeId] = A.[CreativeId]
 where mm.[AdvertisedFrom]>='20191101'
group by A.[CreativeId], A.[Items] 