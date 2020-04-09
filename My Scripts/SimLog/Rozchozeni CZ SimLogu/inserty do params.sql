


INSERT INTO dbo.Params
(
    Name,
    Value,
    ActiveFrom,
    ActiveTo,
    Description
)
VALUES
(   'CommercialPrgTypeIds',        -- Name - varchar(255)
    '101',        -- Value - varchar(max)   // BG = 1
    '2010-01-01 00:00:00.000', -- ActiveFrom - datetime
    '2100-01-01 00:00:00.000', -- ActiveTo - datetime
    N'SimLog - identifikace reklamnich prgTypu, pouzito TestMasterFilterParameters a v CommercialBlockLengthValidator'        -- Description - nvarchar(max)
    )

	SELECT TOP (100) * FROM dbo.Params AS p 
	
	
	INSERT INTO dbo.Params
(
    Name,
    Value,
    ActiveFrom,
    ActiveTo,
    Description
)
VALUES
(   'CommercialAttributeIds',        -- Name - varchar(255)
    '2',        -- Value - varchar(max)    // BG = 1
    '2010-01-01 00:00:00.000', -- ActiveFrom - datetime
    '2100-01-01 00:00:00.000', -- ActiveTo - datetime
    N'SimLog - identifikace reklamnich atributu, pouzito v CommercialBlockLengthValidator'        -- Description - nvarchar(max)
    ) 

UPDATE dbo.Params SET Value = 82550 WHERE name = 'NewsBlockProgrammeIds'

	SELECT TOP (100) * FROM SimLog.Attribute AS a

	SELECT TOP (100) * FROM dbo.Params AS p WHERE p.Name = 'NewsBlockProgrammeIds'

	SELECT TOP (1000) pt.local, p.* FROM SimLog.Programme AS p	
	JOIN SimLog.PrgType AS pt ON pt.Id = p.PrgTypeId
	 WHERE name LIKE '%sportovní noviny%'