/* 
    Author:		 Peter Hlavenka
    Created:	 10.12.2018
    Description:  50239 Zachytavadlo - vycet useru VM pres params
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 168
	 
	if exists(select top 1 1 from [dbo].[DbSchemaVersion] where [DbSchemaVersion] = @newDbSchemaVersion)
		raiserror('Toto cislo verze bylo jiz nasazeno.', 11, 11 )
		--select * from  [dbo].[DbSchemaVersion] order by [DbSchemaVersion] desc

	declare @Location varchar(max);
	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
	if (@Location is null)
		raiserror('@Location is unknown.', 11, 11 )

	if (@Location = 'CZ')
	begin
	  INSERT INTO [dbo].[Params] (dbo.Params.Name, dbo.Params.[Value], dbo.Params.ActiveFrom, dbo.Params.ActiveTo, dbo.Params.Description) 
	  VALUES ('VideoMatchingUserIds', '3, 107, 108, 110, 111, 112, 117, 118, 142, 242, 243', '2000-01-01 00:00:00.000', '2100-01-01 00:00:00.000', 'Zachytavadlo, vycet VideoMatching useru - podbarvovani MM v gridu (byte[])')
	end

		if (@Location = 'BG')
	begin
	  INSERT INTO [dbo].[Params] (dbo.Params.Name, dbo.Params.[Value], dbo.Params.ActiveFrom, dbo.Params.ActiveTo, dbo.Params.Description) 
	  VALUES ('VideoMatchingUserIds', '3, 4, 5, 37, 39, 140, 141, 142, 143, 145, 146', '2000-01-01 00:00:00.000', '2100-01-01 00:00:00.000', 'Zachytavadlo, vycet VideoMatching useru - podbarvovani MM v gridu (byte[])')
	end


	

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
