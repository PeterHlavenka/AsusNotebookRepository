UPDATE media.mediamessage SET media.mediamessage.Ready = 0 WHERE id IN (8475905, 9873441)

SELECT 
mm.Id MessageId,
mm.AdvertisedFrom, 
mm.CodingPlausibilityId, 
mm.MediumId, 
mm.MediaTypeId, 
mm.Ready,
mv.Name
 FROM media.MediaMessage mm 
 JOIN media.Medium m ON mm.MediumId = m.Id
 JOIN media.MediumVersion mv ON m.Id = mv.MediumId
 WHERE mm.id IN (8475905, 9873441)