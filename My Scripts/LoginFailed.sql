if  exists (select * from sys.database_principals where name = N'MediaDataAdminUser')
       drop user MediaDataAdminUser;
create user MediaDataAdminUser for login MediaDataAdminUser with default_schema = [Media];
exec sp_addrolemember N'MediaDataBasicAccess', N'MediaDataAdminUser' 
