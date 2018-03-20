use dbGallery
go
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Users')	
	CREATE table Users
	(
	 Id bigint NOT NULL IDENTITY(1,1) primary key,
	 Name nvarchar(50) NULL,
	 [Login] nvarchar(50) NOT NULL,
	 Email nvarchar(50) NOT NULL,
	 [Password] nvarchar(50) NOT NULL,
	 RoleId bigint NULL,
	 PhotoUser nvarchar(100) NULL,
	)

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Roles')	
	CREATE table Roles
	(
	 Id bigint NOT NULL IDENTITY(1,1) primary key,
	  Name nvarchar(50) NOT NULL,
	  --FOREIGN KEY (UserId) REFERENCES Users(Id)
	  --constraint AlbumsId FOREIGN KEY (AlbumsId) REFERENCES Users(Id) on delete cascade on update cascade,
	)

IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Images')	
	CREATE table Images
	(
	 Id bigint NOT NULL IDENTITY(1,1) primary key,
	 Name nvarchar(MAX) NOT NULL,
	 ImageDate datetime NOT NULL DEFAULT(GETDATE()),
	 PathImage nvarchar(100) NOT NULL,
	)


go
