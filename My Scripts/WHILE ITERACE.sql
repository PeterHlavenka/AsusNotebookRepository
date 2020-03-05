

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
SELECT Id FROM Membership.Securable s WHERE s.Description LIKE 'SimLog%'



-- DO PROMENNE @numrows SI PRIRADIM POCET RADKU Z TABULKY
SET @numrows = (SELECT COUNT(*) FROM @securables)



-- PROITERUJU
    WHILE (@i <= @numrows)		 -- DOKUD   @i JE MENSI NEZ POCET RADKU V TABULCE
    BEGIN
        SET @secId = (SELECT TOP(1) securableId FROM @securables WHERE idx = @i)		-- V KAZDE ITERACI SETNI @secId
	
		SELECT @secId		-- VYPIS, PRIPADNE S NIM NECO PROVED

        SET @i = @i + 1
    END