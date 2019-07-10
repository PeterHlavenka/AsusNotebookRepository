/****** Script for SelectTopNRows command from SSMS  ******/

DECLARE @mediumId int;
DECLARE @dayFrom datetime;
DECLARE @dayTo datetime;
SET @dayFrom = '20190705 05:59:00'
SET @dayTo = '20190706 06:00:00'
SET @mediumId = 26  -- 24 Kitchen

--Delete
SELECT *
  FROM [MediaData3BGRC].[SimLog].[BroadcastingDescription] WHERE SimLog.BroadcastingDescription.DateTimeFrom >=  @dayFrom 
  AND SimLog.BroadcastingDescription.DateTimeTo <= @dayTo
  AND SimLog.BroadcastingDescription.MediumId = @mediumId
  
  ORDER BY SimLog.BroadcastingDescription.DateTimeFrom  
