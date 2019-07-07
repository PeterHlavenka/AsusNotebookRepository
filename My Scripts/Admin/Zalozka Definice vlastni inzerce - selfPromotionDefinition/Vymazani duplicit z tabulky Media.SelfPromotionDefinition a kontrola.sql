	;   
with ##SelfPromotionDefinitionCTE AS              
(
	Select *, ROW_NUMBER () Over(Partition by Media.SelfPromotionDefinition.OwnerId, Media.SelfPromotionDefinition.MediumId, Media.SelfPromotionDefinition.ActiveFrom, Media.SelfPromotionDefinition.ActiveTo order by Media.SelfPromotionDefinition.Created) as RowNumber
	from  [Media].[SelfPromotionDefinition]
)
SELECT * FROM ##SelfPromotionDefinitionCTE WHERE ##SelfPromotionDefinitionCTE.RowNumber > 1

--Delete from ##SelfPromotionDefinitionCTE where RowNumber > 1

SELECT * FROM [Media].[SelfPromotionDefinition] WHERE Id IN (
1622,
1564,
1558,
1537,
1559,
1551,
1565,
1567,
1574,
1573,
1571,
1576,
1351,
1352,
1560)

SELECT * FROM [Media].[SelfPromotionDefinition] spd WHERE spd.OwnerId = 23353 AND spd.MediumId = 88 