use dbGallery

--Alter Table Images
--add UserId bigint null
----alter column UserId bigint null
go
Alter Table Images
add constraint FK_Images_UserId FOREIGN KEY (UserId)
REFERENCES dbo.Users(Id) 
on delete cascade on update cascade

go
