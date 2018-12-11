select * from media.Owner where Name = 'VOLVO CONSTRUCTION EQUIPMENT'

select mes.Id MessageId, cre.Id CreativeId, cre.Transcription, mot.Id MotiveId, ver.Id VersionId, let.Id motivletId, own.Name from media.MediaMessage mes
join creative.creative cre on mes.creativeId = cre.Id
join media.motive mot on cre.MotiveId = mot.Id
join media.MotiveVersion ver on mot.Id = ver.MotiveId
join media.MotiveVersionToMotivlet mtm on ver.Id = mtm.MotiveVersionId
join media.Motivlet let on mtm.motivletId = let.Id
join media.Owner own on let.OwnerId = own.Id
where mes.Id = 7208062


-- Transkripce je jen na bulharsku