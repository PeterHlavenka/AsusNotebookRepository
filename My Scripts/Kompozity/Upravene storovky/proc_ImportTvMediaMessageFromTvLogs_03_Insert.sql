if exists(select * 
	from information_schema.routines
	where specific_schema = N'Import'
		and specific_name = N'proc_ImportTvMediaMessageFromTvLogs_03_Insert' 
)
	drop procedure [Import].[proc_ImportTvMediaMessageFromTvLogs_03_Insert];
GO

/* 
    Author: Miroslav Spacek
    Created: 09.03.2011 - #12624. Skript na vložení TvLogu z CCC
    Description:	- máme skript, který vloží logy na základì pøesahu a výjimek k nevložení
					- chceme skript, který vloží pevnì daná idéèka s jakýmkoliv pøesahem (budou opravena v Z/N)
	Updated: 09.02.2012, Miroslav Spacek - #16339. Rating u insertu MM at media
	         21.09.2012, Petr Dobes - #17647 Statistiky - storovka Import.proc_ImportTvMediaMessageFromTvLogs_03_Insert musí do Logging.Stat insertovat, pokud pøiøadí motiv
			 06.09.2017, Ondrej Sadilek - #40930 Normovadlo - Porovnání záchytu a TvLogu - Dovolit vložit MM bez TapeCode
			 13.09.2017, Ondrej Sadilek - #40937 Normovadlo - Rozchodit Porovnání záchytu a TvLogù pro BG
			 02.05.2018, Hobbys - optimalizace nalezeni existujicich PrgBloku
			 11.05.2018, Kerles - vyplneni footage i v pripade, ze pod TapeCodem jeste neni zadna jina MM
			 14.05.2018, Kerles - logging
			 31.05.2018, Kerles - pokud nenajde pro nejaky TapeCode normu, tak se prerusi a vrati TapeCode
			 29.06.2018, Kerles - nenapojovat Normy, ktere maji nastaveno IgnoreDuplicty
			 09.10.2018, Kerles - pri vytvareni MM odecist TVLogShift od AdvFrom a AdvTo 
			 08.03.2019, Peter Hlavenka - TFS 50464 - zmena znamenka TVLogShift - pri vytvareni MM pricist TVLogShift od AdvFrom a AdvTo
*/
create procedure [Import].[proc_ImportTvMediaMessageFromTvLogs_03_Insert]		
(	
		@InsertIds varchar(max) = '',
		@InsertNormId int = '',
		@ErrorTapeCode varchar(max) out
)
as
begin	
	--begin tran;
	set nocount on;

	-- Logging
		declare @date datetime, @hostName varchar(100), @user varchar(50), @message varchar(max), @rowcount int, @duration float;
		select @date = getdate(), @hostName = host_name(), @user = suser_name(), @message = ' - script proc_ImportTvMediaMessageFromTvLogs_03_Insert';		
		exec Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '';

	declare @ErrorSave int, @ModifiedBy int, @Modified datetime, @IsMotiveFinded varchar(500), @HintId int, @CodingPlausibilityId int, @NewMMId int;
	select @ModifiedBy = 220 /*Import zpráv podle Tv logù*/, @Modified = getdate();

	-- vkladej pouze tyto TvLogy (definovano uzivatelem)
	if object_id('tempdb..#InsertIdsTable') is not null
		drop table [dbo].[#InsertIdsTable]
	create table [dbo].[#InsertIdsTable] (
		[TvImportItemId] int NOT NULL primary key
	) on [PRIMARY]

	if (ltrim(rtrim(@InsertIds)) != '')
	begin
		declare @sql nvarchar(max)
		set @sql='insert into #InsertIdsTable
					select Id from Import.TvImportItem where Id in ('+@InsertIds+');';
		exec(@sql)
	end;
	select @rowcount = @@rowcount, @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'The number of inserted records into #InsertIdsTable: ' + ISNULL(CAST(@rowcount as varchar),'NULL');		
	exec Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
	--select TvImportItemId as InsertIds from #InsertIdsTable order by TvImportItemId
	
	
	-- najdi jim normu a motiv

	create table #TvLogsForInsert (TvImportItemId int not null primary key, MotiveId int, NormCrId int, Footage int);
	insert into #TvLogsForInsert
		select tii.Id as TiiId, 
					(CASE WHEN @InsertNormId IS NOT NULL THEN
					(select top 1 A.MotiveId from
						(	 
							select c.MotiveId, count(mm.Id) as pocet from Media.MediaMessage mm
								inner join Media.TvMediaMessage tmm on mm.Id = tmm.Id
								inner join [Creative].[Creative] c on mm.[NormCreativeId] = c.id
							where mm.NormCreativeId = @InsertNormId
							group by c.MotiveId
						) A 
						ORDER BY A.pocet DESC) 
						ELSE
					(select top 1 A.MotiveId from
						(	 
							select ti.TapeCode, c.MotiveId, count(mm.Id) as pocet from Media.MediaMessage mm
								inner join Media.TvMediaMessage tmm on mm.Id = tmm.Id
								inner join [Creative].[Creative] c on mm.[NormCreativeId] = c.id
								inner join [Media].[TapeInfo] ti on [tmm].[TapeInfoId] = [ti].[Id]
							where ti.TapeCode  = tii.TapeCode
							group by ti.TapeCode, c.MotiveId
						) A
						where A.TapeCode = tii.TapeCode
						order by A.TapeCode, A.pocet desc) END) as MotiveId,
					(CASE WHEN @InsertNormId IS NOT NULL THEN @InsertNormId ELSE(select top 1 B.NormCreativeId from
						(	 
							select ti.TapeCode, mm.NormCreativeId, count(mm.Id) as pocet from Media.MediaMessage mm
								join Media.TvMediaMessage tmm on mm.Id = tmm.Id
								inner join Creative.CreativeToCreativeItem ctci on ctci.CreativeId = mm.NormCreativeId
								inner join Creative.CreativeItem ci on ci.Id = ctci.CreativeItemId
								inner join [Media].[TapeInfo] ti on [tmm].[TapeInfoId] = [ti].[Id]
							where ti.TapeCode  = tii.TapeCode
							and ci.IgnoreDuplicity = 0
							group by ti.TapeCode, mm.NormCreativeId
						) B
						where B.TapeCode = tii.TapeCode
						order by B.TapeCode, B.pocet desc) END) AS NormCrId,
						(CASE WHEN @InsertNormId IS NOT NULL THEN
							(SELECT TOP 1 C.Footage FROM
							(SELECT COUNT(mm.Id) AS Pocet, tmm.Footage
								FROM Media.MediaMessage mm
								JOIN Media.TvMediaMessage tmm ON mm.Id = tmm.Id
								JOIN Creative.Creative c ON mm.NormCreativeId = c.Id
								WHERE mm.NormCreativeId = @InsertNormId
								GROUP BY tmm.Footage) C
								ORDER BY C.Pocet DESC)
							ELSE
								(SELECT COALESCE(
								(SELECT top 1 C.Footage from
									(SELECT ti.TapeCode, tmm.Footage, count(mm.Id) as pocet 
									FROM Media.MediaMessage mm
									JOIN Media.TvMediaMessage tmm on mm.Id = tmm.Id
									INNER join [Media].[TapeInfo] ti on [tmm].[TapeInfoId] = [ti].[Id]
									WHERE ti.TapeCode = tii.TapeCode
									GROUP by ti.TapeCode, tmm.Footage) C
								WHERE C.TapeCode = tii.TapeCode
								ORDER by C.TapeCode, C.pocet DESC),
								DATEDIFF(SECOND,tii.AdvertisedFrom, tii.AdvertisedTo)))	
							END) as Footage
			from Import.TvImportItem tii
				join #InsertIdsTable iit on iit.TvImportItemId = tii.Id;
	select @rowcount = @@rowcount, @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'The number of inserted records into #TvLogsForInsert: ' + ISNULL(cast(@rowcount as varchar),'NULL');		
	EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
	--select * from #TvLogsForInsert;
	
	-- jdi insertovat
	declare @TvImportItemId int, @MotiveId int, @NormCreativeId int, @Footage int;
	-- CURSOR
	DECLARE vendor_cursor CURSOR FOR 
		
		select TvImportItemId, MotiveId, NormCrId, Footage from #TvLogsForInsert;

		OPEN vendor_cursor
			
		FETCH NEXT FROM vendor_cursor 
			INTO @TvImportItemId, @MotiveId, @NormCreativeId, @Footage

		WHILE @@FETCH_STATUS = 0
		begin			
				/* neres konflikty
				if not exists( -- konflikt s jiz existujici zpravou
					select top 1 1 --@TvImportItemId, @MotiveId, mm.Id, mm.MotiveId
					from Media.MediaMessage mm
					where @MediumId = mm.MediumId
						and (  
									@AdvFrom <= dateadd(second, -@ToleranceInSec, mm.AdvertisedTo) 
								and @AdvdTo  >= dateadd(second, @ToleranceInSec, mm.AdvertisedFrom)
							)
					)
				begin*/
				
				begin tran
				
					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'Inserting new message for TVImportItemId=' + ISNULL(cast(@TvImportItemId as varchar),'NULL');
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
	
					if (@MotiveId is null)						  
						select @IsMotiveFinded = 'Nenalezen motiv', @CodingPlausibilityId = 1 /*nezpracovane*/
					else
					 begin 
						select @IsMotiveFinded = 'Nalezen motiv', @CodingPlausibilityId = 8 /*jiste*/;
						insert into Logging.Stat( [Key] ,Target ,TargetTypeId , Value1 ,Value1TypeId ,Value2 , Value2TypeId , Date , UserId)
						values  ( 601 ,  @NewMMId ,  1 , @MotiveId , 2 , NULL , NULL , @Modified , @ModifiedBy ) 
					 end 

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'Motive searching: ' + @IsMotiveFinded;		
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
					 	
					insert into Media.CodingHintContext (AdvertiserCrn, AdvertiserName, AgencyCrn, AgencyName, CampaignName, CompanyBrandName, ProductBrandName, ProductCategoryCode, ProductCategoryName, ProductSpecification, MotiveName)
						select null, tvii.AdvertiserName, null, tvii.AgencyName, tvii.TapeName, tvii.CompanyBrandName, tvii.AdvertisementName, tvii.ProductCode, tvii.ProductName, 'Zdroj Tvlogy', @IsMotiveFinded from Import.TvImportItem tvii where Id = @TvImportItemId;
					select @HintId = scope_identity();

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'HinId=' + ISNULL(CAST(@HintId AS VARCHAR),'NULL') + ' inserted to Media.CodingHintContext.';		
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;


				if (@NormCreativeId is null)
					begin
						select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'No norm for TapeCode found in databse. Exitting with Result = 1.';
						exec Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
						select @ErrorTapeCode = tvii.TapeCode from Import.TvImportItem tvii where tvii.Id = @TvImportItemId;
						commit tran;
						return;
					end;							

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'NormCreativeId is: ' + ISNULL(CAST(@NormCreativeId AS VARCHAR),'NULL');
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;

--select @NormCreativeId as normid;
					
					insert into Media.MediaMessage (ImportId, MediaTypeId, AdvertisementTypeId, AdvertisementSourceId, AdvertisedFrom, AdvertisedTo, CodingHintContextId,
													CodingPlausibilityId, Ready, MediumId, PriceCurrencyId, PriceScopeId,	IsPriceUnknown,
													NormCreativeId, [CreativeId], Created, CreatedBy)
							select tvii.ImportId, 2, tvii.AdvertisementTypeId, 61, dateadd(millisecond, mv.TvLogShift, tvii.AdvertisedFrom), dateadd(millisecond, mv.TvLogShift, tvii.AdvertisedTo), @HintId,
									@CodingPlausibilityId, 0, am.Id, 1, 1, 1, @NormCreativeId, @NormCreativeId, @Modified, @ModifiedBy
							from Import.TvImportItem tvii 
								join Media.Medium am on am.OriginalId = tvii.MediumId
								join Media.MediumVersion mv on mv.MediumId = am.Id and mv.ActiveFrom <= tvii.AdvertisedFrom and mv.ActiveTo > tvii.AdvertisedFrom
							where tvii.Id = @TvImportItemId;
					select @NewMMId = scope_identity();

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'New MediaMessageId=' + ISNULL(CAST(@NewMMId AS VARCHAR),'NULL') + ' inserted to Media.MediaMessage.';
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
--select @NewMMId as mmId		
		
					-- propoj MM a logem - tim "vypnu" trigger (trigger jiz stejne neexistuje)
					update Import.TvImportItem
						set MediaMessageId = @NewMMId,
							Modified = @Modified,
							ModifiedBy = @ModifiedBy
					where Id = @TvImportItemId;
					
					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'TVIMportItemId=' + ISNULL(CAST(@TvImportItemId AS VARCHAR),'NULL') + ' connected to MediaMessageId=' + ISNULL(CAST(@NewMMId AS VARCHAR),'NULL') + ' (inserted to Import.TvImportItem).';
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
					
					declare @TapeInfoId int, @ProgrammeBlockId int, @EmptyDateTime datetime;
					select @TapeInfoId = null, @ProgrammeBlockId  = null, @EmptyDateTime = null;
					select @EmptyDateTime = '19990101';
					/**********************************
					Prace s TapeInfo
					************************************/
					
					/* Kdyz dostanu normu, nemam TapeCode a nemohu insertovat TapeInfo. Bude tedy NULL, ktere lze insertovat do TvMediaMessage.*/
					IF(ISNULL(@InsertNormId, '') != '')
					BEGIN
						SELECT @TapeInfoId = NULL;
					END
					ELSE
					BEGIN
						insert into Media.TapeInfo(TapeAgencyName, TapeCode, TapeLength, TapeName)
						select distinct tvii.AgencyName, tvii.TapeCode, tvii.TapeLength, tvii.TapeName
						from Import.TvImportItem tvii  
							left join [Media].[TapeInfo] ti on
								isnull(tvii.[TapeCode], '') = isnull(ti.[TapeCode], '') 
								and isnull(tvii.[TapeName], '') = isnull(ti.[TapeName], '') 
								and isnull(tvii.AgencyName, '') = isnull(ti.[TapeAgencyName], '') 
								and isnull(tvii.[TapeLength], -1) = isnull(ti.[TapeLength], -1)
						where tvii.Id = @TvImportItemId and ti.Id IS null and isnull(tvii.TapeCode, '') != ''
					if(@@ROWCOUNT > 1)
						select @TapeInfoId = scope_identity();
					--select @TapeInfoId as ti
					if (@TapeInfoId is null)
						select @TapeInfoId = ti.[Id]
						from Import.TvImportItem tvii  
							inner join [Media].[TapeInfo] ti on
								isnull(tvii.[TapeCode], '') = isnull(ti.[TapeCode], '') 
								and isnull(tvii.[TapeName], '') = isnull(ti.[TapeName], '') 
								and isnull(tvii.AgencyName, '') = isnull(ti.[TapeAgencyName], '') 
								and isnull(tvii.[TapeLength], -1) = isnull(ti.[TapeLength], -1)
						where tvii.Id = @TvImportItemId;
					--select @TapeInfoId as ti
					
					if (@TapeInfoId is null)
						raiserror ('@TapeInfoId is null', 11, 11);
					END;

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'TapeInfoId=' + ISNULL(CAST(@TapeInfoId AS VARCHAR),'NULL');
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
										
						
					/**********************************
					Prace s ProgrammeBlock
					************************************/
					-- Chybejici programove bloky
					INSERT INTO Media.ProgrammeBlock(MediumId, BlockIdent, BlockCode, BlockStart, BlockEnd, BlockUnits,
													BlockRating, [BlockLength],
													[BroadcastingDescriptionIdBefore], [BroadcastingDescriptionIdIn], [BroadcastingDescriptionIdAfter])
						SELECT DISTINCT am.Id, tvii.BlockIdent, tvii.BlockCode, tvii.BlockStart, tvii.BlockEnd, tvii.BlockUnits, 
										null, datediff(second, tvii.BlockStart, tvii.BlockEnd), tvii.[BroadcastingDescriptionIdBefore], tvii.[BroadcastingDescriptionIdIn], [tvii].[BroadcastingDescriptionIdAfter]
						from Import.TvImportItem tvii  
							inner join Media.Medium am on am.OriginalId = tvii.MediumId
							left outer hash join [Media].ProgrammeBlock pb ON
									am.Id = pb.[MediumId]
								AND ISNULL(tvii.BlockIdent, '') = ISNULL(pb.BlockIdent, '')
								AND ISNULL(tvii.BlockCode, '') = ISNULL(pb.BlockCode, '')
								AND ISNULL(tvii.BlockStart, @EmptyDateTime) = ISNULL(pb.BlockStart, @EmptyDateTime)
								AND ISNULL(tvii.BlockEnd, @EmptyDateTime) = ISNULL(pb.BlockEnd, @EmptyDateTime)
								AND ISNULL(tvii.BlockUnits, 255) = ISNULL(pb.BlockUnits, 255)
								AND ISNULL(tvii.[BroadcastingDescriptionIdBefore], -1) != -1
								AND ISNULL(tvii.[BroadcastingDescriptionIdIn], -1) != -1
								AND ISNULL(tvii.[BroadcastingDescriptionIdAfter], -1) != -1
								AND -1 = ISNULL(pb.BlockRating, -1)
								AND -1 = ISNULL(pb.[BlockLength], -1)
						where tvii.Id = @TvImportItemId and pb.Id IS NULL
							and  --  nezkladej prazdny
							(
								ISNULL(tvii.BlockIdent, '') != ''
								OR ISNULL(tvii.BlockCode, '') != ''
								OR ISNULL(tvii.BlockStart, @EmptyDateTime) != @EmptyDateTime
								OR ISNULL(tvii.BlockEnd, @EmptyDateTime) != @EmptyDateTime
								OR ISNULL(tvii.BlockUnits, -1) != -1
								OR ISNULL(tvii.[BroadcastingDescriptionIdBefore], -1) != -1
								OR ISNULL(tvii.[BroadcastingDescriptionIdIn], -1) != -1
								OR ISNULL(tvii.[BroadcastingDescriptionIdAfter], -1) != -1
							)
					if(@@ROWCOUNT > 1)
						select @ProgrammeBlockId = scope_identity();
					
					SELECT @duration = datediff(millisecond, @date, getdate()), @date = getdate();
					IF (@ProgrammeBlockId is NOT NULL)
					SELECT @message = 'ProgramBlock searching STEP 1 of 2. ProgrammeBlockId=' + ISNULL(CAST(@ProgrammeBlockId AS VARCHAR),'NULL');
					ELSE
					SELECT @message = 'ProgramBlock searching STEP 1 of 2 finished. @ProgrammeBlockId not found. Starting STEP 2: @TvImportItemId=' + ISNULL(CAST(@TvImportItemId AS VARCHAR),'NULL')
					 + ', @EmptyDateTime=' + ISNULL(CAST(@EmptyDateTime AS VARCHAR),'NULL');
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
							
					
--select @ProgrammeBlockId as pb

					if (@ProgrammeBlockId is null)
						SELECT @ProgrammeBlockId = pb.id
						from Import.TvImportItem tvii  
							inner join Media.Medium am on am.OriginalId = tvii.MediumId
							inner join [Media].ProgrammeBlock pb on
									am.Id = pb.[MediumId]
								AND ISNULL(tvii.BlockIdent, '') = ISNULL(pb.BlockIdent, '')
								AND ISNULL(tvii.BlockCode, '') = ISNULL(pb.BlockCode, '')
								AND ISNULL(tvii.BlockStart, @EmptyDateTime) = ISNULL(pb.BlockStart, @EmptyDateTime)
								AND ISNULL(tvii.BlockEnd, @EmptyDateTime) = ISNULL(pb.BlockEnd, @EmptyDateTime)
								AND ISNULL(tvii.BlockUnits, 255) = ISNULL(pb.BlockUnits, 255)
								AND -1 = ISNULL(pb.BlockRating, -1)
								AND ISNULL(tvii.[BroadcastingDescriptionIdBefore], -1) != -1
								AND ISNULL(tvii.[BroadcastingDescriptionIdIn], -1) != -1
								AND ISNULL(tvii.[BroadcastingDescriptionIdAfter], -1) != -1
								AND -1 = ISNULL(pb.[BlockLength], -1)
						where tvii.Id = @TvImportItemId;
						
						SELECT @duration = datediff(millisecond, @date, getdate()), @date = getdate();
						IF (@ProgrammeBlockId is NOT NULL)
						SELECT @message = 'ProgramBlock searching STEP 2 of 2 finished. @ProgrammeBlockId = ' + ISNULL(CAST(@ProgrammeBlockId AS VARCHAR),'NULL');
						ELSE
						SELECT @message = 'ProgramBlock searching STEP 2 of 2 finished. @ProgrammeBlockId not found.';
						EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;

					--select @ProgrammeBlockId as pb					

					--Zakomentovana kontrola kvuli tasku nize
					--#40937 Normovadlo - Rozchodit Porovnání záchytu a TvLogù pro BG
					--if (@ProgrammeBlockId is null)
					--	raiserror ('@ProgrammeBlockId is null', 11, 11);

			
					--MM
					insert into Media.TvMediaMessage(Id, Footage, Rating, TapeInfoId, ProgrammeBlockId, BlockPosition)
						select @NewMMId, @Footage, null, @TapeInfoId, @ProgrammeBlockId, tvii.BlockPosition
						from Import.TvImportItem tvii where Id = @TvImportItemId;

					select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'New MediMessageId=' + ISNULL(CAST(@NewMMId AS VARCHAR),'NULL') + ' inserted into Media.TvMediaMessage.';
					EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
			
				commit tran 
				
			FETCH NEXT FROM vendor_cursor 
				INTO @TvImportItemId, @MotiveId, @NormCreativeId, @Footage
		END		

	CLOSE vendor_cursor
	DEALLOCATE vendor_cursor
	
	set nocount off;
	
	
	--vypisy (pouziva Normovadlo pro informovani uzivatele o poctu zalozenych MM)		
	select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'Starting "Vypisy". @Modified =' + ISNULL(CAST(@Modified AS VARCHAR),'NULL');
	EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;

	select tvii.id as TvImportItem, mm.Id as NewMediaMessageId, mm.MediumId, mm.CodingPlausibilityId, mm.AdvertisedFrom, mm.AdvertisedTo, pb.id as PrgBlockId, pb.[BlockStart], pb.[BlockEnd], c.MotiveId, mv.Id as MotiveVersionId, mm.NormCreativeId, mm.CodingHintContextId 
	from  Media.MediaMessage mm
		inner join [Media].[TvMediaMessage] tmm on mm.id = tmm.id
	join Import.TvImportItem tvii on tvii.MediaMessageId=mm.Id
		left join [Creative].[Creative] c on [mm].[NormCreativeId] = [c].[Id]
		left join [Media].[MotiveVersion] mv on c.[MotiveId] = mv.[MotiveId] and mm.[AdvertisedFrom] >= mv.[ActiveFrom] and mm.[AdvertisedFrom] < mv.[ActiveTo]
		left join [Media].[ProgrammeBlock] pb on tmm.[ProgrammeBlockId] = pb.id
	 where mm.Created = @Modified
	order by mm.MediumId, mm.AdvertisedFrom;

	select @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = '"Vypisy" finished. Deleting tempdb...';
	EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
	
	if object_id('tempdb..#InsertIdsTable') is not null
		drop table [dbo].[#InsertIdsTable];
	
	SELECT @duration = datediff(millisecond, @date, getdate()), @date = getdate(), @message = 'Tempdb deleted. Procedure ImportTvMediaMessageFromTvLogs finished.';
	EXEC Logging.proc_Log_Save @Date = @date, @SessionId = '', @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_ImportTvMediaMessageFromTvLogs_03_Insert', @Thread = '',	@Message = @message, @Exception = '', @DurationInMiliseconds = @duration;
	
	--commit tran;	
	SET @ErrorTapeCode = '';
end;

GO

GRANT EXEC ON [Import].[proc_ImportTvMediaMessageFromTvLogs_03_Insert] TO MediaDataBasicAccess
/*
SELECT top 100 * FROM Import.TvImportItem where Id in (417405,420346);
exec Import.proc_ImportTvMediaMessageFromTvLogs_03_Insert
	@InsertIds='417405, 420346';
	

*/
