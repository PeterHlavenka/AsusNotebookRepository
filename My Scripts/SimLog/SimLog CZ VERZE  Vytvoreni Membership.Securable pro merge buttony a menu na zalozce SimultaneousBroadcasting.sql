/* 
    Author:		 Peter Hlavenka
    Created:	 26.4.2018
    Description: #42970 Vytvoreni Membership.Securable pro merge buttony a kontextove menu na zalozce SimultaneousBroadcasting.  CZ VERZE - TADY JESTE NEBYLA SKUPINA SIM LOG
	
*/
set transaction isolation level serializable;

begin tran

declare @ErrorSave int, @ModifiedBy varchar(1000), @Modified datetime, @CreatedBy varchar(1000), @Created datetime;
set @ModifiedBy =   NULL
set @Modified = NULL
set @CreatedBy =   (select UserName from[Security].[User] where Id = Security.GetUserId())
set @created = getdate()

-- SIM LOG MEZI APLIKACEMI NA CZ JESTE NEEXISTUJE
insert into Membership.Application (ApplicationName, Description, Created, CreatedBy, Modified, ModifiedBy)
values ('MIR.SimLog', 'SimLog', @Created, @CreatedBy, null, null)

-- Scope identity muzu pouzit jen v pripade, ze jsem do nejake tabulky insertoval. Pak mi vrati nejvyssi Id teto (naposledy pouzite) tabulky (pokud ma ta tabulka identitu). Tj. insertnul jsem Pricing a Scope_identity mi vrati Id pricingu. SimLog uz ale existuje a proto musim ziskat Id jinak
declare @ApplicationId int;
select @ApplicationId =  SCOPE_IDENTITY() 

-- VYTVORIM SKUPINU SIM LOG ADVANCED
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
(    
    'MIR.Media.SimLog ADVANCED', -- GroupName - varchar
     NEWID(), -- Identifier - uniqueidentifier
    @Created, -- Created - datetime
    @CreatedBy, -- CreatedBy - nvarchar
    NULL, -- Modified - datetime
    NULL, -- ModifiedBy - nvarchar
    NULL, -- SecurableId - int
    'MIR.Media.SimLog ADVANCED', -- DisplayedName - varchar
    NULL, -- Email - nvarchar
    NULL -- Note - nvarchar
)

DECLARE @UserGroupId int;
SELECT @UserGroupId = SCOPE_IDENTITY()

-- VYTVORIM SECURABLE
insert into Membership.Securable (Code, Description, Created, CreatedBy, Modified, ModifiedBy) 
values ('MergeProgram', 'SimLog - Merge buttony a kontextove menu na zalozce SimultaneousBroadcasting', @Created, @CreatedBy, @Modified, @ModifiedBy)

declare @SecurableId int;
select @SecurableId =  SCOPE_IDENTITY()  

-- INSERTNU SECURABLE TO PERMISSION
declare @PermissionId int;
set @PermissionId = (select Id from Membership.Permission where Code = 'Execute')

insert into Membership.SecurableToPermission (SecurableId, PermissionId, ApplicationId) values (@SecurableId, @PermissionId, @ApplicationId)

-- INSERTNU USER GROUP PERMISSION - NUTNE KVULI VALIDACI CASU V SIM LOGU
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

commit tran