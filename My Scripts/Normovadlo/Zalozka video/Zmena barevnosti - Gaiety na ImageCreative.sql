

-- Creative.CreativeItem.Id == Creative.ImageCreative.Id  

-- barevnost je sloupec na ImageCreative:
SELECT TOP (1000) imagecreative.[Id]
      ,[ImageData]
      ,[ImageDataHash]
      ,[Width]
      ,[Height]
      ,[ColorDepth]
      ,[GaietyId]
      ,[GaietyGResp]
      ,[GaietyCResp]
      ,[ImageDataSize]
	  ,g.Name
  FROM [MediaData3Auto].[Creative].[ImageCreative] 
  left JOIN Creative.Gaiety g ON Creative.ImageCreative.GaietyId = g.Id
  WHERE imagecreative.Id = 10979777

  -- pokud mam mmId znam i CreativeId (musi jit o pressMM):
  SELECT pmm.CreativeId FROM Media.MediaMessage pmm WHERE Id = 117277446	

  -- vazebni tabulka, podle CreativeId najdu CreativeItem.Id
  SELECT * FROM [Creative].[CreativeToCreativeItem] WHERE Creative.CreativeToCreativeItem.CreativeId = 16228521


  -- Creative.CreativeItem.Id == Creative.ImageCreative.Id     vlozim tedy CreativeItem.Id. Pokud updatnu GaietyId na NULL, bude zobrazeno Neposuzovano protoze to je v ciselniku nastaveno u cisla 0.
  UPDATE [MediaData3Auto].[Creative].[ImageCreative] SET [Creative].[ImageCreative].GaietyId = null  WHERE [Creative].[ImageCreative].Id = 10979777


-- Gaiety:
--Id	Name	Order
--0	Neposuzováno	0
--1	Černobílý	1
--2	Barevný	3
--4	Jednobarevný	2




  SELECT * FROM Media.MediaMessage pmm WHERE Id = 117277446		

