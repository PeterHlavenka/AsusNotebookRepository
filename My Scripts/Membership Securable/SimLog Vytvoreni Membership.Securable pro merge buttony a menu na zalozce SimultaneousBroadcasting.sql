/* 
    Author:		 Peter Hlavenka
    Created:	 26.4.2018
    Description: #42970 Vytvoreni Membership.Securable pro merge buttony a kontextove menu na zalozce SimultaneousBroadcasting.
	
*/
set transaction isolation level serializable;

--begin transaction updateSchemaTransaction;
--																				-- DOKUD NEMAME ROUNDHOUSE MUZU DATA JEN INSERTNOUT, NEPOTREBUJU NAVYSENI SCHEMA VERSION
--begin try
--	/*
--		Kontrola aktuální verze schématu databáze
--	 */
--	declare @newDbSchemaVersion int;
--	select @newDbSchemaVersion  = 139
	 
--	declare @Location varchar(max);
--	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
--	if (@Location is null)
--		raiserror('@Location is unknown.', 11, 11 )

	
--	insert into dbo.[DbSchemaVersion] ( [DbSchemaVersion], [Created], [Modified])
--		values (@newDbSchemaVersion, getdate(), null)

--	commit transaction updateSchemaTransaction;
	
--end try
--begin catch
--	rollback transaction updateSchemaTransaction;

--	declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
--	select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
--	raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
--end catch

--GO

/*
	Data	
*/
begin tran

declare @ErrorSave int, @ModifiedBy varchar(1000), @Modified datetime, @CreatedBy varchar(1000), @Created datetime;
set @ModifiedBy =   (select UserName from[Security].[User] where Id = Security.GetUserId())
set @Modified = getdate()
set @CreatedBy =   (select UserName from[Security].[User] where Id = Security.GetUserId())
set @created = getdate()

-- SIM LOG MEZI APLIKACEMI UZ EXISTUJE
--insert into Membership.Application (ApplicationName, Description, Created, CreatedBy, Modified, ModifiedBy)
--values ('MIR.Pricing', 'Ceníkovadlo', @Created, @CreatedBy, @Modified, @ModifiedBy)

-- Scope identity muzu pouzit jen v pripade, ze jsem do nejake tabulky insertoval. Pak mi vrati nejvyssi Id teto (naposledy pouzite) tabulky (pokud ma ta tabulka identitu). Tj. insertnul jsem Pricing a Scope_identity mi vrati Id pricingu. SimLog uz ale existuje a proto musim ziskat Id jinak
--declare @ApplicationId int;
--select @ApplicationId =  SCOPE_IDENTITY() 

declare @ApplicationId int;
select @ApplicationId =  (SELECT Id FROM Membership.Application a WHERE a.ApplicationName = 'MIR.SimLog') 

-- VYTVORIM SECURABLE
insert into Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
values ('MergeProgram', 'SimLog - Merge buttony a kontextove menu na zalozce SimultaneousBroadcasting', @Created, @CreatedBy, @Modified, @ModifiedBy)

declare @SecurableId int;
select @SecurableId =  SCOPE_IDENTITY()  

-- INSERTNU SECURABLE TO PERMISSION
declare @PermissionId int;
set @PermissionId = (select Id from Membership.Permission where Code = 'Execute')

insert into Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) values (@SecurableId, @PermissionId, @ApplicationId)

-- PRIDAM SECURABLE DO SKUPINY SIM LOG ADVANCED
DECLARE @UserGroupId int;
SELECT @UserGroupId = (SELECT Id FROM Membership.UserGroup ug WHERE ug.GroupName = 'MIR.Media.SimLog ADVANCED')

  INSERT INTO Membership.UserGroupPermission
  (
      --Id - this column value is auto-generated
      UserGroupId,
      PermissionId,
      SecurableId,
      ApplicationId,
      ValidFrom,
      ValidTo
  )
  VALUES
  (
      -- Id - int
      @UserGroupId, -- UserGroupId - int
      @PermissionId, -- PermissionId - int
      @SecurableId, -- SecurableId - int
      @ApplicationId, -- ApplicationId - int
      '1900-01-01 00:00:00.000', -- ValidFrom - datetime
      '2100-01-01 00:00:00.000' -- ValidTo - datetime
  )

-- V SIM LOGU MAM PRAVA DIKY TOMU, ZE JSEM VE SKUPINE SIM LOG ADVANCED. PROTO NEMUSIM PRIDAVAT SEBE A DALSI USERY SCRIPTEM.

--declare @developerId int;
--set @developerId = (select Id FROM [Membership].[User] where UserName like '%phlavenka%')
--insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
--values (@developerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')


--declare @managerId int;
--set @managerId = (select Id FROM [Membership].[User] where UserName like 'MEDIARESEARCH\miroslav')
--insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
--values (@managerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')


--declare @verifierId int;
--set @verifierId = (select Id FROM [Membership].[User] where UserName like 'mediaresearch\ivak')
--insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
--values (@verifierId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')


--declare @secondVerifierId int;
--set @secondVerifierId = (select Id FROM [Membership].[User] where UserName like 'mediaresearch\slunakova')
--insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
--values (@secondVerifierId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

commit tran