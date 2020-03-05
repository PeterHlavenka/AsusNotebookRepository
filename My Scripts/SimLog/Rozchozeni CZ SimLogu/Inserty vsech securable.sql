/* 
    Author:		 Peter Hlavenka
    Created:	 4.3.2020
    Description: 52087 - Prava pro CZ SimLog
*/
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

BEGIN TRANSACTION updateSchemaTransaction;

BEGIN TRY
	/*
		Kontrola aktuální verze schématu databáze
	 */
	DECLARE @newDbSchemaVersion INT;
	SELECT @newDbSchemaVersion  = 2087
	 
	IF EXISTS(SELECT TOP 1 1 FROM [dbo].[DbSchemaVersion] WHERE [DbSchemaVersion] = @newDbSchemaVersion)
		RAISERROR('Toto cislo verze bylo jiz nasazeno.', 11, 11 )
		--select * from  [dbo].[DbSchemaVersion] order by [DbSchemaVersion] desc

	DECLARE @Location VARCHAR(MAX);
	SELECT @Location = [Value] FROM [dbo].[Params] WHERE [Name] = 'Location'
	IF (@Location IS NULL)
		RAISERROR('@Location is unknown.', 11, 11 )

-----------------------------------------

DECLARE @CreatedBy VARCHAR(1000), @Created DATETIME;
DECLARE @ApplicationId INT, @SecurableId INT, @PermissionId INT;

SET @CreatedBy =   (SELECT TOP (1) UserName FROM[Security].[User] WHERE Id = Security.GetUserId())
SET @Created = GETDATE()
SET @ApplicationId = (SELECT TOP (1) Id FROM Membership.Application WHERE ApplicationName = 'MIR.SimLog')
SET @PermissionId = (SELECT TOP (1) Id FROM Membership.Permission WHERE Code = 'Execute')

	  IF NOT EXISTS (SELECT * FROM Membership.UserGroup WHERE GroupName = 'MIR.Media.SimLog BASIC')
	  BEGIN
		  INSERT INTO Membership.UserGroup
		  (
			  GroupName,
			  Identifier,
			  Created,
			  CreatedBy,
			  Modified,
			  ModifiedBy,
			  SecurableId,
			  DisplayedName,
			  Email,
			  Note
		  )
		  VALUES
		  (   'MIR.Media.SimLog BASIC',        -- GroupName - varchar(100)
			  NEWID(),      -- Identifier - uniqueidentifier
			  GETDATE(), -- Created - datetime
			  @CreatedBy,       -- CreatedBy - nvarchar(100)
			  NULL, -- Modified - datetime
			  NULL,       -- ModifiedBy - nvarchar(100)
			  NULL,         -- SecurableId - int
			  'MIR.Media.SimLog BASIC',        -- DisplayedName - varchar(100)
			  NULL,       -- Email - nvarchar(100)
			  NULL        -- Note - nvarchar(max)
		  )
	  END

	  IF NOT EXISTS (SELECT * FROM Membership.UserGroup WHERE GroupName = 'MIR.Media.SimLog ADVANCED')
	  BEGIN
				INSERT INTO Membership.UserGroup
		  (
			  GroupName,
			  Identifier,
			  Created,
			  CreatedBy,
			  Modified,
			  ModifiedBy,
			  SecurableId,
			  DisplayedName,
			  Email,
			  Note
		  )
		  VALUES
		  (   'MIR.Media.SimLog ADVANCED',        -- GroupName - varchar(100)
			  NEWID(),      -- Identifier - uniqueidentifier
			  GETDATE(), -- Created - datetime
			  @CreatedBy,       -- CreatedBy - nvarchar(100)
			  NULL, -- Modified - datetime
			  NULL,       -- ModifiedBy - nvarchar(100)
			  NULL,         -- SecurableId - int
			  'MIR.Media.SimLog ADVANCED',        -- DisplayedName - varchar(100)
			  NULL,       -- Email - nvarchar(100)
			  NULL        -- Note - nvarchar(max)
			  )
	  END


--Hints
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('Hints', 'SimLog - Hints tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- SimultaneusBroadcasting
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('SimultaneusBroadcasting', 'SimLog - Simultaneus Broadcasting tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- ProgramChecker
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('ProgramChecker', 'SimLog - Programme Checker tab', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- SpecificChecksTestMaster
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('SpecificChecksTestMaster', 'SimLog - Test Master tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- SpecificChecksNewsBlock
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('SpecificChecksNewsBlock', 'SimLog - News Block tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- SpecificChecksDuplicities
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('SpecificChecksDuplicities', 'SimLog - Duplicities tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- SpecificChecks
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('SpecificChecks', 'SimLog - Specific Checks tab page (parent tab for other pages)', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


---- CatchingSimLog
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('CatchingSimLog', 'SimLog - Catching (main) tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- ViewsAndChecks
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('ViewsAndChecks', 'SimLog - Views and Checks tab page (parent for other pages)', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)


-- EditProgramme
INSERT INTO Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
VALUES ('EditProgramme', 'SimLog - Edit program dialog in ProgramChecker tab page', @Created, @CreatedBy, NULL, NULL)

SELECT @SecurableId =  SCOPE_IDENTITY() 
INSERT INTO Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) VALUES (@SecurableId, @PermissionId, @ApplicationId)

------------------------------
	/*
		Nastavení aktuální verze schématu
	 */	
	--INSERT INTO dbo.[DbSchemaVersion] ( [DbSchemaVersion], [Created], [Modified])
	--	VALUES (@newDbSchemaVersion, GETDATE(), NULL)

	COMMIT TRANSACTION updateSchemaTransaction;
	
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION updateSchemaTransaction;

	DECLARE @ErrorMessage NVARCHAR (4000), @ErrorSeverity INT, @ErrorState INT;
	SELECT @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH

GO

/*
	Data	
*/
--declare @ErrorSave int, @ModifiedBy int, @Modified datetime;
--select @ModifiedBy = Security.GetUserId(), @Modified = getdate();

