/* 
    Author:		 Peter Hlavenka
    Created:	 25.4.2018
    Description: #42957 Vytvoreni Membership.Securable pro zalozku Codebook Administration a správu Advertisement typù (podzáložka Codebook administration) ,
	Nastaveni prav
	
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 138
	 
	declare @Location varchar(max);
	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
	if (@Location is null)
		raiserror('@Location is unknown.', 11, 11 )

	
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
begin tran

declare @ErrorSave int, @ModifiedBy varchar(1000), @Modified datetime, @CreatedBy varchar(1000), @Created datetime;
set @ModifiedBy =   (select UserName from[Security].[User] where Id = Security.GetUserId())
set @Modified = getdate()
set @CreatedBy =   (select UserName from[Security].[User] where Id = Security.GetUserId())
set @created = getdate()

insert into Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
values ('AdvertisementTypeAdministrationViewModel', 'Spravovadlo - Správa Advertisement typù (podzáložka, vyžaduje oprávnìní na CodeBookAdministrationViewModel)', @Created, @CreatedBy, @Modified, @ModifiedBy)

declare @SecurableId int;
select @SecurableId =  SCOPE_IDENTITY() 

declare @PermissionId int;
set @PermissionId = (select Id from Membership.Permission where Code = 'Manage/execute')

declare @ApplicationId int;
set @ApplicationId = (select Id from Membership.Application where ApplicationName = 'MIR.Media.Admin')

declare @developerId int;
set @developerId = (select Id FROM [Membership].[User] where UserName like '%phlavenka%')

declare @managerId int;
set @managerId = (select Id FROM [Membership].[User] where UserName like 'MEDIARESEARCH\miroslav')

declare @verifierId int;
set @verifierId = (select Id FROM [Membership].[User] where UserName like 'mediaresearch\ivak')

insert into Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) values (@SecurableId, @PermissionId, @ApplicationId)

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@developerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@managerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@verifierId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')



insert into Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
values ('CodebookAdministrationViewModel', 'Spravovadlo - záložka Codebook administration', @Created, @CreatedBy, @Modified, @ModifiedBy)

select @SecurableId =  SCOPE_IDENTITY() 

insert into Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) values (@SecurableId, @PermissionId, @ApplicationId)

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@developerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@managerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@verifierId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

update Membership.Securable set Description = 'Spravovadlo - Správa Media typù (podzáložka, vyžaduje oprávnìní na CodebookAdministrationViewModel)' where Code = 'MediaTypeAdministrationViewModel'

commit tran