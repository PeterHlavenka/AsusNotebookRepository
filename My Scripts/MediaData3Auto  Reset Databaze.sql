
-- USE master DROP DATABASE MediaData3Auto_ss
-- create snapshot MEDIADATA
USE master
CREATE DATABASE MediaData3Auto_ss ON
( NAME = N'MediaData', FILENAME = N'd:\SNAPSHOTS\MediaData3Auto_ss'  ), 
( NAME = N'DefaultCreativeData', FILENAME = N'd:\SNAPSHOTS\MediaData3Auto_ss_DefaultCreativeData' ), 
( NAME = N'VideoCreativeData', FILENAME = N'd:\SNAPSHOTS\MediaData3Auto_ss_VideoCreativeData' ),
( NAME = N'VideoCreativeData_2', FILENAME = N'd:\SNAPSHOTS\MediaData3Auto_ss_VideoCreativeData25' ),
( NAME = N'ftrow_ft', FILENAME = N'd:\SNAPSHOTS\MediaData3Auto_ss_ftrow_ft' )
AS SNAPSHOT OF MediaData3Auto;
GO

-- restore to snapshot
USE master
EXEC dbo.killusers 'MediaData3Auto'
EXEC dbo.killusers 'MediaData3Auto_ss'
RESTORE DATABASE MediaData3Auto from 
DATABASE_SNAPSHOT = 'MediaData3Auto_ss';



--Skript na DT db by mìl být jen: (tam je snapshotem vlastnì RO db):
USE master
EXEC dbo.killusers 'MediaData3DT'
RESTORE DATABASE MediaData3DT from 
DATABASE_SNAPSHOT = 'MediaData3RO';
GO


-- na Auto je potreba si na zacatku tydne vytvorit snapshot, ke kteremu se da vracet
-- na DT je snapshot a staci restornout 

-- Miroslav: 2.12.2019 ok, èus jen pro úplnost :
USE master 
exec master.[dbo].[RestoreDBFromSnapshot] @DB_NAME = 'MediaData3Auto' 

----------------------------------------------------------------------------------------
--1) nejprve odpalim na Stoupovi (protoze tam je RC databaze) pres master toto: 
USE master
exec master.[dbo].[RestoreDBFromSnapshot] @DB_NAME = 'MediaData3RC' 

-- 2) potom jdu na Alfreda, ktery vidi na RC databazi a tam pod masterem odpalim toto:
USE master
EXEC('
use MediaData3RC

update [dbo].[Params] set Value = ''itservices@mediaresearch.cz'' where value like ''%@%''
update [dbo].[Params] set Value = ''net.tcp://192.168.0.122:8001/VideoMatchingSortingService'' where name = ''PREPROCESSING_ENDPOINTS''
update [Environment].[ServiceComponentInstance] set [Value02] = ''stoupa'', [Value03]=''MediaData3RC'' where id in (8,9)

update [Environment].[ServiceComponentInstance] set [Value01] = 255 where id = 4

DROP SYNONYM [Source].[wanAgregationTargetGroup]
DROP SYNONYM [Source].[wanRule]
DROP SYNONYM [Source].[wanRuleSet]
DROP SYNONYM [Synonym].[Part]
DROP SYNONYM [Synonym].[Page]
drop synonym [Synonym].[Location]
create SYNONYM [Synonym].[Part] FOR [PrintStorageAuto].[dbo].[Part]
create SYNONYM [Synonym].[Page] FOR [PrintStorageAuto].[dbo].[Page]
CREATE SYNONYM [Synonym].[Location] FOR [Stoupa].[TvStorage2Auto].[dbo].[Location];
create synonym [Source].[wanAgregationTargetGroup] for [Stoupa].[PoolingAuto].[SimProducer].[wanAgregationTargetGroup]
create synonym [Source].[wanRule] for [Stoupa].[PoolingAuto].[SimProducer].[wanRule]
create synonym [Source].[wanRuleSet] for [Stoupa].[PoolingAuto].[SimProducer].[wanRuleSet]

grant select, insert, update, delete on [Synonym].[Location] to MediaDataBasicAccess;
grant select, insert, update, delete on [Synonym].[Page] to MediaDataBasicAccess;
grant select, insert, update, delete on [Synonym].[Part] to MediaDataBasicAccess;
grant select, insert, update, delete on [Source].[wanAgregationTargetGroup] to MediaDataBasicAccess;
grant select, insert, update, delete on [Source].[wanRule] to MediaDataBasicAccess;
grant select, insert, update, delete on [Source].[wanRuleSet] to MediaDataBasicAccess


if  exists (select * from sys.database_principals where name = N''MediaDataThumbnailUser'')
       drop user [MediaDataThumbnailUser];
create user [MediaDataThumbnailUser] for login [MediaDataThumbnailUser] with default_schema = [Media];
exec sp_addrolemember @rolename = N''MediaDataBasicAccess'', @membername = N''MediaDataThumbnailUser''

if  exists (select * from sys.database_principals where name = N''MediaDataPricingUser'')
       drop user MediaDataPricingUser;
create user MediaDataPricingUser for login MediaDataPricingUser with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataPricingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataChangingUser'')
       drop user MediaDataChangingUser;
create user MediaDataChangingUser for login MediaDataChangingUser with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataChangingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataExportCreativeUser'')
       drop user [MediaDataExportCreativeUser];
