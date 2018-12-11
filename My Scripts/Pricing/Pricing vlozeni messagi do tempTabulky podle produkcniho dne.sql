CREATE TABLE ##Pricing_TVMessageIds (Id INT NOT NULL, Ord INT NOT NULL IDENTITY (1,1))

declare @messageId int;
select @messageId = 9566145

declare @From datetime;
select @From = (select cast(AdvertisedFrom as date)  from media.MediaMessage where Id = @messageId)

declare @To datetime;
select @To = (select DATEADD(DAY, 1, cast( AdvertisedTo as date)) from media.MediaMessage where Id = @messageId)

INSERT INTO ##Pricing_TVMessageIds (Id) SELECT Id from media.MediaMessage where MediaTypeId = 2 and  AdvertisedFrom >=  @From  and AdvertisedTo <= @To

;
with ##Pricing_TVMessageIdsCTE AS
(
	Select *, ROW_NUMBER () Over(Partition by Id order by Id) as RowNumber
	from ##Pricing_TVMessageIds
)
Delete from ##Pricing_TVMessageIdsCTE where RowNumber > 1

select COUNT(Id) from ##Pricing_TVMessageIds

DROP TABLE ##Pricing_TVMessageIds