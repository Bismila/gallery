using System.Text;
using System.Threading.Tasks;
using System.Data;
using Moq;
using Gallery.DAL.IRepository;
using Gallery.BAL.Services;
using Gallery.BAL.DTO;
using Gallery.DAL.Models;
using Gallery.BAL.DTO.ImagesDto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Gallery.Tests.ServicesTests
{
    [TestClass]
    public class ImagesServicesTests
    {
        [TestMethod]
        public void TestMoqCreateImage()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);
            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var user = new User
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                //ConfirmPassword = "111111",
                RoleId = 3

            };
            List<CreateUpdateImageDto> dbImages = new List<CreateUpdateImageDto>()
            {
                new CreateUpdateImageDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                    UserName = "Max"
                }
            };
            var newImage = new CreateUpdateImageDto
            {
                Id = 2,
                Name = "image 02",
                ImageDate = System.DateTime.Now,
                PathImage = "blabla",
                UserId = 10,
                UserName = user.Name
            };
            dbImages.Add(newImage);

            mockUser.Setup(curUs => curUs.GetCurrentUser(newImage.UserName))
                 .Returns(user);

            mockImage.Setup(u => u.Create(new Image
            {
                Id = newImage.Id,
                Name = newImage.Name,
                ImageDate = newImage.ImageDate,
                PathImage = newImage.PathImage,
                UserId = newImage.UserId,
                userName = newImage.UserName
            }));

            mockImage.Setup(u => u.GetAllElements()).Returns(dbImages.Select(img => new Image
            {
                Id = img.Id,
                Name = img.Name,
                ImageDate = img.ImageDate,
                PathImage = img.PathImage,
                UserId = img.UserId,
                userName = img.UserName
            }));

            // Act
            UserDTO curUser = userService.GetCurrentUser(user.Login);

            imageService.Create(newImage);
            List<CreateUpdateDto> actualLisImages = imageService.GetAllElements().ToList();

            // Assert
            mockUser.Verify(u => u.GetCurrentUser(It.Is<string>(login => login == user.Login)), Times.AtLeastOnce);
            mockImage.Verify(i => i.Create(It.Is<Image>(t => t.Id == 2)), Times.Once);
            mockImage.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(dbImages.Count(), actualLisImages.Count());

            IEnumerator<CreateUpdateImageDto> listExp = dbImages.GetEnumerator();

            IEnumerator<CreateUpdateDto> listAct = actualLisImages.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.Id, listAct.Current.Id);
                Assert.AreEqual(listExp.Current.ImageDate, listAct.Current.ImageDate);
                Assert.AreEqual(listExp.Current.Name, listAct.Current.Name);
                Assert.AreEqual(listExp.Current.PathImage, listAct.Current.PathImage);
                Assert.AreEqual(listExp.Current.UserId, listAct.Current.UserId);
            }
        }

        [TestMethod]
        public void TestMoqUpdateImage()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);
            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var user = new User
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                //ConfirmPassword = "111111",
                RoleId = 3

            };
            List<CreateUpdateImageDto> dbImages = new List<CreateUpdateImageDto>()
            {
                new CreateUpdateImageDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                    UserName = "Max"
                },
                 new CreateUpdateImageDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                    UserName = "Max"
                }
        };
            var newImage = new CreateUpdateImageDto
            {
                Id = 2,
                Name = "new image 02",
                ImageDate = System.DateTime.Now,
                PathImage = "bla",
                UserId = 10,
                UserName = user.Name
            };
            for (int i = 0; i < dbImages.Count(); i++)
            {
                if (dbImages[i].Id == newImage.Id)
                {
                    dbImages[i] = newImage;
                }
            }

            mockUser.Setup(curUs => curUs.GetCurrentUser(newImage.UserName))
                 .Returns(user);

            mockImage.Setup(u => u.Update(new Image
            {
                Id = newImage.Id,
                Name = newImage.Name,
                ImageDate = newImage.ImageDate,
                PathImage = newImage.PathImage,
                UserId = newImage.UserId,
                userName = newImage.UserName
            }));

            mockImage.Setup(u => u.GetAllElements()).Returns(dbImages.Select(img => new Image
            {
                Id = img.Id,
                Name = img.Name,
                ImageDate = img.ImageDate,
                PathImage = img.PathImage,
                UserId = img.UserId,
                userName = img.UserName
            }));

            // Act
            UserDTO curUser = userService.GetCurrentUser(user.Login);
            imageService.Update(newImage);
            List<CreateUpdateDto> actualLisImages = imageService.GetAllElements().ToList();

            // Assert
            mockUser.Verify(u => u.GetCurrentUser(It.Is<string>(login => login == user.Login)), Times.AtLeastOnce);
            mockImage.Verify(i => i.Update(It.Is<Image>(t => t.Id == 2)), Times.Once);
            mockImage.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(dbImages.Count(), actualLisImages.Count());

            IEnumerator<CreateUpdateImageDto> listExp = dbImages.GetEnumerator();

            IEnumerator<CreateUpdateDto> listAct = actualLisImages.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.Id, listAct.Current.Id);
                Assert.AreEqual(listExp.Current.ImageDate, listAct.Current.ImageDate);
                Assert.AreEqual(listExp.Current.Name, listAct.Current.Name);
                Assert.AreEqual(listExp.Current.PathImage, listAct.Current.PathImage);
                Assert.AreEqual(listExp.Current.UserId, listAct.Current.UserId);
            }
        }

        [TestMethod]
        public void TestMoqDeleteImage()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);
            List<CreateUpdateDto> dbImages = new List<CreateUpdateDto>()
            {
                 new CreateUpdateDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 3,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "bla",
                    UserId = 10,
                }
            };
            var delImageId = 3;
            for (int i = 0; i < dbImages.Count(); i++)
            {
                if (dbImages[i].Id == delImageId)
                {
                    dbImages.RemoveAt(delImageId-1);
                }
            }

            mockImage.Setup(u => u.Delete(delImageId));

            mockImage.Setup(u => u.GetAllElements()).Returns(dbImages.Select(img => new Image
            {
                Id = img.Id,
                Name = img.Name,
                ImageDate = img.ImageDate,
                PathImage = img.PathImage,
                UserId = img.UserId,
            }));

            // Act
            imageService.Delete(delImageId);
            List<CreateUpdateDto> actualLisImages = imageService.GetAllElements().ToList();

            // Assert
            mockImage.Verify(i => i.Delete(It.Is<int>(id => id == delImageId)), Times.Once);
            mockImage.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(dbImages.Count(), actualLisImages.Count());

            IEnumerator<CreateUpdateDto> listExp = dbImages.GetEnumerator();

            IEnumerator<CreateUpdateDto> listAct = actualLisImages.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqGetAllElements()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();

            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);

            List<CreateUpdateDto> dbImages = new List<CreateUpdateDto>()
            {
                 new CreateUpdateDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 3,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "bla",
                    UserId = 10,
                }
            };

            mockImage.Setup(u => u.GetAllElements()).Returns(dbImages.Select(img => new Image
            {
                Id = img.Id,
                Name = img.Name,
                ImageDate = img.ImageDate,
                PathImage = img.PathImage,
                UserId = img.UserId,
            }));

            // Act
            List<CreateUpdateDto> actualLisImages = imageService.GetAllElements().ToList();

            // Assert
            mockImage.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(dbImages.Count(), actualLisImages.Count());

            IEnumerator<CreateUpdateDto> listExp = dbImages.GetEnumerator();

            IEnumerator<CreateUpdateDto> listAct = actualLisImages.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqGet()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);

            List<CreateUpdateDto> dbImages = new List<CreateUpdateDto>()
            {
                 new CreateUpdateDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateDto
                {
                    Id = 3,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "bla",
                    UserId = 10,
                }
            };
            var getElementById = 2;
            CreateUpdateDto findElement = new CreateUpdateDto();
            for (int i = 0; i < dbImages.Count(); i++)
            {
                if (dbImages[i].Id == getElementById)
                {
                    findElement = dbImages[getElementById - 1];
                }
            }

            mockImage.Setup(u => u.Get(getElementById)).Returns(
            new Image
            {
                Id = findElement.Id,
                Name = findElement.Name,
                ImageDate = findElement.ImageDate,
                PathImage = findElement.PathImage
            });

            // Act
            CreateUpdateDto actualElement = imageService.Get(getElementById);

            // Assert
            mockImage.Verify(actual => actual.Get(getElementById), Times.Once);

            Assert.AreEqual(findElement.ToString(), actualElement.ToString());
          
        }

        [TestMethod]
        public void TestMoqGetAllElementsFromUser()
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);
            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var user = new User
            {
                Id = 10,
                Name = "Max",
                Login = "Max",
                Email = "max@nik.ru",
                Password = "111111",
                RoleId = 3
            };
            List<CreateUpdateImageDto> dbImages = new List<CreateUpdateImageDto>()
            {
                new CreateUpdateImageDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                    UserName = "Max"
                },
                new CreateUpdateImageDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 8,
                    UserName = "Max"
                },
                new CreateUpdateImageDto
                {
                    Id = 3,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                    UserName = "Max"
                }
        };

            mockImage.Setup(u => u.GetAllElementsFromUser(user.Id)).Returns(dbImages.Select(img => new Image
            {
                Id = img.Id,
                Name = img.Name,
                ImageDate = img.ImageDate,
                PathImage = img.PathImage,
                UserId = img.UserId,
                userName = img.UserName
            }));

            // Act
            List<CreateUpdateImageDto> actualLisImages = imageService.GetAllElementsFromUser(user.Id).ToList();

            // Assert
            mockImage.Verify(actual => actual.GetAllElementsFromUser(user.Id), Times.Once);

            Assert.AreEqual(dbImages.Count(), actualLisImages.Count());

            IEnumerator<CreateUpdateImageDto> listExp = dbImages.GetEnumerator();

            IEnumerator<CreateUpdateImageDto> listAct = actualLisImages.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqGetOneImage() //CreateUpdateDto
        {
            // arrange
            var mockImage = new Mock<IImageRepository>();
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockLikes = new Mock<ILikeRepository>();
            var mockComments = new Mock<ICommentRepository>();

            var imageService = new ImageService(mockImage.Object, mockUser.Object, mockFriend.Object, mockLikes.Object, mockComments.Object);

            List<CreateUpdateImageDto> dbImages = new List<CreateUpdateImageDto>()
            {
                 new CreateUpdateImageDto
                {
                    Id = 1,
                    Name = "image 01",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateImageDto
                {
                    Id = 2,
                    Name = "image 02",
                    ImageDate = System.DateTime.Now,
                    PathImage = "blabla",
                    UserId = 10,
                },
                 new CreateUpdateImageDto
                {
                    Id = 3,
                    Name = "image 03",
                    ImageDate = System.DateTime.Now,
                    PathImage = "bla",
                    UserId = 10,
                }
            };
            var getElementById = 2;
            CreateUpdateImageDto findElement = new CreateUpdateImageDto();
            for (int i = 0; i < dbImages.Count(); i++)
            {
                if (dbImages[i].Id == getElementById)
                {
                    findElement = dbImages[getElementById - 1];
                }
            }

            mockImage.Setup(u => u.Get(getElementById)).Returns(
            new Image
            {
                Id = findElement.Id,
                Name = findElement.Name,
                ImageDate = findElement.ImageDate,
                PathImage = findElement.PathImage
            });

            // Act
            CreateUpdateImageDto actualElement = imageService.GetOneImage(getElementById);

            // Assert
            mockImage.Verify(actual => actual.Get(getElementById), Times.Once);

            Assert.AreEqual(findElement.ToString(), actualElement.ToString());


        }

    }






}

