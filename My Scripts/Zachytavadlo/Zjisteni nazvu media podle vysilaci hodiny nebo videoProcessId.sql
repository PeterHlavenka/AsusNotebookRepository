
-- Na zaklade casu videa najde videoprocess, k nemu skrze tabulku TvMedium priradi medium a pres tabulku MediumVersion zjisti nazev media.

SELECT TOP 100  mv.Name, vps.Name,  *
FROM Creative.VideoProcess vp 
JOIN media.TvMedium tm ON vp.TvStorageChannelId = tm.TvStorageChannelId
JOIN media.MediumVersion mv ON mv.MediumId = tm.Id
JOIN Creative.VideoProcessStatus vps ON vp.VideoProcessStatusId = vps.Id
WHERE vp.VideoDateTime >= '2019-08-02 06:00:00' AND vp.VideoDateTime < '2019-08-02 07:00:00' 
AND mv.Name = 'Nova'

SELECT TOP 100 *
FROM Creative.VideoProcessStatus vps