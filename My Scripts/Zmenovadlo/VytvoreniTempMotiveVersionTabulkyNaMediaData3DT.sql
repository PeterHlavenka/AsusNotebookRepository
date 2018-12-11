
--CREATE TABLE Media.TempMotiveVersion
--                    (
--                        [Id] [int] NOT NULL PRIMARY KEY,
--	                    [MotiveId] [int] NOT NULL,
--	                    [ActiveFrom] [datetime] NOT NULL,
--	                    [ActiveTo] [datetime] NOT NULL,
--	                    [PrimaryMotivletId] [int] NULL,
--	                    [Name] [varchar](500) NOT NULL,
--	                    [Campaign] [varchar](200) NULL,
--	                    [IsProduct] [bit]  NULL,
--	                    [IsSponsorship] [bit]  NULL,
--	                    [IsShow] [bit]  NULL,
--	                    [IsCoupon] [bit]  NULL,
--	                    [IsAnnouncement] [bit]  NULL,
--	                    [IsMedialPartnership] [bit]  NULL,
--	                    [IsGeneralMeeting] [bit]  NULL,
--	                    [NotMatchingSubject] [bit]  NULL,
--	                    [Created] [datetime]  NULL,
--	                    [CreatedBy] [tinyint]  NULL,
--	                    [Modified] [datetime] NULL,
--	                    [ModifiedBy] [tinyint] NULL,	                    
--                    )

		/* 
    Author:		 Peter Hlavenka
    Created:	 24.05.2018
    Description: #41832 Zmenovadlo,  vytvoreni tabulky TempMotiveVersion 
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 143
	 
	if exists(select top 1 1 from [dbo].[DbSchemaVersion] where [DbSchemaVersion] = @newDbSchemaVersion)
		raiserror('Toto cislo verze bylo jiz nasazeno.', 11, 11 )
		--select * from  [dbo].[DbSchemaVersion] order by [DbSchemaVersion] desc

	declare @Location varchar(max);
	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
	if (@Location is null)
		raiserror('@Location is unknown.', 11, 11 )

	--if (@Location = 'CZ')
	--begin
	
	--end

	

	/*
		Create skripty
	*/
	CREATE TABLE Media.TempMotiveVersion
                    (
                        [Id] [int] NOT NULL PRIMARY KEY,
	                    [MotiveId] [int] NOT NULL,
	                    [ActiveFrom] [datetime] NOT NULL,
	                    [ActiveTo] [datetime] NOT NULL,
	                    [PrimaryMotivletId] [int] NULL,
	                    [Name] [varchar](500) NOT NULL,
	                    [Campaign] [varchar](200) NULL,
	                    [IsProduct] [bit]  NULL,
	                    [IsSponsorship] [bit]  NULL,
	                    [IsShow] [bit]  NULL,
	                    [IsCoupon] [bit]  NULL,
	                    [IsAnnouncement] [bit]  NULL,
	                    [IsMedialPartnership] [bit]  NULL,
	                    [IsGeneralMeeting] [bit]  NULL,
	                    [NotMatchingSubject] [bit]  NULL,
	                    [Created] [datetime]  NULL,
	                    [CreatedBy] [tinyint]  NULL,
	                    [Modified] [datetime] NULL,
	                    [ModifiedBy] [tinyint] NULL,	                    
                    )
	/*
		Oprávnìní		
	 */
	grant select, insert, update, delete on Media.TempMotiveVersion to MediaDataBasicAccess;

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
--declare @ErrorSave int, @ModifiedBy int, @Modified datetime;
--select @ModifiedBy = Security.GetUserId(), @Modified = getdate();
