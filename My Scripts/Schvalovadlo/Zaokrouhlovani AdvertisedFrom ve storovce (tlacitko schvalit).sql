   
if exists(select * 
	from information_schema.routines
	where specific_schema = N'Media'
		and specific_name = N'proc_AutoImporter_SetAttributes' 
)
	drop procedure [Media].[proc_AutoImporter_SetAttributes];
GO

/*
	Úèelem procedury je nastavit vybrané atributy pro reklamy vybraných médií za urèitý den.
	Jedná se o atributy používané exportní službou a které urèují, zda se reklamy mohou exportovat nebo ne.
	Autor: Jan Procházka
	History: 20090904 created
    - 03.01.2011 - Miroslav Spacek	- TP #11782. Povolit export tv zpráv, které nejsou nacenìny	
    - 16.09.2012 - Petr Dobeš		- TP #22393. Povolit schvaleni nenacenenych radiovych zprav
    - 25.09.2013 - Petr Dobeš		- TP #22523
    - 25.11.2013 - Miroslav Spacek	- TP # Neschvalovat jiste bez motivu, premiery
	- 16.06.2014 - Miroslav Spacek	- TP #26190. Schvalovadlo - Neschvalovat tv po šesté
	- 08.10.2015 - Petr Dobeš		- TP #33177 U Tv neschvalovat zprávy, které mají NULL cenu a nejsou ProductPlacement
	- 26.09.2016 - Miroslav Spacek	- TP #37568. Schvalovadlo - Schvalovat i nedokonèenou denní dávku
*/
create procedure [Media].[proc_AutoImporter_SetAttributes]
(
	@StartDate datetime,
	@EndDate datetime,
	@MediumId smallint,
	@MediaTypeId int,
	@Force bit,	
	@UserId tinyint,
	@Period varchar(25)
)
as
begin	
	set transaction isolation level serializable;
	
	begin transaction txAutoImport;

	begin try
		declare @plausibilityNone tinyint,
				@plausibilitySure tinyint,
				@plausibilityUntouched tinyint,
				@plausibilityD tinyint;

		select @plausibilityNone = 0,		-- X Neposuzované
			   @plausibilitySure = 8,		-- A Jisté
			   @plausibilityUntouched = 1,	-- 0 Nezpracované
			   @plausibilityD = 16;

		--#26190. Schvalovadlo - Neschvalovat tv (tim vlastne nic) po šesté
		declare @TodayTvEnd datetime;
		select @TodayTvEnd = dateadd(hour, 6, cast(floor(cast(getdate() as float)) as datetime));
		if (@TodayTvEnd < @EndDate)
			select @EndDate = @TodayTvEnd;

		-- denní a mìsíèní dávka
		if @Period in ('Monthly') 
			and not exists(select 1 
				from Media.MediaMessage 
					where MediumId = @MediumId
						and MediaTypeId = @MediaTypeId
						and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) >= @StartDate
						and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) < @EndDate						
						and CodingPlausibilityId = @plausibilityUntouched)
			begin						
				update mm
				set Ready = 1,
					Modified = getdate(),
					ModifiedBy = @UserId
				from [Media].[MediaMessage] mm
					inner join [Creative].[Creative] c on [mm].[NormCreativeId] = [c].[Id]
				where mm.MediumId = @MediumId
					and mm.MediaTypeId = @MediaTypeId
					and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) >= @StartDate
					and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) < @EndDate					
					and (mm.PriceValue is not null 
						or (mm.PriceValue is null and mm.CodingPlausibilityId = @plausibilityNone)
						or (mm.PriceValue is NULL and @MediaTypeId = 2 AND mm.AdvertisementTypeId = 6)) 
					and mm.Ready = 0 
					and mm.CodingPlausibilityId != @plausibilityD
					and c.[MotiveId] is not null
					and mm.[PremiereMessageId] is not null;
			end;						


		-- prùbìžná dávka
		if @Period = 'Continuous'			
			and not exists(select 1 
				from Media.MediaMessage 
					where MediumId = @MediumId
						and MediaTypeId = @MediaTypeId
						and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) >= @StartDate
						and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) < @EndDate						
						and CodingPlausibilityId not in (@plausibilityNone, @plausibilitySure, @plausibilityD))	
			begin
				update mm
				set Ready = 1,
					Modified = getdate(),
					ModifiedBy = @UserId
				from [Media].[MediaMessage] mm
					inner join [Creative].[Creative] c on [mm].[NormCreativeId] = [c].[Id]
				where mm.MediumId = @MediumId
					and mm.MediaTypeId = @MediaTypeId
					and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) >= @StartDate
					and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) < @EndDate					
					and (mm.PriceValue is not null or (mm.PriceValue is null and mm.CodingPlausibilityId = @plausibilityNone))
					and mm.Ready = 0 
					and mm.CodingPlausibilityId != @plausibilityD
					and c.[MotiveId] is not null
					and mm.[PremiereMessageId] is not null;
			end;				
				

		-- parametr Force		
		if (@Force = 1 or @Period = 'Daily')
			update mm
			set Ready = 1,					
				Modified = getdate(),
				ModifiedBy = @UserId
			from Media.MediaMessage mm
				inner join [Creative].[Creative] c on [mm].[NormCreativeId] = [c].[Id]
			where [mm].MediumId = @MediumId
				and [mm].MediaTypeId = @MediaTypeId		
				and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) >= @StartDate
				and (case when datepart(ms, AdvertisedFrom) >= 500 then dateadd(ms, 1000-datepart(ms, AdvertisedFrom), AdvertisedFrom) else dateadd(ms, -datepart(ms, AdvertisedFrom), AdvertisedFrom) end) < @EndDate							
				and ([mm].PriceValue is not null 
					or ([mm].PriceValue is null and [mm].CodingPlausibilityId = @plausibilityNone)
					or @MediaTypeId in (2))				
				and [mm].Ready = 0 AND [mm].CodingPlausibilityId != @plausibilityD
				and c.[MotiveId] is not null
				and mm.[PremiereMessageId] is not null;

		commit transaction txAutoImport;
	end try
	begin catch
		rollback transaction txAutoImport;
	
		declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
		select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
		raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
	end catch 				
end;
GO

grant exec on [Media].[proc_AutoImporter_SetAttributes] to MediaDataBasicAccess;
-- [Media].[proc_AutoImporter_SetAttributes] @StartDate = '2009-07-31 00:00:00', @EndDate = '2009-07-31 23:59:59', @MediaTypeId = 1, @MediumId = null, @UserId = 1

  