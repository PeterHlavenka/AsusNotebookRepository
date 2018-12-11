-- RESET YZF 

use MediaData3DT

delete from media.MotiveVersionToMotivlet where MotiveVersionId > 1267882 and MotiveVersionId < 12678900
delete from media.MotiveVersion where Id > 1267882 and  Id < 12678900

update media.MotiveVersion set ActiveTo = ('2100-11-11') where Id = 226026


-- RESET HEEL
use MediaData3Auto

delete from media.MotiveVersionToMotivlet where MotiveVersionId > 1249146
delete from media.MotiveVersion where Id > 1249146

update media.MotiveVersion set ActiveTo = ('2100-11-11') where Id = 462574
update media.MotiveVersion set ActiveTo = ('2100-11-11') where Id = 476311
update media.MotiveVersion set ActiveTo = ('2100-11-11') where Id = 595747
update media.MotiveVersion set ActiveTo = ('2100-11-11') where Id = 1120805

update media.MotiveVersion set PrimaryMotivletId = 631057 where Id = 462574
update media.MotiveVersion set PrimaryMotivletId = 631057 where Id = 476311
update media.MotiveVersion set PrimaryMotivletId = 631057 where Id = 595747
update media.MotiveVersion set PrimaryMotivletId = 631057 where Id = 1120805

select * from media.Motivlet where ProductBrandId = 120230
select * from media.ProductBrand where Name like '- Heel'
select * from media.MotiveVersionToMotivlet where produ = 120230