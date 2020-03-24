/* 
    Author:		 Peter Hlavenka
    Created:	 23.3.2020
    Description: #52103 Presunuti CheckedProductInvalidMotives z configu Kodovadla do params
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 2103
	 
	if exists(select top 1 1 from [dbo].[DbSchemaVersion] where [DbSchemaVersion] = @newDbSchemaVersion)
		raiserror('Toto cislo verze bylo jiz nasazeno.', 11, 11 )
		--select * from  [dbo].[DbSchemaVersion] order by [DbSchemaVersion] desc

	declare @Location varchar(max);
	select @Location = [Value] from [dbo].[Params] where [Name] = 'Location'
	if (@Location is null)
		raiserror('@Location is unknown.', 11, 11 )

	DELETE FROM dbo.Params WHERE Name = 'CheckedProductValidMotivesCollection'

	IF (@Location = 'CZ')
	BEGIN
		INSERT INTO dbo.Params
		(
			Name,
			Value,
			ActiveFrom,
			ActiveTo,
			Description
		)
		VALUES
		(   'CheckedProductInvalidMotives',                                                      -- Name - varchar(255)
			'Oznámení,Zaměstnání,Valná hromada,OST,ZAM,ZAM+OST,Malý formát,Cizojazyčná reklama', -- Value - varchar(max)
			'2010-01-01 00:00:00.000',                                                           -- ActiveFrom - datetime
			'2100-01-01 00:00:00.000',                                                           -- ActiveTo - datetime
			N'Speciální motivy, používané v Kódovadle v SavePreventerech'                        -- Description - nvarchar(max)
			)

		INSERT INTO dbo.Params
		(
			Name,
			Value,
			ActiveFrom,
			ActiveTo,
			Description
		)
		VALUES
		(   'NespecProductBrandValidMotives',                                                                                              -- Name - varchar(255)
			'Oznámení,Zaměstnání,Valná hromada,OST,ZAM,ZAM+OST,Malý formát,Cizojazyčná reklama',                                           -- Value - varchar(max)
			'2010-01-01 00:00:00.000',                                                                                                     -- ActiveFrom - datetime
			'2100-01-01 00:00:00.000',                                                                                                     -- ActiveTo - datetime
			N'Motivy, které v Kódovadle jdou uložit i když je v poli Produktu uvedeno Nespecifikováno a zároveň není zaškrtnutý Produkt. ' -- Description - nvarchar(max)
			)
	END

		IF (@Location = 'BG')
	BEGIN
		INSERT INTO dbo.Params
		(
			Name,
			Value,
			ActiveFrom,
			ActiveTo,
			Description
		)
		VALUES
		(   'CheckedProductInvalidMotives',                                                      -- Name - varchar(255)
			'',                                                                                  -- Value - varchar(max)
			'2010-01-01 00:00:00.000',                                                           -- ActiveFrom - datetime
			'2100-01-01 00:00:00.000',                                                           -- ActiveTo - datetime
			N'Speciální motivy, používané v Kódovadle v SavePreventerech'                        -- Description - nvarchar(max)
			)

		INSERT INTO dbo.Params
		(
			Name,
			Value,
			ActiveFrom,
			ActiveTo,
			Description
		)
		VALUES
		(   'NespecProductBrandValidMotives',                                                                                              -- Name - varchar(255)
			'',                                                                                                                            -- Value - varchar(max)
			'2010-01-01 00:00:00.000',                                                                                                     -- ActiveFrom - datetime
			'2100-01-01 00:00:00.000',                                                                                                     -- ActiveTo - datetime
			N'Motivy, které v Kódovadle jdou uložit i když je v poli Produktu uvedeno Nespecifikováno a zároveň není zaškrtnutý Produkt.  '-- Description - nvarchar(max)
			)
	END

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
