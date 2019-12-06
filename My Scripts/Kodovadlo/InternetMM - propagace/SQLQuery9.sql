-- V K. si vyberu internet za nejaky mesic, zkopiruju MM.Id od nactene kreativy, vlozim do query a spustim. Motive by mel byt null, plausibilita nezpracovane. Najdu motiv, priradim ho MM a zmacku F2. Pustim query znovu a plausibilita by mela byt sure, a motiv by
-- mel byt ten ktery jsem priradil v K. 

-- propaguji se jen MM v konkretnim mesici
DECLARE @od datetime
DECLARE @do datetime
SELECT @od = '2019-10-01'
SELECT @do = '2019-11-01'


--Dej mi vsechny internetove MM, ktere maji pozadovane campaignId  a k ni mi rekni jeji codingPlausibility a nazev motivu
 SELECT imm.Id InternetAndMediaMessageId, cp.Name Plausibility,  mm.CreativeId, mv.Name MotiveVersionName, mm.AdvertisedFrom, mm.AdvertisedTo FROM Media.InternetMediaMessage imm 
 JOIN Media.MediaMessage mm ON imm.Id = mm.Id
 JOIN Media.CodingPlausibility cp ON mm.CodingPlausibilityId = cp.Id
 JOIN Creative.Creative c ON mm.CreativeId = c.Id
 left JOIN Media.Motive m ON c.MotiveId = m.Id
 left JOIN Media.MotiveVersion mv ON m.Id = mv.MotiveId
 WHERE imm.CampaignId = 
 (
	SELECT imm.CampaignId FROM Media.InternetMediaMessage imm WHERE Id = 19088724 -- Najdi mi cislo kampane podle MM.Id
	AND mm.AdvertisedFrom >= @od 
	AND mm.AdvertisedTo < @do
 )
 ORDER BY mm.Id

 exec Media.Coding_Propagate @MediaMessageId=19088724,@OriginalCodingPlausibilityId=1,@NewCodingPlausibilityId=8,@PriceValue=$0.0000,@SponsoredProgrammeId=NULL,@PlacementId=NULL,@JustReturnCount=0,@DebugLevel=default

 select mm.Id 
							from Media.MediaMessage mm
								inner join [Media].[InternetMediaMessage] [imm] on [imm].[Id] = [mm].[Id]
							WHERE [imm].[CampaignId] = 36091
								AND mm.MediaTypeId = 32
								and mm.CodingPlausibilityId = 1 --#8682 propagovat pouze pokud se neco zmeni
								
								-- Maty: jasne... jedine co me napada je, ze kdyz bych chtel shozenim plausibility "smazat" celou kampan, musim po jedne MM, nebo skriptem...
								-- hm...tak to tam teda nedáme...ooh nemáme v bg, tak se ke všemu chovejme jednotnì jako ke kampani....a v tom není bordel...
								--and (mm.CodingPlausibilityId != @NewCodingPlausibilityId)	--#16487 propagovat i pokud se zmeni jen plausibilita a zaroven nepropagovat, pokud již je vše jiste, pak se zmeni jen motiv na te jedne kodovane/opravovane norme
								
								and mm.AdvertisedFrom >= '2019-10-01 00:00:00.000' and mm.AdvertisedFrom < '2019-11-01 00:00:00.000'	




 --Media.Coding_Propagate

 UPDATE Media.MediaMessage 
 SET Media.MediaMessage.CodingPlausibilityId = 1 WHERE Id IN(19088725,
19088726,
19088727)
    
	select * from Media.CodingPlausibility cp