USE [MediaData3_KOMPOZITY]
GO

/****** Object:  StoredProcedure [Media].[proc_TvMediaMessage_FindTvImportItem]    Script Date: 18.10.2019 8:59:55 ******/
DROP PROCEDURE [Media].[proc_TvMediaMessage_FindTvImportItem]
GO

/****** Object:  StoredProcedure [Media].[proc_TvMediaMessage_FindTvImportItem]    Script Date: 18.10.2019 8:59:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


/*
	Author: Miroslav Spacek
	Úèelem procedury je nají MM TvLog:
	Trigger je u insertu TvImportItem - to probíhá cca v 7 ráno, v té dobì jsou ale jen tv MM od VideoMatchingu (pokud bìžel), 
	ne od zachytávaèù, nicménì zachtávaèi pøiøazují tvlogy právì jimi vystøiženým zprávám.
	Ne vždy mají zachtávaèí logy k dispozici  (havárie) nebo z vystøihne novou normu pøedchozího dne a VM ji ještì nìkde nalezne
	(po importu logù) tzn. vytvoøme job, který nenapojeným MM zkusí najít tvlog na konci dne (pøed cenìním).
	Viz TP #8228.
  History:
	20101027 - Miroslav Spacek - TP #11186. U TvMessage se neplní TapeAgencyName		
	20101222 - Miroslav Spacek - TP #11751. Nepøiøazovat ProductPlacementu TvLogy		
	20110126 - Miroslav Spacek - TP #7656.  Zalozeni uzivatele AssignTvlogs
	20110214 - Miroslav Spacek - TP #11463. Parametrizace From/To + debug logging
	20110216 - Hobbys          - TP #11463. Optimalizace rychlosti storovky
	20110222 - Miroslav Spacek - TP #12292. Nová promìnná BlockIdent
	20110223 - Miroslav Spacek - TP #12462. U Atmedií se nesmí pøiøazovat rtg z logu
	20110405 - Miroslav Spacek - TP #13258. PM2 versus UCHO - revize tabulky MediumDetail
	20130114 - Karel Benes	   - TP #19077. Tv Petka - importovat rating bloku atd. jako AtMedia
	20131120 - Miroslav Spacek - TP #21437. Optimalizace + MD3
	20131217 - Miroslav Spacek - TP #23642. Zmìnit práci s PrgBloky (na úrovni db)
	20141118 - Jan Muncinsky   - TP #28396. Parametrizace MediumId
	20170206 - Petr Dobes	   - TP #39131. Nová promìnná TvLogShift
	20170523 - Hobbys		   - TP #40238. Optimalizace proc_TvMediaMessage_FindTvImportItem
	20170911 - Ondrej Sadilek  - TP #40937. Normovadlo - Rozchodit Porovnání záchytu a TvLogù pro BG - parametrizace storovky pro CZ a BG
	20171211 - Miroslav Spacek - TP #40826. Parametrizace @toleranceInMs
	20180102 - Miroslav Spacek - TP #42056. Normovadlo - Porovnání záchytu a TvLogù - Custom shift a tolerance
	20180727 - Hobbys		   -  Optimalizace
	20190711 - Miroslav Spacek - #51474. Napojení MM a TvLogù - Nezahrnovat logy s pøíznakem IsHiden
 */
