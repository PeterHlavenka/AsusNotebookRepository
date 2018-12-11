

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