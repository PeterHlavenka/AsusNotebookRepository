-- Script mi prida securable a permission (jiz existujici) pro normovadlo 

  USE [MediaData3BG_KOMPOZITY]
GO

INSERT INTO [Membership].[UserPermission]
           ([UserId]
           ,[PermissionId]
           ,[SecurableId]
           ,[ApplicationId]
           ,[ValidFrom]
           ,[ValidTo])
     VALUES
           (135  -- moje id
           ,1   -- permissionId
           ,53 -- 54 --55  zalozka porovnani zachytu a tvlogu
           ,2 -- Normovadlo
           ,'1900-01-01 00:00:00.000'
           ,'2100-01-01 00:00:00.000')
GO