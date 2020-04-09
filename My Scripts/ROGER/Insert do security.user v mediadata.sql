/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[Login]
      ,[UserName]
      ,[Version]
      ,[UserTypeId]
  FROM [MediaData3].[Security].[User]

  INSERT INTO Security.[User]
  (
      Id,
      Login,
      UserName,
      UserTypeId
  )
  VALUES
  (   260,  -- Id - smallint
      'jh04-pc\admin', -- Login - varchar(50)
      'jh04-pc\admin', -- UserName - varchar(100)
      1   -- UserTypeId - tinyint
      )