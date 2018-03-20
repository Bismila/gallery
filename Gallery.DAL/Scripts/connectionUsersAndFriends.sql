use dbGallery

Alter Table Friends add 
constraint FK_Friends_Users FOREIGN KEY (UserId)
REFERENCES dbo.Users(Id) 

go
Alter Table Friends add 
constraint FK_2Friends_Users FOREIGN KEY (FriendId)
REFERENCES dbo.Users(Id) 
--on delete cascade on update cascade

