use dbGallery
go
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Likes')	
	CREATE table Likes
	(
	 ImageId bigint NOT NULL,
	 UserId bigint NOT NULL,
	 CONSTRAINT [PK_Likes] PRIMARY KEY(UserId, ImageId)   
	)

go

