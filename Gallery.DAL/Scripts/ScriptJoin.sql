use dbGallery
select TOP 9 i.Id, i.UserId, u.Login, i.Name, i.PathImage, u.Id, u.Login
from Friends f
inner join Images i
on i.UserId = f.FriendId
inner join Users u
on u.Id = i.UserId
where f.UserId = 1
union 
select TOP 9 i.Id, i.UserId, u.Login, i.Name, i.PathImage, u.Id, u.Login
from Users u
inner join Images i
on i.UserId = u.Id
where u.Id = 1
order by i.ImageDate DESC

						

--select * 
--from Users 
--left JOIN Friends ON Friends.UserId = 10

--select *
--from Users u
--inner JOIN Friends f ON f.FriendId = u.Id
--where f.UserId = 10

--select * 
--from users u
--JOIN Friends f ON f.UserId = u.Id
--where u.Id=9

--select * 
--from Users
--where Users.Login like '%q'

--SELECT * FROM Users u INNER JOIN Friends f ON f.FriendId = u.id WHERE f.UserId = 9
--select * from Users left JOIN Friends ON Friends.UserId = Users.Id
------------------------------------------------------------------------
--select U.Id, U.Name, F.FriendId, F.UserId
--from Users U
--left JOIN Friends F
--ON U.Id = F.UserId 
 ------------------------------------------------------------------------
--SELECT Images.UserId, Users.Name, Images.Name, Images.PathImage 
--FROM Images
--INNER JOIN Users ON Images.UserId = Users.Id 
--INNER JOIN Friends ON Friends.FriendId = Images.UserId 
--where Friends.UserId = 9 
--union 
--SELECT  Images.UserId,Users.Name, Images.Name, Images.PathImage 
--FROM Images 
--INNER JOIN Users ON Images.UserId = 9 
--WHERE Users.Id=9
--------------------------------------------------------------------------
--select U.Id,U.Login, I.Name, I.UserId from Users U
--           JOIN Images I
--           ON I.UserId = 2
--------------------------------------------------------------------------
--SELECT TOP 6 i.Id AS Image_id, u.name, i.name, i.UserId AS user_id, i.ImageDate AS img_Date 
--FROM Images  i
--INNER JOIN Users u ON i.UserId = u.id
--WHERE i.UserId=u.Id
--ORDER BY i.Id DESC
------------------------------------------------------------------------
--INNER JOIN Friends AS f ON f.UserId = i.UserId

-----------------------------------------------------------------------
--select U.Id, U.Login, U.PhotoUser, I.Name as img_name, I.PathImage 
--from Images I
--JOIN Users U on U.Id = 11
--------------------------------------------------------------------------


--select DISTINCT *
--from Users
--INNER JOIN Friends ON Friends.UserId = 1
--------------------------------------------------------------------------
--SELECT * FROM Friends WHERE UserId = 1
------------------------------------------------------------------------
--DELETE FROM Friends WHERE FriendId = 8
--INNER JOIN Images i ON u.id = i.UserId
------------------------------------------------------------------------
--SELECT u.id, u.name, i.name, i.UserId AS img_name
--FROM Images i
--LEFT OUTER JOIN Users u ON i.UserId = u.id 
--WHERE i.UserId=2
------------------------------------------------------------------------