

set transaction isolation level READ COMMITTED;

begin transaction MyTransaction;

	BEGIN try
	

			USE [MediaData3]			
			
			DELETE FROM SimLog.AllowedAttributeToPrgTypeCombination
			DELETE FROM simlog.ProgrammeTranslation
			DELETE FROM SimLog.Programme
			DELETE FROM SimLog.PrgType

			INSERT INTO MediaData3.SimLog.PrgType
			(
				Id,
				cs,
				en,
				local,
				ExternalId,
				MinLen,
				MaxLen,
				CreatedBy,
				Created,
				ModifiedBy,
				Modified,
				Code
			)
			SELECT 				
				pt.Id,
				pt.cs,
				pt.en,
				pt.local,
				pt.ExternalId,
				pt.MinLen,
				pt.MaxLen,
				pt.CreatedBy,
				pt.Created,
				pt.ModifiedBy,
				pt.Modified,
				pt.Code FROM [STOUPA\SQL2016].[MediaData3RC].[SimLog].[PrgType] pt

			UPDATE SimLog.PrgType SET MaxLen = 86400  WHERE MaxLen = 0

	commit transaction MyTransaction;
	
end try
begin catch
	rollback transaction MyTransaction;
	PRINT 'rollbacking'
	declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
	select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
end catch

GO



--SELECT TOP (100) * FROM SimLog.PrgType