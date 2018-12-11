SET XACT_ABORT ON
SET TRANSACTION ISOLATION LEVEL SNAPSHOT

SELECT mv.MotiveId, mv.Id as 'motiveVersionID',
convert (varchar, mv.ActiveFrom, 104) as 'platný OD', convert (varchar, mv.ActiveTo, 104) as 'platný DO',
--mv.ActiveFrom ,mv.ActiveTo ,
mv.Name as 'MotiveName', 
CASE WHEN mv.PrimaryMotivletId = ml.Id THEN 'Primár' ELSE 'Sekundár' END AS 'Prvek motivu',
mv.PrimaryMotivletId, 
ml.Id as 'MotiveLetID' , ml.OwnerId , o.Name as 'Zadavatel', o.CRN as 'IÈO', ml.CompanyBrandId , cb.Name as 'Firemni' , ml.ProductBrandId , pb.Name as 'Produktova',
ml.CategoryId , c.Code , c.Name as 'Categorie'
, r.Name as 'role',ma.Name as 'Subrole', tl.Name as 'telko', ge.Name as 'Pohlaví', pl.Name as 'Cíl'
, mv.Created as 'vytvoren Motiv', u.UserName as 'Kdo'


from Media.MotiveVersion mv
left JOIN Media.MotiveVersionToMotivlet mvtml on mvtml.MotiveVersionId = mv.Id  
left JOIN Media.Motivlet ml ON ml.Id = mvtml.MotivletId 
left JOIN Security.[User] u ON u.Id = mv.CreatedBy 

left JOIN Media.ProductBrand pb ON pb.Id=ml.ProductBrandId
left JOIN Media.ProductDetail pd ON pd.Id=ml.ProductDetailId

left JOIN Media.CompanyBrand cb ON cb.Id=ml.CompanyBrandId
left JOIN Media.Owner o ON ml.OwnerId=o.Id
left JOIN Media.Category c ON c.Id = ml.CategoryId

left JOIN Media.Role r ON r.Id=ml.RoleId
left JOIN Media.Platform pl on pl.Id = ml.PlatformId
left JOIN Media.Telco tl on tl.Id = ml.TelcoId
left JOIN Media.Gender ge on ge.Id = ml.GenderId
left JOIN Media.Market ma on ma.Id = ml.MarketId

where 
--mv.MotiveId in ()
--mv.in (1074736)
--o.Name LIKE ('tablet%')-- AND o.CRN in (45278776)
--cb.Name like ('etržby')
--c.Code LIKE ('P.77.2%')
--pb.name like ('Obsah test')
--c.name like ('%pùjèo%')

--and mv.ActiveFrom = '20100101'
--and mv.ActiveTo = '21000101'

ORDER BY mv.Name, mv.MotiveId, mv.ActiveFrom, mv.ActiveTo , mv.Id, CASE WHEN mv.PrimaryMotivletId = ml.Id THEN 'Primár' ELSE 'Sekundár' END, ml.Id