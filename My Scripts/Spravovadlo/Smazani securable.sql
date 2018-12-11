declare @Id int;
set @Id = (select Id from Membership.Securable where Code = 'AdvertisementTypeAdministrationViewModel')

delete from Membership.UserPermission where SecurableId = @Id
delete from Membership.SecurableToPermission where SecurableId = @Id
delete from Membership.Securable where Id = @Id

select @Id = (select Id from Membership.Securable where Code = 'DialAdministrationViewModel') 

delete from Membership.UserPermission where SecurableId = @Id
delete from Membership.SecurableToPermission where SecurableId = @Id
delete from Membership.Securable where Id = @Id