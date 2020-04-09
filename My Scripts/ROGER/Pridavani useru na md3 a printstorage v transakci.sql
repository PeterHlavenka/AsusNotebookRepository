
--SELECT * FROM [MediaData3].[Security].[User] WHERE Id > 170
--SELECT TOP (100) * FROM PrintStorage.dbo.[User] AS u WHERE Id > 170
  


	  --
BEGIN TRANSACTION MyTransaction;
	BEGIN TRY
	DECLARE @name nvarchar (300);
	DECLARE @id int;

	SELECT @id = 183
	SELECT @name = 'jh33-pc\admin'

	  INSERT INTO MediaData3.Security.[User]
  (
      Id,
      Login,
      UserName,
      UserTypeId
  )
  VALUES
  (   @id,  -- Id - smallint
      @name, -- Login - varchar(50)
      @name, -- UserName - varchar(100)
      1   -- UserTypeId - tinyint
   )
			
	INSERT INTO PrintStorage.dbo.[User]
	(
	    Id,
	    Login,
	    UserName,
	    MediaDataVersion
	)
	VALUES
	(   @id,  -- Id - smallint
	    @name, -- Login - varchar(50)
	    @name, -- UserName - varchar(100)
	    2673646430   -- MediaDataVersion - bigint
	)		

	commit transaction MyTransaction;	
end try
begin catch
	rollback transaction MyTransaction;	
	declare @ErrorMessage nvarchar (4000), @ErrorSeverity int, @ErrorState int;
	select @ErrorMessage = ERROR_MESSAGE (), @ErrorSeverity = ERROR_SEVERITY (), @ErrorState = ERROR_STATE ();
	raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState);
end catch
