
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

BEGIN TRANSACTION addPermissionsTransaction;

BEGIN TRY


DECLARE @ApplicationId INT, @PermissionId INT;
DECLARE @securables TABLE(idx INT PRIMARY KEY IDENTITY (1, 1), securableId INT NOT NULL );
DECLARE @developerId INT;
DECLARE @numrows INT, @i INT, @secId INT;
SET @i = 1
SET @developerId = (SELECT TOP (1) Id FROM [Membership].[User] WHERE UserName LIKE '%slunakova%')         -- nastavit spravny userName
SET @ApplicationId = (SELECT TOP (1) Id FROM Membership.Application WHERE ApplicationName = 'MIR.SimLog')
SET @PermissionId = (SELECT TOP (1) Id FROM Membership.Permission WHERE Code = 'Execute')

INSERT INTO @securables
(
    securableId
)
SELECT Id FROM Membership.Securable s WHERE s.Description LIKE 'SimLog%'

SET @numrows = (SELECT COUNT(*) FROM @securables)

    WHILE (@i <= @numrows)
    BEGIN
        SET @secId = (SELECT TOP(1) securableId FROM @securables WHERE idx = @i)				

		INSERT INTO Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
		VALUES (@developerId, @PermissionId, @secId, @ApplicationId, GETDATE(), '2100-1-1')

        SET @i = @i + 1
    END

		COMMIT TRANSACTION updateSchemaTransaction;
	
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION addPermissionsTransaction;

	DECLARE @ErrorMessage NVARCHAR (4000), @ErrorSeverity INT, @ErrorState INT;
	SELECT @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH


