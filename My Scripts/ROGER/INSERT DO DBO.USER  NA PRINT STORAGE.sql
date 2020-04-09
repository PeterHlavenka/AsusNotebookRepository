/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Login]
      ,[UserName]
      ,[MediaDataVersion]
  FROM [PrintStorage].[dbo].[User] WHERE login = 'jh30-pc\admin'

  --UPDATE [PrintStorage].[dbo].[User] SET Login = 'jh30-pc\admin256' WHERE login = 'jh30-pc\admin'
  --UPDATE [PrintStorage].[dbo].[User] SET Login = 'pha0123\admin257' WHERE login = 'pha0123\admin'
  INSERT INTO dbo.[User]
  (
      Id,
      Login,
      UserName,
      MediaDataVersion
  )
  VALUES
  (   171,  -- Id - smallint
      'jh30-pc\admin', -- Login - varchar(50)
      'jh30-pc\admin', -- UserName - varchar(100)
      2673646430   -- MediaDataVersion - bigint
      )

	    INSERT INTO dbo.[User]
  (
      Id,
      Login,
      UserName,
      MediaDataVersion
  )
  VALUES
  (   172,  -- Id - smallint
      'pha0123\admin', -- Login - varchar(50)
      'pha0123\admin', -- UserName - varchar(100)
      2673646430   -- MediaDataVersion - bigint
      )