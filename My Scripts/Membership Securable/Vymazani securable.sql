

-- Vymaze securable s danym cislem a na nej navazane zalezitosti




DECLARE @SecurableIdToDelete int;
SELECT @SecurableIdToDelete =  (SELECT Id FROM Membership.Securable s WHERE s.Code like '%MergeProgram%')

--DELETE FROM Membership.UserGroupPermission WHERE Membership.UserGroupPermission.SecurableId = @SecurableIdToDelete
--DELETE FROM Membership.UserPermission WHERE Membership.UserPermission.SecurableId = @SecurableIdToDelete
--DELETE FROM Membership.SecurableToPermission WHERE [Membership].SecurableToPermission.SecurableId = @SecurableIdToDelete
--DELETE FROM Membership.[Securable] WHERE Membership.Securable.Id = @SecurableIdToDelete

SELECT 'Securable to delete je: ', @SecurableIdToDelete 
SELECT * FROM Membership.Securable s WHERE s.Id = @SecurableIdToDelete  -- vypis bude prazdny
