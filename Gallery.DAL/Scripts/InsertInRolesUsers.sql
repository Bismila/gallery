use dbGallery
go
insert into Roles(name) values ('admin')
insert into Roles(name) values ('moderator')
insert into Roles(name) values ('user')
go

insert into Users(Name, Login, Email, Password, PhotoUser, RoleId) values ('Tom', 'Tom', 'tom@gmail.com', '123456', 'url', '3')
insert into Users(Name, Login, Email, Password, PhotoUser, RoleId) values ('Max', 'Max', 'max@gmail.com', '123456', 'url', '3')
insert into Users(Name, Login, Email, Password, PhotoUser, RoleId) values ('Ben', 'Ben', 'ben@gmail.com', '123456', 'url', '3')
go
