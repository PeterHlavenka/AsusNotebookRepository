
-- Vystrihovadlo - po vystrizeni servisni akce SavePressMediaMessageServiceAction ulozi Creative, CreativeItem a PressMediaMessage. 

-- Posledni kreativa a item:
  SELECT cre.* , ci.*
  FROM [MediaData3BGAuto].[Creative].[Creative] cre 
  JOIN Creative.CreativeToCreativeItem ctci ON cre.Id = ctci.CreativeId
  JOIN creative.CreativeItem ci ON ctci.CreativeItemId = ci.Id
  WHERE cre.Id = ( SELECT Max(Id ) FROM creative.Creative)

 SELECT * FROM Creative.CreativeToCreativeItem ctci WHERE ctci.CreativeId = 473192

 SELECT Max(Id ) FROM creative.Creative c
 SELECT Max(Id ) FROM Media.PressMediaMessage pmm

 -- Posledni PressMediaMessage:
 SELECT * FROM Media.PressMediaMessage pmm WHERE pmm.Id = (SELECT Max(Id) FROM Media.PressMediaMessage pmm2)