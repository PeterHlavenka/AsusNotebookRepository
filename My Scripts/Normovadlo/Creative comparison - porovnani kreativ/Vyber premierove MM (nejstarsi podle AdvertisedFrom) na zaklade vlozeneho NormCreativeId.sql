 
 -- Vybere 3 MM od jedne normy podle zadaneho NormCreativeId, seradi podle AdvertisedFrom, pricemz ta prvni je nejstarsi MM. Je to tedy premierova MM
 
 SELECT TOP (3) tvmm.Id, mm.NormCreativeId, tvmm.Footage, mm.AdvertisedFrom FROM Media.TvMediaMessage tvmm
 JOIN media.MediaMessage mm ON tvmm.Id = mm.Id
  WHERE mm.NormCreativeId = 436662 ORDER BY mm.AdvertisedFrom