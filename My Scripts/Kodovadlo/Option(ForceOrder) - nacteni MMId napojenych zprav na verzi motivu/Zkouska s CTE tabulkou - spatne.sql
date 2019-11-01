
DECLARE @motiveVersionId int ;
SELECT @motiveVersionId = 306149

;
WITH neco_cte (NormCreativeId)
AS
(
	SELECT
	[message].[NormCreativeId]
	FROM
	[Media].[MediaMessage] [message]
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
	WHERE
	[motiveVersion].[Id] = @motiveVersionId AND [motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom]
	GROUP BY
	[message].[NormCreativeId]
)
--SELECT * FROM neco_cte

SELECT Min([message].[Id])   -- blbost, dostanu jen nejmensi Id ze vsech musel bych jeste groupovat.
FROM neco_cte cte
		inner join [Media].[MediaMessage] [message] on cte.NormCreativeId = message.NormCreativeId	
		INNER JOIN [Creative].[Creative] [creative] ON [message].[NormCreativeId] = [creative].[Id]
		INNER JOIN [Media].[MotiveVersion] [motiveVersion] ON [creative].[MotiveId] = [motiveVersion].[MotiveId]
		
WHERE
	[motiveVersion].[Id] = @motiveVersionId AND
	[motiveVersion].[ActiveFrom] <= [message].[AdvertisedFrom] AND
	[motiveVersion].[ActiveTo] > [message].[AdvertisedFrom] 
	option(force order)
