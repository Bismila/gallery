use dbGallery

Alter Table Users
--add RoleId bigint null
alter column RoleId bigint null
go
Alter Table Users
add constraint FK_Users_RoleId FOREIGN KEY (RoleId)
REFERENCES dbo.Roles(Id) 
go
