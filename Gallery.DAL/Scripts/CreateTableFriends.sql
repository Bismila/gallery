use dbGallery
go
IF NOT EXISTS (SELECT * FROM SYSOBJECTS WHERE NAME='Friends') 
 CREATE table Friends
 (
  UserId bigint NOT NULL,
  FriendId bigint NOT NULL,
  CONSTRAINT [PK_Friends] PRIMARY KEY(UserId, FriendId)   
 )

go
