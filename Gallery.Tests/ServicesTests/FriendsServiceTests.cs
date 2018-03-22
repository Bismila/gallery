using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Moq;
using Gallery.DAL.IRepository;
using Gallery.BAL.Services;
using Gallery.BAL.DTO;
using Gallery.DAL.Models;
using Gallery.BAL.DTO.ImagesDto;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.Tests.ServicesTests
{
    [TestClass]
    public class FriendsServiceTests
    {
        [TestMethod]
        public void TestMoqAddFriend()
        {
            // arrange
            var mockFriend = new Mock<IFriendRepository>();
            var mockImage = new Mock<IImageRepository>();

            var friendService = new FriendService(mockFriend.Object, mockImage.Object);

            var currentUser = new UserDTO
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                RoleId = 3
            };

            var friends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 1
                }
            };
            var addNewFriend = new FriendDTO
            {
                UserId = 10,
                FriendId = 2
            };
            friends.Add(addNewFriend);

            mockFriend.Setup(f => f.AddFriend(currentUser.Id, addNewFriend.FriendId));
            mockFriend.Setup(frnds => frnds.GetAllFriend(currentUser.Id)).Returns(friends.Select(x=>new Friend
            {
                UserId = x.UserId,
                FriendId = x.FriendId
            }));

            // Act
            friendService.AddFriendService(currentUser.Id, addNewFriend.FriendId);
            List<FriendDTO> actualLisFriends = friendService.GetAllFriendsService(currentUser.Id).ToList();

            // Assert
            mockFriend.Verify(f => f.AddFriend(It.Is<long>(us => us == addNewFriend.UserId),
                                               It.Is<long>(fr => fr == addNewFriend.FriendId)), Times.Once);

            mockFriend.Verify(f => f.GetAllFriend(It.Is<long>(curUser => curUser == currentUser.Id)), Times.AtLeastOnce);

            Assert.AreEqual(friends.Count, actualLisFriends.Count);

            IEnumerator<FriendDTO> listExp = friends.GetEnumerator();

            IEnumerator<FriendDTO> listAct = actualLisFriends.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestMoqDelFriend()
        {
            // arrange
            var mockFriend = new Mock<IFriendRepository>();
            var mockImage = new Mock<IImageRepository>();

            var friendService = new FriendService(mockFriend.Object, mockImage.Object);

            var currentUser = new UserDTO
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                RoleId = 3
            };
            var dbFriends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 1
                },
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 2
                },
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 3
                }
            };

            var delFriendId = 2;
            for (int i = 0; i < dbFriends.Count; i++)
            {
                if(dbFriends[i].FriendId == delFriendId)
                {
                    dbFriends.RemoveAt(delFriendId-1);
                }
            }
            mockFriend.Setup(f => f.DelFriend(delFriendId));
            mockFriend.Setup(frnds => frnds.GetAllFriend(currentUser.Id)).Returns(dbFriends.Select(x => new Friend
            {
                UserId = x.UserId,
                FriendId = x.FriendId
            }));
            // Act
            friendService.DelFriendService(delFriendId);
            
            List<FriendDTO> actualLisFriends = friendService.GetAllFriendsService(currentUser.Id).ToList();

            // Assert
            mockFriend.Verify(f => f.DelFriend(It.Is<long>(us => us == delFriendId)), Times.Once);

            mockFriend.Verify(f => f.GetAllFriend(It.Is<long>(curUser => curUser == currentUser.Id)), Times.AtLeastOnce);

            Assert.AreEqual(dbFriends.Count, actualLisFriends.Count);

            IEnumerator<FriendDTO> listExp = dbFriends.GetEnumerator();

            IEnumerator<FriendDTO> listAct = actualLisFriends.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestMoqGetAllFriends()
        {
            // arrange
            var mockFriend = new Mock<IFriendRepository>();
            var mockImage = new Mock<IImageRepository>();

            var friendService = new FriendService(mockFriend.Object, mockImage.Object);

            var currentUser = new UserDTO
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                RoleId = 3
            };
            var dbFriends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 1
                },
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 2
                },
                new FriendDTO
                {
                    UserId = 10,
                    FriendId = 3
                }
            };

            mockFriend.Setup(frnds => frnds.GetAllFriend(currentUser.Id)).Returns(dbFriends.Select(x => new Friend
            {
                UserId = x.UserId,
                FriendId = x.FriendId
            }));

            // Act
            List<FriendDTO> actualLisFriends = friendService.GetAllFriendsService(currentUser.Id).ToList();

            // Assert
            mockFriend.Verify(f => f.GetAllFriend(It.Is<long>(curUser => curUser == currentUser.Id)), Times.AtLeastOnce);

            Assert.AreEqual(dbFriends.Count, actualLisFriends.Count);

            IEnumerator<FriendDTO> listExp = dbFriends.GetEnumerator();

            IEnumerator<FriendDTO> listAct = actualLisFriends.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqGetAllImageFriends()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var friendService = new FriendService(mockFriend.Object, mockImage.Object);

            List<CreateUpdateDto> listImages = new List<CreateUpdateDto>
            {
                new CreateUpdateDto
                {
                    Id = 10011,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 1
                },
                 new CreateUpdateDto
                {
                    Id = 10012,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 2,
                },
                 new CreateUpdateDto
                {
                    Id = 10013,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 2,
                },
                 new CreateUpdateDto
                {
                    Id = 10014,
                    Name = "image 04",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 3,
                }
             
            };
            
            var dbFriends = new List<Friend>
            {
                new Friend
                {
                    UserId = 10,
                    FriendId = 1
                },
                new Friend
                {
                    UserId = 10,
                    FriendId = 2
                },
                new Friend
                {
                    UserId = 10,
                    FriendId = 3
                }
            };
            UserDTO currentUser = new UserDTO
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                RoleId = 3,
                Friends = dbFriends
            };

            mockImage.Setup(frnds => frnds.GetAllImagesFromFriends(currentUser.Id)).Returns(listImages.Select(i=> new Image
            {
                Id = i.Id,
                Name = i.Name,
                ImageDate = i.ImageDate,
                PathImage = i.PathImage,
                UserId = i.UserId,
                User = i.User
            }));
            // Act
            List<CreateUpdateDto> actualLisImagesFromFriends = friendService.GetAllImageFriends(currentUser.Id).ToList();

            // Assert
            mockImage.Verify(i => i.GetAllImagesFromFriends(It.Is<long>(curUser => curUser == currentUser.Id)), Times.AtLeastOnce);

            Assert.AreEqual(listImages.Count, actualLisImagesFromFriends.Count);

            IEnumerator<CreateUpdateDto> listExp = listImages.GetEnumerator();

            IEnumerator<CreateUpdateDto> listAct = actualLisImagesFromFriends.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }


    }
}
