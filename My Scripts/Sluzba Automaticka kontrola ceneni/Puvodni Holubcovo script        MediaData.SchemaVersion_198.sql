/* 
    Author:		 Petr Holubec
    Created:	 20191204
    Description: #51583 - Automatic repricing
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

SET IDENTITY_INSERT Environment.ServiceType ON 
INSERT INTO Environment.ServiceType
(
	Id, Description, InstallerTypeFullName
)
VALUES
(	2,
	'MIR.Pricing.Service', -- Description - varchar(max)
	NULL	-- InstallerTypeFullName - varchar(max)
	)

SET IDENTITY_INSERT Environment.ServiceType OFF

SET IDENTITY_INSERT Environment.ServiceComponentType ON
INSERT INTO Environment.ServiceComponentType
(
	Id, Description, ComponentRegistratorTypeFullName, ComponentRegistratorTypeFullNameAlternative, DefaultViewModelTypeFullName
)
VALUES
( 13,	
'Generic value provider - int', -- Description - varchar(1023)
	'Mediaresearch.Framework.Environment.Core.Components.GenericValueComponentRegistrator`1[[System.Int32]], Mediaresearch.Framework.Environment.Core', -- ComponentRegistratorTypeFullName - varchar(max)
	NULL, -- ComponentRegistratorTypeFullNameAlternative - varchar(max)
	NULL	-- DefaultViewModelTypeFullName - varchar(max)
	)

SET IDENTITY_INSERT Environment.ServiceComponentType OFF

SET IDENTITY_INSERT Environment.ServiceTypeComponentPermission ON

INSERT INTO Environment.ServiceTypeComponentPermission
(
	Id, ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	19,
	2,	 -- ServiceTypeId - smallint
	13,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobStartMs',	 -- ComponentName - varchar(255)
	'RepricingJobStartMs - Press',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
	)

INSERT INTO Environment.ServiceTypeComponentPermission
(
	Id, ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	20,
	2,	 -- ServiceTypeId - smallint
	13,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobPeriodMs',	 -- ComponentName - varchar(255)
	'RepricingJobPeriodMs - Press',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
	)

INSERT INTO Environment.ServiceTypeComponentPermission
(
	Id, ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	21,
	2,	 -- ServiceTypeId - smallint
	8,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobPriceListGuidString',	 -- ComponentName - varchar(255)
	'RepricingJob - Press - Pricelist guid',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
	)

INSERT INTO Environment.ServiceTypeComponentPermission
(
	Id, ServiceTypeId, ServiceComponentTypeId, RegistrationTargetId, ComponentName, ComponentDescription, IsMandatory
)
VALUES
(	22,
	2,	 -- ServiceTypeId - smallint
	13,	 -- ServiceComponentTypeId - smallint
	2,	 -- RegistrationTargetId - tinyint
	'pressRepricingJobBatch',	 -- ComponentName - varchar(255)
	'RepricingJob - Press - Batch',	 -- ComponentDescription - varchar(max)
	1 -- IsMandatory - bit
	)

SET IDENTITY_INSERT Environment.ServiceTypeComponentPermission OFF

SET IDENTITY_INSERT Environment.ServiceInstance ON
INSERT INTO Environment.ServiceInstance
(
	Id, ServiceTypeId, IpAddress, Hostname, Port, Description, LastActivityUtc, RelativePerformance
)
VALUES
(	12,
	2,		   -- ServiceTypeId - smallint
	'192.168.0.120',		   -- IpAddress - varchar(15)
	'adintel-test-cz',		   -- Hostname - varchar(255)
	0,		   -- Port - int
	'MIR.Pricing.Service',		   -- Description - varchar(max)
	'20000101', -- LastActivityUtc - datetime
	0		   -- RelativePerformance - int
	)
SET IDENTITY_INSERT Environment.ServiceInstance OFF

SET IDENTITY_INSERT Environment.ServiceComponentInstance ON
INSERT INTO Environment.ServiceComponentInstance
(
	Id, Value01, Value02, Value03, Value04, Value05
)
VALUES
(	142,
	'2000', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
	)

		
INSERT INTO Environment.ServiceComponentInstance
(
	Id, Value01, Value02, Value03, Value04, Value05
)
VALUES
(	143,
	'60000', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
	)

INSERT INTO Environment.ServiceComponentInstance
(
	Id, Value01, Value02, Value03, Value04, Value05
)
VALUES
(	144,
	'D392988D-869D-43A9-B252-BD0F0FCD8BF9', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
	)

INSERT INTO Environment.ServiceComponentInstance
(
	Id, Value01, Value02, Value03, Value04, Value05
)
VALUES
(	145,
	'500', -- Value01 - varchar(max)
	NULL, -- Value02 - varchar(max)
	NULL, -- Value03 - varchar(max)
	NULL, -- Value04 - varchar(max)
	NULL	-- Value05 - varchar(max)
	)

SET IDENTITY_INSERT Environment.ServiceComponentInstance OFF

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	19, -- PermissionId - int
	12, -- ServiceInstanceId - smallint
	142  -- ServiceComponentInstanceId - int
	)

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	20, -- PermissionId - int
	12, -- ServiceInstanceId - smallint
	143  -- ServiceComponentInstanceId - int
	)

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	21, -- PermissionId - int
	12, -- ServiceInstanceId - smallint
	144  -- ServiceComponentInstanceId - int
	)

INSERT INTO Environment.ServiceInstanceComponentRegistration
(
	PermissionId, ServiceInstanceId, ServiceComponentInstanceId
)
VALUES
(	22, -- PermissionId - int
	12, -- ServiceInstanceId - smallint
	145  -- ServiceComponentInstanceId - int
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
