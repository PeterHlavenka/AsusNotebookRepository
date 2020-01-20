 -- PREDELANI HOLUBCOVO SCRIPTU #51935

--Changescript instalujici danou sluzbu byl napsan spatne - jsou v nem natvrdo idecka typu komponent, instanci apod. Na jinych databazich (BG) to zpusobi problem. Je potreba jej predelat na automaticky. Tedy:
--vyhodit SET IDENTITY_INSERT
--pouzit normalni insert s identitou (vynechanim Id)
--vytvorit promenne pro zapamatovani si insertnutych ID
--naplnit je pres SCOPE_IDENTITY()
--pouzit tato ID ve vazbach

 SELECT * FROM Environment.ServiceType st
 SELECT * FROM Environment.ServiceComponentType sct
 SELECT * FROM Environment.ServiceInstance
 SELECT * FROM Environment.ServiceComponentInstance WHERE Environment.ServiceComponentInstance.Value01 in ('2000', '60000', 'D392988D-869D-43A9-B252-BD0F0FCD8BF9', '500')
 SELECT * FROM Environment.ServiceTypeComponentPermission WHERE Environment.ServiceTypeComponentPermission.ComponentName in ('pressRepricingJobBatch', 'pressRepricingJobPeriodMs', 'pressRepricingJobPriceListGuidString', 'pressRepricingJobStartMs')
 SELECT * FROM Environment.ServiceInstanceComponentRegistration WHERE Environment.ServiceInstanceComponentRegistration.ServiceInstanceId = 15

 SELECT * FROM Environment.ServiceComponentType sct

 -- ID CKA SE PRI NOVEM REVERTU MUZOU LISIT.

 --delete FROM Environment.ServiceType  WHERE Id = 2
 --DELETE FROM Environment.ServiceComponentType WHERE Environment.ServiceComponentType.Id = 13

 --DELETE FROM dbo.DbSchemaVersion WHERE dbo.DbSchemaVersion.DbSchemaVersion = 198
 --DELETE FROM Environment.ServiceInstance WHERE Id = 12
 --DELETE FROM Environment.ServiceComponentInstance WHERE Environment.ServiceComponentInstance.Value01 in ('2000', '60000', 'D392988D-869D-43A9-B252-BD0F0FCD8BF9', '500')
 --DELETE FROM  Environment.ServiceTypeComponentPermission WHERE Environment.ServiceTypeComponentPermission.ComponentName in ('pressRepricingJobBatch', 'pressRepricingJobPeriodMs', 'pressRepricingJobPriceListGuidString', 'pressRepricingJobStartMs')
 --DELETE FROM Environment.ServiceInstanceComponentRegistration WHERE Environment.ServiceInstanceComponentRegistration.ServiceInstanceId = 12