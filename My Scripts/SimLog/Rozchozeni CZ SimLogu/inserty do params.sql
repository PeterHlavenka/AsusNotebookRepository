


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



	 -- NAPLNENI TABULKY [AllowedAttributeToPrgTypeCombination] POMOCI ITERACE - PRO KAZDE PRGTYPEID SE VYTVORI ZAZNAM A VSUDE SE PRIRADI ATTRIBUTE 1 - REGULERNI PORAD
SELECT TOP (1000) * FROM [SimLog].[Programme]
SELECT TOP (1000) * FROM [SimLog].[Attribute]
SELECT TOP (1000) * FROM [SimLog].[PrgType]
SELECT TOP (1000) * FROM [SimLog].[AllowedAttributeToPrgTypeCombination]

-- ABYCH MOHL V SQL MIT NECO JAKO List<int> POTREBUJU TABLE
-- DEKLARUJ VSE POTREBNE - TABLE KDE JE IDENTITA A JEDEN SLOUPEC (ID CKA KTERE POTREBUJU )
DECLARE @securables TABLE(idx INT PRIMARY KEY IDENTITY (1, 1), securableId int NOT NULL );
DECLARE @numrows INT, @i INT, @secId INT;
SET @i = 1

-- NAPLNIM TABULKU SELECTEM Z JINE TABULKY
INSERT INTO @securables
(
    securableId
)
SELECT Id FROM Simlog.PrgType AS pt  

-- DO PROMENNE @numrows SI PRIRADIM POCET RADKU Z TABULKY
SET @numrows = (SELECT COUNT(*) FROM @securables)

-- PROITERUJU
    WHILE (@i <= @numrows)		 -- DOKUD   @i JE MENSI NEZ POCET RADKU V TABULCE
    BEGIN
        SET @secId = (SELECT TOP(1) securableId FROM @securables WHERE idx = @i)		-- V KAZDE ITERACI SETNI @secId
	
		--SELECT @secId		-- VYPIS, PRIPADNE S NIM NECO PROVED
		INSERT INTO SimLog.AllowedAttributeToPrgTypeCombination
		(
		    PrgTypeId,
		    AttributeId
		)
		VALUES
		(   @secId, -- PrgTypeId - tinyint
		    1  -- AttributeId - tinyint
		    )

        SET @i = @i + 1
    END
