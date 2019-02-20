SELECT TOP 10 mm.Id, mm.NormCreativeId
FROM media.MediaMessage mm WHERE mm.MediaTypeId = 2 ORDER BY mm.Created DESC 

-- Normovadlo tlacitkem ChangeNorm zmeni vybran MM normu, najdeme creativeItemId a koukneme do tabulky VideoCreative jestli jsou VideoData
SELECT * from creative.Creative WHERE Creative.Creative.Id = 15903304
SELECT * FROM creative.CreativeToCreativeItem ctci WHERE ctci.CreativeId = 15903304

SELECT * FROM creative.VideoCreative vc WHERE vc.CreativeItemId = 11035779

SELECT TOP 1 *
FROM Creative.Creative c ORDER BY c.Id desc