create procedure [Media].[proc_TvMediaMessage_FindTvImportItem]
(
	@from datetime,
	@to datetime,
	@OnlyUnAmbiguous BIT,
    @MediumId smallint=null,
	@tvLogShiftInMs int = null,
	@toleranceInMs int = null
)with recompile
as
begin	
	declare @userId int, @duration float;
	select @userId = 124; -- AssignTvlogs
	
	declare @TvImportItemMediumId varchar(max);
	select @TvImportItemMediumId = [Value] from [dbo].[Params] where ActiveTo >getdate() and [Name]='TvImportItemMediumId';

	declare @TvImportItemAdvertisementTypeId varchar(max);
	select @TvImportItemAdvertisementTypeId = [Value] from [dbo].[Params] where ActiveTo >getdate() and [Name]='TvImportItemAdvertisementTypeId';
	
	if (@toleranceInMs is null)
		select @toleranceInMs = [Value] from [dbo].[Params] where ActiveTo >getdate() and [Name]='SpotMinLength';

	begin try
		begin transaction txMotive;
	
		-- u AtMedia zprav nevyplnuj rtg !!!
		
		CREATE table #AtMedia (MediumId int, OriginalId int, ValidFrom datetime, ValidTo datetime, TargetGroup tinyint);
		
		DECLARE @insertMediumSqlQuery nvarchar(max);
		DECLARE @ParamDefinition NVARCHAR(MAX);

		SET @ParamDefinition = N'@from datetime, @to datetime'

		SELECT @insertMediumSqlQuery = 'select m.Id, cast(m.OriginalId as int), mv.ActiveFrom, mv.ActiveTo, mv.NoAggregationTargetGroupId
			from Media.Medium m
				LEFT JOIN Media.MediumVersion mv on m.Id = mv.MediumId
			where m.MediaTypeId=2 AND ((mv.ActiveFrom < @to AND mv.ActiveTo > @from and mv.ActiveFrom < mv.ActiveTo and mv.IsMinorChannel=1)
			OR (m.Id = ' + @TvImportItemMediumId + ' AND @from >= ''20130131 06:00''));' -- Pìtka

		insert into #AtMedia
			EXEC sp_executesql @insertMediumSqlQuery, @ParamDefinition, @from = @from, @to = @to;
	
		CREATE TABLE [dbo].[#Ids] (
			[Id] [int] identity(1,1) ,
			[MmId] int NOT NULL ,
			[TiiId] [int] NOT NULL ,
			[MediumId] smallint not null,
			[AtMedia] bit not null,
			[rownumber] int not null,
			[ProgrammeBlockId] int null,
			[TapeInfoId] int null,
		) ON [PRIMARY]

		CREATE TABLE #MediaMessageIds (
			[Id] int not null, 
			[MediumId] smallint not null, 
			[From] datetime not null, 
			[MmFootage] int not null, 
			AtMedia bit, 
			TvLogShift int
			);

		declare @modified datetime,	@tvImportId int, @hostName varchar(100), @user varchar(50), @sessionId char(36), @rowcount int, @tvlogId int, @date datetime, @message varchar(max);
		select @modified = getdate(), @hostName = host_name(), @user = suser_name(),@sessionId = cast(newid() as char(36)),@rowcount = 0,@date = getdate();
		   


		-- vyber tv mm, které nemají prirazen TvImportItem, jsou "jisté" nebo "neposuzované" a nesmazané
		DECLARE @insertMmSqlQuery nvarchar(max);
		DECLARE @ParamDefinitioninsertMm NVARCHAR(MAX);

		SET @ParamDefinitioninsertMm = N'@from datetime, @to datetime, @MediumId smallint'

		SELECT @insertMmSqlQuery = 'select distinct mm.Id, mm.MediumId, mm.AdvertisedFrom, tmm.Footage, (case when am.MediumId is null then 0 else 1 end), mv.TvLogShift
			from Media.MediaMessage mm
				join Media.TvMediaMessage tmm on tmm.Id = mm.Id
				join Media.MediumVersion mv ON mm.MediumId = mv.MediumId AND mv.ActiveFrom <= mm.[AdvertisedFrom] AND mv.ActiveTo > mm.[AdvertisedFrom]
				left join Import.TvImportItem tii on tii.MediaMessageId = mm.Id 
				left join #AtMedia am on am.MediumId = mm.MediumId
			where 
				mm.AdvertisedFrom >= @from and mm.AdvertisedFrom < @to
				and mm.CodingPlausibilityId in (8,1)
				and mm.MediaTypeId = 2 
				--and mm.DeletedByBusinessRules is null 
				and mm.AdvertisementTypeId NOT IN ('+ @TvImportItemAdvertisementTypeId +') -- ProductPlacement
				and tii.Id is null
				and (@MediumId IS NULL OR mm.[MediumId] = @MediumId)
			order by mm.AdvertisedFrom;'

		insert into #MediaMessageIds
			EXEC sp_executesql @insertMmSqlQuery, @ParamDefinitioninsertMm, @from = @from, @to = @to, @MediumId = @MediumId;


		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet vložených záznamù @MediaMessageIds: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		--select @from, @to;
		--select getdate(), * from @MediaMessageIds order by [MediumId], [From]

		-- pøiøaï jim tvlog - optimalizace do 2 queries
		SELECT  mmi.Id, mmi.MediumId, mmi.[From], 
				dateadd(ms, @toleranceInMs - isnull(@tvLogShiftInMs, mmi.TvlogShift), mmi.[From]) AS FromPlus, 
				dateadd(ms, -@toleranceInMs - isnull(@tvLogShiftInMs, mmi.TvlogShift), mmi.[From]) AS FromMinus, 
				mmi.MmFootage, mmi.AtMedia, mmi.TvLogShift, m.Id AS mId, m.OriginalId
		INTO #mmi
		FROM #MediaMessageIds mmi
			join Media.Medium m on m.Id = mmi.MediumId
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet vložených záznamù #mmi: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		insert into #Ids
			select mmi.Id, tii.Id, mmi.mId, mmi.AtMedia, 
				row_number() over (partition by mmi.Id order by abs(datediff(second, DATEADD(ms, isnull(@tvLogShiftInMs, mmi.TvlogShift), tii.AdvertisedFrom), mmi.[From])), abs(mmi.MmFootage-tii.Footage)) AS rownumber,
				null, null
			from #mmi mmi
				inner LOOP join Import.TvImportItem tii ON
						mmi.OriginalId = tii.MediumId 
						and tii.AdvertisedFrom <= mmi.FromPlus
						and tii.AdvertisedFrom >= mmi.FromMinus
						and tii.MediaMessageId is null
			where tii.[IsHidden] = 0
			order by mmi.id;
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet vložených záznamù #Ids: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		if not exists(select top 1 1 from [#Ids] [i])
		begin
			if OBJECT_ID('tempdb..#Ids') IS NOT null
				drop table [dbo].[#Ids]
			if OBJECT_ID('tempdb..#AtMedia') IS NOT null		
				drop table [dbo].#AtMedia
			if OBJECT_ID('tempdb..#MediaMessageIds') IS NOT null
				drop table [dbo].#MediaMessageIds
			if OBJECT_ID('tempdb..#mmi') IS NOT null
				drop table [dbo].#mmi
		
			commit  transaction txMotive;
			return;
		end;
		
		--select getdate(), * from #Ids

		-- odstran nejednoznacny
		if (@OnlyUnAmbiguous=1)
		begin
			delete from [#Ids] where MmId in (select MmId from #Ids where [rownumber]=2)
		end;
	

--select getdate(), * from #Ids
	
		
		--------------------------------------------------------------------------------------------------------------------------
		/*********************
		Prirazeni tv logu
		*******************/
		update Import.TvImportItem
		set MediaMessageId = i.MmId,
			Modified = @modified,
			ModifiedBy = @userId
		from Import.TvImportItem tvi
			inner join #Ids i ON tvi.Id = i.tiiid
		where i.[rownumber] = 1;

		--select * from Import.TvImportItem where [Modified]=@modified
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet aktualizovaných záznamù TvImportItem: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		
		DECLARE @EmptyDateTime DATETIME
		SELECT @EmptyDateTime = '19990101'
		/**********************************
		Prace s ProgrammeBlock
		************************************/
		-- Chybejici programove bloky
		INSERT INTO Media.ProgrammeBlock(MediumId, BlockIdent, BlockCode, BlockStart, BlockEnd, BlockUnits,
										BlockRating, BroadcastingDescriptionIdBefore, BroadcastingDescriptionIdAfter, [BlockLength])
			SELECT DISTINCT i.MediumId, 
							coalesce(pb2.[BlockIdent], tii.BlockIdent), coalesce(pb2.BlockCode, tii.BlockCode), coalesce(pb2.BlockStart, tii.BlockStart), 
							coalesce(pb2.BlockEnd, tii.BlockEnd), coalesce(pb2.BlockUnits, tii.BlockUnits),
							coalesce(pb2.BlockRating, (CASE WHEN i.atmedia = 1 THEN NULL ELSE tii.BlockRating end)) /*ATckam nenastavovat BlockRating - pokud jej nemaji, pak nechat*/,
							coalesce(pb2.BroadcastingDescriptionIdBefore, tii.BroadcastingDescriptionIdBefore), 
							coalesce(pb2.BroadcastingDescriptionIdAfter, tii.BroadcastingDescriptionIdAfter), 
							pb2.[BlockLength] -- jeste neznam, pokud nekdo nevyplnil rucne, pak nechat
			FROM Import.TvImportItem tii
				inner join #Ids i on tii.Id = i.tiiid
				inner join [Media].[TvMediaMessage] tmm on tmm.id = i.mmId --nechci prepisovat jiz vyplnene, jen chybejici
				left  join [Media].[ProgrammeBlock] pb2 on pb2.id = tmm.[ProgrammeBlockId]  -- aktualni, nechci prepsat
				left  join [Media].ProgrammeBlock pb on    -- hledany
						i.MediumId = pb.MediumId
					and isnull(coalesce(pb2.[BlockIdent], tii.BlockIdent), '')			= isnull(pb.BlockIdent, '')
					and isnull(coalesce(pb2.BlockCode, tii.BlockCode), '')				= isnull(pb.BlockCode, '')
					and isnull(coalesce(pb2.BlockStart, tii.BlockStart), @EmptyDateTime)= isnull(pb.BlockStart, @EmptyDateTime)
					and isnull(coalesce(pb2.BlockEnd, tii.BlockEnd), @EmptyDateTime)	= isnull(pb.BlockEnd, @EmptyDateTime)
					and isnull(coalesce(pb2.BlockUnits, tii.BlockUnits), 255)			= isnull(pb.BlockUnits, 255)
					and isnull(coalesce(pb2.BlockRating, 
					 (case when i.atmedia = 1 then null else tii.BlockRating end)), -1) = isnull(pb.BlockRating, -1)
					and isnull(coalesce(pb2.BroadcastingDescriptionIdBefore, tii.BroadcastingDescriptionIdBefore), -1)	= isnull(pb.BroadcastingDescriptionIdBefore, -1)
					and isnull(coalesce(pb2.BroadcastingDescriptionIdAfter, tii.BroadcastingDescriptionIdAfter), -1)	= isnull(pb.BroadcastingDescriptionIdAfter, -1)
					and isnull(pb2.[BlockLength], -1)									= isnull(pb.[BlockLength], -1)
			where i.rownumber = 1 and  pb.Id is null
				and  --  nezkladej prazdny
				(
					   isnull(coalesce(pb2.[BlockIdent], tii.BlockIdent), '') != ''
					or isnull(coalesce(pb2.BlockCode, tii.BlockCode), '') != ''
					or isnull(coalesce(pb2.BlockStart, tii.BlockStart), @EmptyDateTime) != @EmptyDateTime
					or isnull(coalesce(pb2.BlockEnd, tii.BlockEnd), @EmptyDateTime) != @EmptyDateTime
					or isnull(coalesce(pb2.BlockUnits, tii.BlockUnits), -1) != -1
					or isnull(coalesce(pb2.BlockRating, 
					 (case when i.atmedia = 1 then null else tii.BlockRating end)), -1) != -1
					or isnull(coalesce(pb2.BroadcastingDescriptionIdBefore, tii.BroadcastingDescriptionIdBefore), -1) != -1
					or isnull(coalesce(pb2.BroadcastingDescriptionIdAfter, tii.BroadcastingDescriptionIdAfter), -1) != -1
				)
		
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet vložených záznamù Media.ProgrammeBlock: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		-- napoj
		update i 
			set ProgrammeBlockId = pb.Id
		from #Ids i
			inner join [Import].[TvImportItem] tii on tii.Id = i.tiiid
			inner join [Media].[TvMediaMessage] tmm on tmm.id = i.mmId --nechci prepisovat jiz vyplnene, jen chybejici
			left hash join [Media].[ProgrammeBlock] pb2 on pb2.id = tmm.[ProgrammeBlockId]  -- aktualni, nechci prepsat
			inner join [Media].ProgrammeBlock pb on
					i.MediumId = pb.MediumId
				and isnull(coalesce(pb2.[BlockIdent], tii.BlockIdent), '')			= isnull(pb.BlockIdent, '')
				and isnull(coalesce(pb2.BlockCode, tii.BlockCode), '')				= isnull(pb.BlockCode, '')
				and isnull(coalesce(pb2.BlockStart, tii.BlockStart), @EmptyDateTime)= isnull(pb.BlockStart, @EmptyDateTime)
				and isnull(coalesce(pb2.BlockEnd, tii.BlockEnd), @EmptyDateTime)	= isnull(pb.BlockEnd, @EmptyDateTime)
				and isnull(coalesce(pb2.BlockUnits, tii.BlockUnits), 255)			= isnull(pb.BlockUnits, 255)
				and isnull(coalesce(pb2.BlockRating, 
				 (case when i.atmedia = 1 then null else tii.BlockRating end)), -1) = isnull(pb.BlockRating, -1)
				and isnull(coalesce(pb2.BroadcastingDescriptionIdBefore, tii.BroadcastingDescriptionIdBefore), -1)	= isnull(pb.BroadcastingDescriptionIdBefore, -1)
				and isnull(coalesce(pb2.BroadcastingDescriptionIdAfter, tii.BroadcastingDescriptionIdAfter), -1)	= isnull(pb.BroadcastingDescriptionIdAfter, -1)
				and isnull(pb2.[BlockLength], -1)									= isnull(pb.[BlockLength], -1)
		where i.rownumber = 1 	
		
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet aktualizovaných záznamù #Ids (ProgrammeBlockId): ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		 
--select getdate(), * from #Ids

		/**********************************
		Prace s TapeInfo
		************************************/
		INSERT INTO Media.TapeInfo(TapeAgencyName, TapeCode, TapeLength, TapeName)
			SELECT DISTINCT tii.AgencyName, tii.[TapeCode], tii.[TapeLength], tii.[TapeName]
			FROM Import.TvImportItem tii
				inner join #Ids i ON tii.Id = i.tiiid
				left join [Media].[TapeInfo] ti on
						isnull(tii.[TapeCode], '') = isnull(ti.[TapeCode], '') 
					and isnull(tii.[TapeName], '') = isnull(ti.[TapeName], '') 
					and isnull(tii.AgencyName, '') = isnull(ti.[TapeAgencyName], '') 
					and isnull(tii.[TapeLength], -1) = isnull(ti.[TapeLength], -1)
			where i.rownumber = 1 and ti.Id is null
					and isnull(tii.[TapeCode], '') != ''
		
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet vložených záznamù Media.TapeInfo: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
		-- napoj
		update i 
			set TapeInfoId = ti.Id
		from #Ids i
			inner join [Import].[TvImportItem] tii on tii.Id = i.tiiid
			inner join [Media].[TapeInfo] ti on
					isnull(tii.[TapeCode], '') = isnull(ti.[TapeCode], '') 
				and isnull(tii.[TapeName], '') = isnull(ti.[TapeName], '') 
				and isnull(tii.AgencyName, '') = isnull(ti.[TapeAgencyName], '') 
				and isnull(tii.[TapeLength], -1) = isnull(ti.[TapeLength], -1)
		where i.rownumber = 1
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet aktualizovaných záznamù #Ids (TapeInfoId): ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
		
--select getdate(), * from #Ids

		
		/*********************
		Na zaver update MMs
		*******************/			
		update Media.TvMediaMessage
		set Rating = (case when (select top 1 AtMedia from #Ids where MmId = msg.Id) = 1 then msg.Rating else coalesce(msg.Rating, tvi.Rating) end),
			BlockPosition = coalesce(msg.BlockPosition, tvi.BlockPosition),
			TapeInfoId = i.TapeInfoId, --coalesce(msg.TapeInfoId, i.TapeInfoId),
			ProgrammeBlockId = i.ProgrammeBlockId --coalesce(msg.ProgrammeBlockId, i.ProgrammeBlockId)
		from Media.TvMediaMessage msg
			inner join Import.TvImportItem tvi on msg.Id = tvi.MediaMessageId
			inner join #Ids i ON tvi.Id = i.tiiid
		where i.[rownumber] = 1;
								
		-- log
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(), @message = 'Poèet aktualizovaných záznamù TvMediaMessage: ' + cast(@rowcount as varchar(15)) + '.';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;
				
		update Media.MediaMessage
		set Modified = @modified,
			ModifiedBy = @userId 
		from Media.MediaMessage msg
			inner join Import.TvImportItem tvi on msg.Id = tvi.MediaMessageId
			inner join #Ids i ON tvi.Id = i.tiiid
		where i.[rownumber] = 1;
		
		-- log
		select @rowcount = @@ROWCOUNT;
		select @duration = datediff(millisecond, @date, getdate());
		select @date = getdate(),
			   @message = 'Byly nalezeny jednoznaèné TvLogy pro ' + cast(@rowcount as varchar(15)) + ' zpráv. Modified = ''' +  CONVERT(VARCHAR(30), @modified, 21) + '''';
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'DEBUG', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @message, @Exception = null, @DurationInMiliseconds = @duration;

		
		if OBJECT_ID('tempdb..#Ids') IS NOT null
			drop table [dbo].[#Ids]
		if OBJECT_ID('tempdb..#AtMedia') IS NOT null		
			drop table [dbo].#AtMedia
		if OBJECT_ID('tempdb..#MediaMessageIds') IS NOT null
			drop table [dbo].#MediaMessageIds
		if OBJECT_ID('tempdb..#mmi') IS NOT null
			drop table [dbo].#mmi

		commit  transaction txMotive;


	end try
	begin catch	
		print error_message();

		if OBJECT_ID('tempdb..#Ids') IS NOT null
			drop table [dbo].[#Ids]
		if OBJECT_ID('tempdb..#AtMedia') IS NOT null		
			drop table [dbo].#AtMedia
		if OBJECT_ID('tempdb..#MediaMessageIds') IS NOT null
			drop table [dbo].#MediaMessageIds
		if OBJECT_ID('tempdb..#mmi') IS NOT null
			drop table [dbo].#mmi
			
		
		if @@TRANCOUNT > 0
			rollback transaction txMotive;
		
		declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
		select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
		select @date = getdate();
		exec Logging.proc_Log_Save @Date = @date, @SessionId = @sessionId, @HostName = @hostName, @UserName = @user, @LogLevel = 'ERROR', @Logger = 'proc_TvMediaMessage_FindTvImportItem', @Thread = '', @Message = @ErrorMessage, @Exception = null;

		raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
	end catch										
end;
GO


