-- Televize
SELECT TOP (5) [Id]
      ,[MediaTypeId],
	  [Media].[MediaMessage].NormCreativeId
  FROM [Media].[MediaMessage] WHERE Media.MediaMessage.MediaTypeId = 2 AND Media.MediaMessage.AdvertisedFrom > '20190701' ORDER BY Media.MediaMessage.Created DESC --  ORDER BY MediaMessage.NormCreativeId DESC

  --Tisk mediatyp 1
SELECT TOP (5) [Id]
      ,[MediaTypeId],
	  [Media].[MediaMessage].NormCreativeId
  FROM [Media].[MediaMessage] WHERE Media.MediaMessage.MediaTypeId = 1 AND Media.MediaMessage.AdvertisedFrom > '20181201' ORDER BY Media.MediaMessage.Created DESC

  --Radio mediatyp 4
SELECT TOP (5) [Id]
      ,[MediaTypeId],
	  [Media].[MediaMessage].NormCreativeId
  FROM [Media].[MediaMessage] WHERE Media.MediaMessage.MediaTypeId = 4 AND Media.MediaMessage.AdvertisedFrom > '20181201' ORDER BY Media.MediaMessage.Created DESC

-- televizni MM MediaMessageDao    List<AlliedMessage> GetAlliedMessages(int normCreativeId);
-- Timhle se nacitaji MM do zalozky video, odkomentovanim radku dole se daji najit MM pro ktere plati ze TapeCode se nerovnaji a Normovadlo tyto radky zvyrazni
exec sp_executesql N'SELECT top (100) mm.[Id], 
				tmm.[VideoSimilarityInPercent],
				tmm.[AudioSimilarityInPercent],
				mm.[AdvertisedFrom],
				mm.[AdvertisedTo], 
				mm.Created,
				tmm.Footage,
				ti.TapeLength,
				mdv.Name AS MediumName,
				at.Name AS AdvertisementTypeName,
				c.MotiveId,
				mo.Name AS MotiveName,
				o.Name AS OwnerName,
				cb.Name AS CompanyBrandName,
				pb.Name AS ProductBrandName,
				cp.Name AS CodingPlausibilityName,
				mm.CodingPlausibilityId,
				u.UserName AS CreatedByUserName,
				u2.UserName AS ModifiedByUserName,
				mm.PriceValue AS Price,
				mm.Modified,
				mm.Note,
				ti.TapeCode,
				ti.TapeName,
				mm.NormCreativeId,
				pbl.BlockCode,
				pbl.BlockRating,
				tmm.Rating,
				p.Name AS [PlacementName],
				p.Id AS [PlacementId],
				mm.MediumId,
			    tvm.TvStorageChannelId,
				m.OriginalId AS OriginalMediumId,
                tii.TapeCode AS TvLogTapeCode
			FROM Media.MediaMessage mm 
			  inner join Media.TvMediaMessage tmm on mm.Id = tmm.Id
			  inner join Media.Medium m on mm.MediumId = m.Id
			  inner join Media.TvMedium	tvm on m.Id = tvm.Id
			  inner join Media.MediumVersion mdv on m.Id = mdv.MediumId and mdv.ActiveFrom <= mm.AdvertisedFrom and mdv.ActiveTo > mm.AdvertisedFrom
			  inner join Creative.Creative c ON mm.CreativeId = c.Id
			  left join Media.AdvertisementType at on mm.AdvertisementTypeId = at.Id			  
			  left join media.TapeInfo ti ON tmm.TapeInfoId = ti.Id
			  left join media.ProgrammeBlock pbl ON tmm.ProgrammeBlockId = pbl.id
			  left join Media.MotiveVersion mo on c.MotiveId = mo.MotiveId AND mo.ActiveFrom <= mm.AdvertisedFrom AND mo.ActiveTo > mm.AdvertisedFrom
			  left join Media.Motivlet ml on mo.PrimaryMotivletId = ml.Id
			  left join Media.[Owner] o on ml.OwnerId = o.Id
			  left join Media.CompanyBrand cb on ml.CompanyBrandId = cb.Id
			  left join Media.ProductBrand pb on ml.ProductBrandId = pb.Id
			  left join Media.Placement p on mm.PlacementId = p.Id
			  inner join [Security].[User] u on mm.CreatedBy = u.Id
			  left join [Security].[User] u2 on mm.ModifiedBy = u2.Id
			  inner join Media.CodingPlausibility cp on mm.CodingPlausibilityId = cp.Id
              LEFT JOIN Import.TvImportItem tii ON mm.Id = tii.MediaMessageId
--where tii.TapeCode != ti.TapeCode  NAJDE MM PRO KTERE PLATI, ZE TAPECODY SE NESHODUJU A NORMOVADLO TYTO RADKY PODBARVI 
			  where mm.[NormCreativeId] = @normCreativeId
			order by mm.[AdvertisedFrom]',N'@normCreativeId int',@normCreativeId=15757438