use dbGallery

Alter Table Likes add 
constraint FK_Images_Users FOREIGN KEY (UserId)
REFERENCES dbo.Users(Id) 

go
Alter Table Likes add 
constraint FK_2Images_Users FOREIGN KEY (ImageId)
REFERENCES dbo.Images(Id) 
on delete cascade on update cascade