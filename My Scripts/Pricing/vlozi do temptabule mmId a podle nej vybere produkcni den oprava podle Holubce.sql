-- Pricing.TvMediaMessageDao.SelectByParameter<List<int> messageId
-- Vytvori temp tabulku do ktere vlozi jedno Id, (pripadne se to da prepsat na vice Id), odstrani z temp tabulky duplicity, a podle vlozene message najde produkcni den 06:00 - 06:00 nasl.dne

CREATE TABLE ##Pricing_TVMessageIds (Id INT NOT NULL, Ord INT NOT NULL IDENTITY (1,1))

INSERT INTO ##Pricing_TVMessageIds (Id) SELECT Id from media.MediaMessage mm where mm.Id in(93245853,93245230, 93246031,93230375 -- vrati dva produkcni dny pro dve media, tedy ctyri radky
)
;   --strednik nutny

-- Odstraneni duplicit z tempTabulky
with ##Pricing_TVMessageIdsCTE AS
(
	Select *, ROW_NUMBER () Over(Partition by Id order by Id) as RowNumber
	from ##Pricing_TVMessageIds
)
Delete from ##Pricing_TVMessageIdsCTE where RowNumber > 1



--select Id from ##Pricing_TVMessageIds     -- vypise Id ktere jsou v temp


declare @broadcastingStart int;
set @broadcastingStart =  360
declare @startHour int;
set @startHour = DATEPART(HOUR, @broadcastingStart)
declare @startMinute int;
set @startMinute = DATEPART(Minute, @broadcastingStart)
declare @startSecond int;
set @startSecond = DATEPART(SECOND, @broadcastingStart)



--select 
--DATEDIFF(MILLISECOND, CAST(FLOOR(CAST(mm.AdvertisedFrom as float)) AS datetime), mm.AdvertisedFrom) AS 'PRVNI'
--,@broadcastingStart * 60000 AS 'DRUHY' 
--from media.MediaMessage mm
-- join ##Pricing_TVMessageIds temp on mm.Id = temp.Id

 
		            select distinct  (CASE
							            WHEN 
							            DATEDIFF(MILLISECOND, CAST(FLOOR(CAST(mm.AdvertisedFrom as float)) AS datetime), mm.AdvertisedFrom) < @broadcastingStart * 60 * 1000     
							            THEN 
							            DATEADD(MINUTE, @broadcastingStart, DATEADD(DAY, -1, CAST(FLOOR(CAST(mm.AdvertisedFrom AS float)) AS datetime)))   
							            ELSE 
							            DATEADD(MINUTE, @broadcastingStart, CAST(FLOOR(CAST(mm.AdvertisedFrom as float)) AS datetime))   
						            END) as 'AdvertisedFrom', 
									
									(CASE
							            WHEN 
							            DATEDIFF(MILLISECOND, CAST(FLOOR(CAST(mm.AdvertisedFrom as float)) AS datetime), mm.AdvertisedFrom) < @broadcastingStart * 60 * 1000      
							            THEN 
										DATEADD(MINUTE, @broadcastingStart, CAST(FLOOR(CAST(mm.AdvertisedFrom as float)) AS datetime))   							            
							            ELSE 
							            DATEADD(MINUTE, @broadcastingStart, DATEADD(DAY, 1, CAST(FLOOR(CAST(mm.AdvertisedFrom AS float)) AS datetime)))   
						            END) as 'AdvertisedTo', 

									mm.MediumId as 'MediumId'
	            from media.MediaMessage mm
	            join ##Pricing_TVMessageIds temp on mm.Id = temp.Id         -- join na temp aby se ten dota tykal jen Id ktere jsou v temptabuli
				

DROP TABLE ##Pricing_TVMessageIds


select AdvertisedFrom, AdvertisedTo, MediumId from media.MediaMessage where Id in(93245853, 93246031,93230375, 93245230)