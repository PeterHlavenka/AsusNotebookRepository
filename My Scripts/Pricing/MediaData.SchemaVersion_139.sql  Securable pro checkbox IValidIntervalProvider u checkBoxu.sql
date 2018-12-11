/* 
    Author:		 Peter Hlavenka
    Created:	 26.4.2018
    Description: #42970 Vytvoreni Membership.Securable pro checkbox v Pricingu ktery omezuje vybrany datum v datepickeru podle nastaveni v configu.
	
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 139
	 
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

insert into Membership.Application (ApplicationName, Description, Created, CreatedBy, Modified, ModifiedBy)
values ('MIR.Pricing', 'Ceníkovadlo', @Created, @CreatedBy, @Modified, @ModifiedBy)


declare @ApplicationId int;
select @ApplicationId =  SCOPE_IDENTITY() 


insert into Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
values ('IntervalProviderViewModel', 'Ceníkovadlo - Checkbox na omezení vybraného datumu', @Created, @CreatedBy, @Modified, @ModifiedBy)

declare @SecurableId int;
select @SecurableId =  SCOPE_IDENTITY() 

declare @PermissionId int;
set @PermissionId = (select Id from Membership.Permission where Code = 'Manage/execute')


insert into Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) values (@SecurableId, @PermissionId, @ApplicationId)

declare @developerId int;
set @developerId = (select Id FROM [Membership].[User] where UserName like '%phlavenka%')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@developerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

declare @managerId int;
set @managerId = (select Id FROM [Membership].[User] where UserName like 'MEDIARESEARCH\miroslav')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@managerId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

declare @verifierId int;
set @verifierId = (select Id FROM [Membership].[User] where UserName like 'mediaresearch\ivak')

insert into Membership.UserPermission (UserId, PermissionId, SecurableId, ApplicationId, ValidFrom, ValidTo) 
values (@verifierId, @PermissionId, @SecurableId, @ApplicationId, GETDATE(), '2100-1-1')

commit tran