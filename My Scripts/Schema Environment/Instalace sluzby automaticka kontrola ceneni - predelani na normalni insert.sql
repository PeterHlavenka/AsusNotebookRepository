/* 
    Author:		 Petr Holubec
    Created:	 20191204
    Description: #51583 - Automatic repricing
				 #51935 - Predelat na normalni insert s promennymi plnenymi pres SCOPE_IDENTITY().  Petr Hlavenka
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 198
	 
	if exists(select top 1 1 from [dbo].[DbSchemaVersion] where [DbSchemaVersion] = @newDbSchemaVersion)
		raiserror('Toto cislo verze bylo jiz nasazeno.', 11, 11 )
		--select * from  [dbo].[DbSchemaVersion] order by [DbSchemaVersion] desc

	declare @Location varchar(max);
	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
	if (@Location is null)
		raiserror('@Location is unknown.', 11, 11 )


DECLARE @ServiceTypeId SMALLINT	
DECLARE @ServiceComponentTypeId SMALLINT
DECLARE @ServiceTypeComponentPermissionId SMALLINT	
DECLARE @ServiceInstanceId SMALLINT	
DECLARE @ServiceComponentInstanceId SMALLINT																		


--------------------------------------------------------------------------------------------------------------
-- SERVICE_TYPE 
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceType																		    		
(
	Description, InstallerTypeFullName
)
VALUES
(	
	'MIR.Pricing.Service', -- Description - varchar(max)
	NULL	-- InstallerTypeFullName - varchar(max)
)
SELECT @ServiceTypeId = SCOPE_IDENTITY()



--------------------------------------------------------------------------------------------------------------
-- SERVICE_COMPONENT_TYPE
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceComponentType
(
	 Description, ComponentRegistratorTypeFullName, ComponentRegistratorTypeFullNameAlternative, DefaultViewModelTypeFullName
)
VALUES
( 
	'Generic value provider - int', -- Description - varchar(1023)
	'Mediaresearch.Framework.Environment.Core.Components.GenericValueComponentRegistrator`1[[System.Int32]], Mediaresearch.Framework.Environment.Core', -- ComponentRegistratorTypeFullName - varchar(max)
	NULL, -- ComponentRegistratorTypeFullNameAlternative - varchar(max)
	NULL	-- DefaultViewModelTypeFullName - varchar(max)
)
SELECT @ServiceComponentTypeId = SCOPE_IDENTITY()



--------------------------------------------------------------------------------------------------------------
-- SERVICE_INSTANCE
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceInstance
(
	ServiceTypeId, IpAddress, Hostname, Port, Description, LastActivityUtc, RelativePerformance
)
VALUES
(	
	@ServiceTypeId,		   -- ServiceTypeId - smallint
	'192.168.0.120',		   -- IpAddress - varchar(15)
	'adintel-test-cz',		   -- Hostname - varchar(255)
	0,		   -- Port - int
	'MIR.Pricing.Service',		   -- Description - varchar(max)
	'20000101', -- LastActivityUtc - datetime
	0		   -- RelativePerformance - int
)
SELECT @ServiceInstanceId = SCOPE_IDENTITY()



--------------------------------------------------------------------------------------------------------------
-- SERVICE_COMPONENT_INSTANCE,  SERVICE_TYPE_COMPONENT_PERMISSION, SERVICE_INSTANCE_COMPONENT_REGISTRATION (1)
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceComponentInstance
(
	Value01, Value02, Value03, Value04, Value05
)
VALUES
(	
	'2000', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
)
SELECT @ServiceComponentInstanceId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceTypeComponentPermission
(
	ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	
	@ServiceTypeId,	 -- ServiceTypeId - smallint
	@ServiceComponentTypeId,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobBatch',	 -- ComponentName - varchar(255)
	'RepricingJob - Press - Batch',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
	)
SELECT @ServiceTypeComponentPermissionId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	@ServiceTypeComponentPermissionId, -- PermissionId - int
	@ServiceInstanceId, -- ServiceInstanceId - smallint
	@ServiceComponentInstanceId  -- ServiceComponentInstanceId - int
)



--------------------------------------------------------------------------------------------------------------
-- SERVICE_COMPONENT_INSTANCE,  SERVICE_TYPE_COMPONENT_PERMISSION, SERVICE_INSTANCE_COMPONENT_REGISTRATION (2)
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceComponentInstance
(
	Value01, Value02, Value03, Value04, Value05
)
VALUES
(	
	'60000', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
)
SELECT @ServiceComponentInstanceId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceTypeComponentPermission
(
	ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	
	@ServiceTypeId,	 -- ServiceTypeId - smallint
	@ServiceComponentTypeId,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobPeriodMs',	 -- ComponentName - varchar(255)
	'RepricingJobPeriodMs - Press',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
)
SELECT @ServiceTypeComponentPermissionId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	@ServiceTypeComponentPermissionId, -- PermissionId - int
	@ServiceInstanceId, -- ServiceInstanceId - smallint
	@ServiceComponentInstanceId  -- ServiceComponentInstanceId - int
)



--------------------------------------------------------------------------------------------------------------
-- SERVICE_COMPONENT_INSTANCE,  SERVICE_TYPE_COMPONENT_PERMISSION, SERVICE_INSTANCE_COMPONENT_REGISTRATION (3)		
--------------------------------------------------------------------------------------------------------------								
INSERT INTO Environment.ServiceComponentInstance
(
	Value01, Value02, Value03, Value04, Value05
)
VALUES
(	
	'D392988D-869D-43A9-B252-BD0F0FCD8BF9', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
)
SELECT @ServiceComponentInstanceId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceTypeComponentPermission
(
	ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	
	@ServiceTypeId,	 -- ServiceTypeId - smallint
	@ServiceComponentTypeId,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobPriceListGuidString',	 -- ComponentName - varchar(255)
	'RepricingJob - Press - Pricelist guid',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
)
SELECT @ServiceTypeComponentPermissionId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	@ServiceTypeComponentPermissionId, -- PermissionId - int
	@ServiceInstanceId, -- ServiceInstanceId - smallint
	@ServiceComponentInstanceId  -- ServiceComponentInstanceId - int
)



--------------------------------------------------------------------------------------------------------------
-- SERVICE_COMPONENT_INSTANCE,  SERVICE_TYPE_COMPONENT_PERMISSION, SERVICE_INSTANCE_COMPONENT_REGISTRATION (4)
--------------------------------------------------------------------------------------------------------------
INSERT INTO Environment.ServiceComponentInstance
(
	Value01, Value02, Value03, Value04, Value05
)
VALUES
(	
	'500', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
)
SELECT @ServiceComponentInstanceId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceTypeComponentPermission
(
	ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	
	@ServiceTypeId,	 -- ServiceTypeId - smallint
	@ServiceComponentTypeId,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint    
	'pressRepricingJobStartMs',	 -- ComponentName - varchar(255)
	'RepricingJobStartMs - Press',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
)
SELECT @ServiceTypeComponentPermissionId = SCOPE_IDENTITY()

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	@ServiceTypeComponentPermissionId, -- PermissionId - int
	@ServiceInstanceId, -- ServiceInstanceId - smallint
	@ServiceComponentInstanceId  -- ServiceComponentInstanceId - int
)

---COMMIT




CREATE TABLE Pricing.MessageRepricingStatus
(
	Id TINYINT NOT NULL,
	[Name] VARCHAR(4096) NOT NULL,
	[Description] NVARCHAR(MAX) NULL
)

ALTER TABLE Pricing.MessageRepricingStatus
ADD CONSTRAINT PK_MessageRepricingStatus PRIMARY KEY (Id)

CREATE TABLE Pricing.MessageRepricing
(
	Id BIGINT NOT NULL IDENTITY(1, 1),
	MediaMessageId INT NOT NULL,
	StatusId TINYINT NOT NULL,
	Created DATETIME NOT NULL,
	Modified DATETIME NULL,
	OriginalPrice MONEY NULL,
	NewPrice MONEY NULL,
	[Description] NVARCHAR(max) NULL
)

ALTER TABLE Pricing.MessageRepricing
ADD CONSTRAINT FK_MessageRepricing_StatusId FOREIGN KEY (StatusId) REFERENCES Pricing.MessageRepricingStatus(Id)

ALTER TABLE Pricing.MessageRepricing
ADD CONSTRAINT PK_MessageRepricing PRIMARY KEY (Id)

CREATE NONCLUSTERED INDEX IX_StatusId ON Pricing.MessageRepricing(StatusId)
	
INSERT INTO Pricing.MessageRepricingStatus
(
	Id, Name, Description
)
VALUES
(	1,	-- Id - tinyint
	'New', -- Name - varchar(4096)
	NULL -- Description - nvarchar(max)
	)

INSERT INTO Pricing.MessageRepricingStatus
(
	Id, Name, Description
)
VALUES
(	2,	-- Id - tinyint
	'Done', -- Name - varchar(4096)
	NULL -- Description - nvarchar(max)
	)

INSERT INTO Pricing.MessageRepricingStatus
(
	Id, Name, Description
)
VALUES
(	3,	-- Id - tinyint
	'Cancelled', -- Name - varchar(4096)
	NULL -- Description - nvarchar(max)
	)

INSERT INTO Pricing.MessageRepricingStatus
(
	Id, Name, Description
)
VALUES
(	4,	-- Id - tinyint
	'Error', -- Name - varchar(4096)
	NULL -- Description - nvarchar(max)
	)

INSERT INTO Pricing.MessageRepricingStatus
(
	Id, Name, Description
)
VALUES
(	5,	-- Id - tinyint
	'Reported', -- Name - varchar(4096)
	NULL -- Description - nvarchar(max)
	)

GRANT SELECT, INSERT, DELETE, UPDATE ON Pricing.MessageRepricing TO MediaDataBasicAccess
GRANT SELECT ON Pricing.MessageRepricingStatus TO MediaDataBasicAccess

	
	/*
		Nastavení aktuální verze schématu
	 */	
	insert into dbo.[DbSchemaVersion] ( [DbSchemaVersion], [Created], [Modified])
		values (@newDbSchemaVersion, getdate(), null)
	
	commit transaction updateSchemaTransaction;
	
end try
begin catch
	rollback transaction updateSchemaTransaction;

	declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
	select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
end catch

GO

/*
	Data	
*/
