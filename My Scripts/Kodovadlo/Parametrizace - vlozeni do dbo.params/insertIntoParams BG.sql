  SELECT TOP (1000) [Id]
      ,[Name]
      ,[Value]
      ,[ActiveFrom]
      ,[ActiveTo]
      ,[Description]
  FROM [MediaData3BG].[dbo].[Params]
  
  
  INSERT INTO [MediaData3BGAuto].[dbo].[Params] ([Name], [Value], [ActiveFrom], [ActiveTo], [Description])
  VALUES ('PMConcreteAdvTypeDecisionTypes', '', '2010-01-01 00:00:00.000', '2100-01-01 00:00:00.000', 'Kodovadlo, PictureMatchingInstaller, komponenta messageHasConcretePressCategoryFilter - nastaveni Predicate, zrusena zavislost na Enumu AdvertisementType.Values')

  UPDATE [MediaData3BGAuto].[dbo].[Params] SET Value = '0' WHERE [MediaData3BGAuto].[dbo].[Params].Name = 'PMConcreteAdvTypeDecisionTypes'