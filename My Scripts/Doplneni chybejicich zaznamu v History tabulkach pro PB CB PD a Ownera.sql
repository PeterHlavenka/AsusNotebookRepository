
DECLARE @Date datetime
SELECT @Date = GETDATE();

INSERT INTO [History].[ProductDetail] ([HistoryCode], [HistoryDate], [HistoryBy], [Id], [Name], [Created], [CreatedBy], [Modified], [ModifiedBy], [Version])
SELECT'M', @Date, SUSER_NAME(), Pro.Id, Pro.Name, Pro.Created, Pro.CreatedBy, Pro.Modified, Pro.ModifiedBy, cast(Pro.Version AS bigint)
FROM [Media].[ProductDetail] Pro
LEFT JOIN [History].[ProductDetail] His ON Pro.Id = His.Id
WHERE His.Id is null

select * from History.ProductDetail where HistoryDate = '2018-06-27 10:59:00.913'
----------------------
DECLARE @Date datetime
SELECT @Date = GETDATE();

INSERT INTO [History].[ProductBrand] ([HistoryCode], [HistoryDate], [HistoryBy], [Id], [Name], [Created], [CreatedBy], [Modified], [ModifiedBy], [Version])
SELECT'M', @Date, SUSER_NAME(), Bra.Id, Bra.Name, Bra.Created, Bra.CreatedBy, Bra.Modified, Bra.ModifiedBy, cast(Bra.Version AS bigint)
FROM [Media].[ProductBrand] Bra
LEFT JOIN [History].[ProductBrand] His ON Bra.Id = His.Id
WHERE His.Id is null

----------------
DECLARE @Date datetime
SELECT @Date = GETDATE();

INSERT INTO [History].[CompanyBrand] ([HistoryCode], [HistoryDate], [HistoryBy], [Id], [Name], [Created], [CreatedBy], [Modified], [ModifiedBy], [Version])
SELECT'M', @Date, SUSER_NAME(), Com.Id, Com.Name, Com.Created, Com.CreatedBy, Com.Modified, Com.ModifiedBy, cast(Com.Version AS bigint)
FROM [Media].[CompanyBrand] Com
LEFT JOIN [History].[CompanyBrand] His ON Com.Id = His.Id
WHERE His.Id is null


----------
DECLARE @Date datetime
SELECT @Date = GETDATE();

INSERT INTO [History].[Owner] ([HistoryCode], [HistoryDate], [HistoryBy], [Id], [Name], [CRN], [OwnershipId], [Created], [CreatedBy], [Modified], [ModifiedBy], [Version], [CrnTypeId])
SELECT'M', @Date, SUSER_NAME(), Own.Id, Own.Name, Own.CRN, Own.OwnershipId, Own.Created, Own.CreatedBy, Own.Modified, Own.ModifiedBy, cast(Own.Version AS bigint), Own.CrnTypeId
FROM [Media].[Owner] Own
LEFT JOIN [History].[Owner] His ON Own.Id = His.Id
WHERE His.Id is null


