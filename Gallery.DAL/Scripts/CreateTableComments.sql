use dbGallery
go
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Comments')	
	CREATE table Comments
	(
	 Id bigint NOT NULL IDENTITY(1,1),
	 UserId bigint NOT NULL,
	 ImageId bigint NOT NULL,
	 [Text] nvarchar(MAX) NOT NULL,
	 CommentData datetime NOT NULL,
	 ParentId bigint NULL,
	 CONSTRAINT [PK_Comments] PRIMARY KEY(Id),  
	 FOREIGN KEY (UserId) REFERENCES Users(Id),
	 FOREIGN KEY (ImageId) REFERENCES Images(Id) on delete cascade on update cascade
	)

go

