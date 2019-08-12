

-- ukaze Part a jeho regiony


SELECT dbo.Part.*, pr.*, pn.Name AS PartName
  FROM [PrintStorageAuto].[dbo].[Part] 
  left JOIN dbo.PartRegion pr ON dbo.Part.Id = pr.PartId
  JOIN dbo.PartName pn ON dbo.Part.NameId = pn.Id
  WHERE dbo.Part.Id = (SELECT max(Id) FROM dbo.Part p)