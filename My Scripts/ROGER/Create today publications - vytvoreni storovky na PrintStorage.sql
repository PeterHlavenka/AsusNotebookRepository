USE [PrintStorage]
GO

/****** Object:  StoredProcedure [dbo].[proc_CreateTodayPublications]    Script Date: 14.04.2020 10:28:00 ******/
DROP PROCEDURE [dbo].[proc_CreateTodayPublications]
GO

/****** Object:  StoredProcedure [dbo].[proc_CreateTodayPublications]    Script Date: 14.04.2020 10:28:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[proc_CreateTodayPublications]
AS

BEGIN
	SET XACT_ABORT ON

	BEGIN TRAN
		INSERT INTO dbo.Publication (MediumId, Date, Year, Number, PublicationStatusId, Printing, ScheduleDate, Created, CreatedBy)
		SELECT MediumId, CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, GETDATE()))), YEAR(GETDATE()), 0, 1, 0, CONVERT(DATETIME, FLOOR(CONVERT(FLOAT, GETDATE()))), GETDATE(), 127
		FROM dbo.fn_GetDayPublications(GETDATE())
		
	--ROLLBACK
	COMMIT
END

GO