create user [MediaDataExportCreativeUser] for login [MediaDataExportCreativeUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataExportCreativeUser''

if  exists (select * from sys.database_principals where name = N''MediaDataCuttingUser'')
       drop user [MediaDataCuttingUser];
create user [MediaDataCuttingUser] for login [MediaDataCuttingUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataCuttingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataCutting2User'')
       drop user [MediaDataCutting2User];
create user [MediaDataCutting2User] for login [MediaDataCutting2User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataCutting2User''

if  exists (select * from sys.database_principals where name = N''PrintStorageScanningUser'')
       drop user [PrintStorageScanningUser];
create user [PrintStorageScanningUser] for login [PrintStorageScanningUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''PrintStorageScanningUser''

if  exists (select * from sys.database_principals where name = N''MediaDataAdminUser'')
       drop user [MediaDataAdminUser];
create user [MediaDataAdminUser] for login [MediaDataAdminUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataAdminUser''

if  exists (select * from sys.database_principals where name = N''MediaDataApprovingUser'')
       drop user [MediaDataApprovingUser];
create user [MediaDataApprovingUser] for login [MediaDataApprovingUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataApprovingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataCatchingUser'')
       drop user [MediaDataCatchingUser];
create user [MediaDataCatchingUser] for login [MediaDataCatchingUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataCatchingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataCoding2User'')
       drop user [MediaDataCoding2User];
create user [MediaDataCoding2User] for login [MediaDataCoding2User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataCoding2User''


if  exists (select * from sys.database_principals where name = N''MediaDataNormingUser'')
       drop user [MediaDataNormingUser];
create user [MediaDataNormingUser] for login [MediaDataNormingUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataNormingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataScheduleWinUser'')
       drop user [MediaDataScheduleWinUser];
create user [MediaDataScheduleWinUser] for login [MediaDataScheduleWinUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataScheduleWinUser''

if  exists (select * from sys.database_principals where name = N''MediaDataImportUser'')
       drop user [MediaDataImportUser];
create user [MediaDataImportUser] for login [MediaDataImportUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataImportUser''

if  exists (select * from sys.database_principals where name = N''MediaDataImportingUser'')
       drop user [MediaDataImportingUser];
create user [MediaDataImportingUser] for login [MediaDataImportingUser] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataImportingUser''

if  exists (select * from sys.database_principals where name = N''MediaDataCoding2User'')
       drop user [MediaDataCoding2User];
create user [MediaDataCoding2User] for login [MediaDataCoding2User] with default_schema = [dbo];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataCoding2User''

if  exists (select * from sys.database_principals where name = N''SimLogUser'')
       drop user [SimLogUser];
create user [SimLogUser] for login [SimLogUser] with default_schema = [dbo];
exec sp_addrolemember @rolename = N''MediaDataBasicAccess'', @membername = N''SimLogUser'';


if  exists (select * from sys.database_principals where name = N''MediaDataReprocessingUser'')
       drop user [MediaDataReprocessingUser];
create user [MediaDataReprocessingUser] for login [MediaDataReprocessingUser] with default_schema = [dbo];
exec sp_addrolemember @rolename = N''MediaDataBasicAccess'', @membername = N''MediaDataReprocessingUser'';


if  exists (select * from sys.database_principals where name = N''MediaDataMatching1User'')
       drop user [MediaDataMatching1User];
create user [MediaDataMatching1User] for login [MediaDataMatching1User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching1User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching2User'')
       drop user [MediaDataMatching2User];
create user [MediaDataMatching2User] for login [MediaDataMatching2User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching2User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching3User'')
       drop user [MediaDataMatching3User];
create user [MediaDataMatching3User] for login [MediaDataMatching3User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching3User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching4User'')
       drop user [MediaDataMatching4User];
create user [MediaDataMatching4User] for login [MediaDataMatching4User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching4User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching5User'')
       drop user [MediaDataMatching5User];
create user [MediaDataMatching5User] for login [MediaDataMatching5User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching5User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching6User'')
       drop user [MediaDataMatching6User];
create user [MediaDataMatching6User] for login [MediaDataMatching6User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching6User''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching7User'')
       drop user [MediaDataMatching7User];
create user [MediaDataMatching7User] for login [MediaDataMatching7User] with default_schema = [Media];
exec sp_addrolemember N''MediaDataBasicAccess'', N''MediaDataMatching7User''
       
if  exists (select * from sys.database_principals where name = N''MediaDataMatching1testUser'')
       drop user [MediaDataMatching1testUser];
create user [MediaDataMatching1testUser] for login [MediaDataMatching1testUser] with default_schema = [Media];
exec sp_addrolemember @rolename = N''MediaDataBasicAccess'', @membername = N''MediaDataMatching1testUser''

if  exists (select * from sys.database_principals where name = N''MediaDataMatching1rcUser'')
       drop user [MediaDataMatching1rcUser];
create user [MediaDataMatching1rcUser] for login [MediaDataMatching1rcUser] with default_schema = [Media];
exec sp_addrolemember @rolename = N''MediaDataBasicAccess'', @membername = N''MediaDataMatching1rcUser''

') AT [STOUPA\SQL2016]



