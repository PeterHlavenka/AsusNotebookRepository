CREATE TABLE [dbo].B_Words
(
	[Id] INT NOT NULL PRIMARY KEY identity, 
    [Name] NCHAR(100) NOT NULL, 
    [IsEnumerated] BIT NOT NULL DEFAULT 1, 
    [CoveredName] NCHAR(100) NULL,
)

CREATE TRIGGER [Trigger]
	ON [dbo].[B_Words]
	FOR  INSERT
	AS
	BEGIN
		SET NOCOUNT ON
		DECLARE @coveredName nvarchar(100);

SELECT @coveredName = i.Name FROM INSERTED i
SELECT @coveredName = replace (@coveredName, 'by', 'b_')
SELECT @coveredName = replace (@coveredName, 'bi', 'b_')
SELECT @coveredName = replace (@coveredName, 'bý', 'b_')
SELECT @coveredName = replace (@coveredName, 'bí', 'b_')


  UPDATE dbo.B_Words
  SET dbo.B_Words.CoveredName =  @coveredName
  FROM  Inserted AS i
  WHERE dbo.B_Words.Id = i.Id ;   
	END


