

-- Okno DuplicityHunter v Kodovadle se otevre jen u nekterych video nebo press kreativ. 

-- Najdi mi video duplicity od zadaneho datumu.  Pokud uz je duplicita resolvnuta, bude status jiny nez 0
  SELECT ctci.*, cs.NormCreativeItemId, cs.ComparedCreativeItemId, cs.SimilarityKindId, cs.SimilarityResolvedStatusId FROM Creative.Creative AS cre
  JOIN Creative.CreativeToCreativeItem ctci ON cre.Id = ctci.CreativeId
  JOIN Creative.CreativeItem ci ON ctci.CreativeItemId = ci.Id
  JOIN Creative.CreativeSimilarity cs ON ci.Id = cs.NormCreativeItemId 
  where ci.ContentTypeId IN (7) AND ci.Created > '20191101 00:00:00' AND cs.SimilarityKindId = 3 AND cs.SimilarityResolvedStatusId = 0     


