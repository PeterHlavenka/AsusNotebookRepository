select * from media.Motivlet let
join media.MotiveVersionToMotivlet mtm on let.Id = mtm.MotivletId
join media.MotiveVersion ver on mtm.MotiveVersionId = ver.Id
where ver.Id = 742488

select * from media.Motivlet let
join media.MotiveVersionToMotivlet mtm on let.Id = mtm.MotivletId
join media.MotiveVersion ver on mtm.MotiveVersionId = ver.Id
where ver.Id = 275985

select mot.Id motId, ver.Id verId, let.Id letId, own.Name owner, probra.Name proBraName, * from media.Motivlet let
join media.MotiveVersionToMotivlet mtm on let.Id = mtm.MotivletId
join media.MotiveVersion ver on mtm.MotiveVersionId = ver.Id
join media.Motive mot on ver.MotiveId = mot.Id
join media.Owner own on let.OwnerId = own.Id
join media.ProductBrand probra on let.ProductBrandId = probra.Id
where mot.Id = 13

