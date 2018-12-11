/* 
    Author:		 Peter Hlavenka
    Created:	 16.11.2018
    Description: 50127 (TFS) 
*/
set transaction isolation level serializable;

begin transaction updateSchemaTransaction;

begin try
	/*
		Kontrola aktuální verze schématu databáze
	 */
	declare @newDbSchemaVersion int;
	select @newDbSchemaVersion  = 162
	 
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
		Alter skripty
	*/
	IF NOT EXISTS (SELECT * FROM syscolumns WHERE id=object_id('[Import].[TvImportItem]') AND name='IsHidden') 
	ALTER TABLE [Import].[TvImportItem] ADD IsHidden bit not NULL default(0)
	
	IF NOT EXISTS (SELECT * FROM syscolumns WHERE id=object_id('[History].[TvImportItem]') AND name='IsHidden') 
	ALTER TABLE [History].[TvImportItem] ADD IsHidden bit NULL
	
	EXEC('ALTER trigger [Import].[trg_TvImportItem_Delete]
on [Import].[TvImportItem]
for delete
not for replication
as
begin
	insert into [History].[TvImportItem]
	(
		HistoryCode,
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	)
	select
		''D'',
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	from deleted;
end;')
	EXEC('ALTER trigger [Import].[trg_TvImportItem_Insert]
on [Import].[TvImportItem]
for insert
not for replication
as
begin
	insert into [History].[TvImportItem]
	(
		HistoryCode,
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	)
	select
		''I'',
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	from inserted;
end;')
	EXEC('ALTER trigger [Import].[trg_TvImportItem_Update]
on [Import].[TvImportItem]
for update
not for replication
as
begin
	insert into [History].[TvImportItem]
	(
		HistoryCode,
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	)
	select
		''U'',
          [Id],
	[ImportId],
	[ModificationStatusId],
	[OriginalImportItemId],
	[MediaMessageId],
	[AdvertisementTypeId],
	[AdvertisedFrom],
	[AdvertisedTo],
	[MediumId],
	[Footage],
	[AdvertisementName],
	[AdvertiserName],
	[AgencyName],
	[CompanyBrandName],
	[ProductCode],
	[ProductName],
	[BlockIdent],
	[BlockCode],
	[BlockEnd],
	[BlockPosition],
	[BlockRating],
	[BlockStart],
	[BlockUnits],
	[PoolingIdentifier],
	[ProgrammeAfter],
	[ProgrammeBefore],
	[ProgrammeTypeAfter],
	[ProgrammeTypeBefore],
	[ProgrammeTypeAfterCode],
	[ProgrammeTypeBeforeCode],
	[TapeCode],
	[TapeLength],
	[TapeName],
	[TapeAgencyName],
	[Rating],
	[Created],
	[CreatedBy],
	[Modified],
	[ModifiedBy],
	[Note],
	[IsHidden]
	from inserted;
end;')

	/*
		Oprávnìní		
	 */
	

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
