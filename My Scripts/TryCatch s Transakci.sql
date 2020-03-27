
set transaction isolation level serializable;

begin transaction MyTransaction;

	BEGIN try
	

	commit transaction MyTransaction;
	
end try
begin catch
	rollback transaction MyTransaction;

	declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
	select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
end catch

GO

