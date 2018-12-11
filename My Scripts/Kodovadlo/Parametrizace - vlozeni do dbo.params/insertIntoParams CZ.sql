/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Name]
      ,[Value]
      ,[ActiveFrom]
      ,[ActiveTo]
      ,[Description]
  FROM [MediaData3Auto].[dbo].[Params] WHERE name LIKE '%oncrete%'

  select * from [Media].[AdvertisementType]

  INSERT INTO [MediaData3Auto].[dbo].[Params] ([Name], [Value], [ActiveFrom], [ActiveTo], [Description])
  VALUES ('PMConcreteAdvTypeDecisionTypes', '10,13,14', '2010-01-01 00:00:00.000', '2100-01-01 00:00:00.000', 'Kodovadlo, PictureMatchingInstaller, komponenta messageHasConcretePressCategoryFilter - nastaveni Predicate, zrusena zavislost na Enumu AdvertisementType.Values')

