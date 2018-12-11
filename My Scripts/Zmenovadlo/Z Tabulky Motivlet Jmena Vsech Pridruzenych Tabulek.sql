-- spolecne s verzemi, motivlet se opakuje podle toho na kolika verzich se nachazi --
select 
ver.MotiveId motiveId,
ver.Id versionId,
ver.Name MotiveName,
ver.ActiveFrom ActiveFrom,
ver.ActiveTo ActiveTo,
ver.PrimaryMotivletId PrimarniMotivlet,
let.Id MotivletId,
own.Name Owner,
com.Name CompanyBrand,
probrand.Name ProductBrand,
prodetail.Name ProductDetail,
cat.Name Category,
rol.Name Role,
mar.Name Market,
gen.Name Gender,
pla.Name Platform,
tel.Name Telco

 from media.Motivlet let

join media.MotiveVersionToMotivlet mtm on let.Id = mtm.MotivletId
join media.MotiveVersion ver on mtm.MotiveVersionId = ver.Id
join media.Owner own on let.OwnerId = own.Id
join media.CompanyBrand com on let.CompanyBrandId = com.Id
join media.ProductBrand probrand on let.ProductBrandId = probrand.Id
join media.ProductDetail prodetail on let.ProductDetailId = prodetail.Id
join media.Category cat on let.CategoryId = cat.Id
join media.Telco tel on let.TelcoId = tel.Id
join media.Market mar on let.MarketId = mar.Id
join media.Gender gen on let.GenderId = gen.Id
join media.Platform pla on let.PlatformId = pla.Id
join media.Role rol on let.RoleId = rol.Id
--where Own.Name like 'Media Vision' and com.Name like 'CS LINK' and ver.ActiveFrom = '2018-01-01'
where probrand.Name like '%Blíže neurèeno%' and pla.Name like '%Blíže neurèeno%' and ver.ActiveFrom = '2018-01-01'
----where OwnerId = 5118 and ProductBrandId =50150 and CompanyBrandId = 37744 and CategoryId = 671 and TelcoId = 3 and MarketId = 4 and GenderId =3 and PlatformId = 1 and RoleId = 2
----where OwnerId = 5118
 


-- Bez verzi, jen jednotlive motivlety --
select 
let.Id letId,
own.Name Owner,
com.Name CompanyBrand,
probrand.Name ProductBrand,
prodetail.Name ProductDetail,
cat.Name Category,
rol.Name Role,
mar.Name Market,
gen.Name Gender,
pla.Name Platform, 
tel.Name Telco

 from media.Motivlet let

join media.Owner own on let.OwnerId = own.Id
join media.CompanyBrand com on let.CompanyBrandId = com.Id
join media.ProductBrand probrand on let.ProductBrandId = probrand.Id
join media.ProductDetail prodetail on let.ProductDetailId = prodetail.Id
join media.Category cat on let.CategoryId = cat.Id
join media.Telco tel on let.TelcoId = tel.Id
join media.Market mar on let.MarketId = mar.Id
join media.Gender gen on let.GenderId = gen.Id
join media.Platform pla on let.PlatformId = pla.Id
join media.Role rol on let.RoleId = rol.Id


where Own.Name like 'Media Vision' and com.Name like 'CS LINK' and probrand.Name like 'Blíže neurèeno' and pla.Name = 'Blíže neurèeno'
--where OwnerId = 33297 and ProductBrandId =120230 and CompanyBrandId = 27975 and CategoryId = 1395 and TelcoId = 3 and MarketId = 4 and GenderId =3 and PlatformId = 1 and RoleId = 3 and ProductDetailId = 3570

