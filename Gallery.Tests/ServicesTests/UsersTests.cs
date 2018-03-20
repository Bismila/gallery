using System;
using System.Collections.Generic;
using Gallery.BAL.DTO;
using Gallery.BAL.Services;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Gallery.Tests.ServicesTests
{
    [TestClass]
    public class UsersTests
    {
        [TestMethod]
        public void TestCreateUser()
        {
            // arrange
            var mock = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mock.Object, mockFriend.Object, mockRoles.Object);

            var dbUsers = new List<UserDTO>();
            var newUser = new UserDTO
            {
                Id = 1,
                Name = "Nik",
                Login = "Nik",
                Email = "nik@nik.ru",
                Password = "111111",
                RoleId = 3
            };

            dbUsers.Add(newUser);

            mock.Setup(u => u.Create(new User
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Login = newUser.Login,
                Email = newUser.Email,
                Password = newUser.Password,
                RoleId = newUser.RoleId
            }));

            mock.Setup(u => u.GetAllElements()).Returns(dbUsers.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId
            }));

            // Act
            userService.Create(newUser);
            var actualListUsers = userService.GetAllElements().ToList();

            // Assert
            mock.Verify(u => u.Create(It.Is<User>(t => t.Id == 1)), Times.Once);
            mock.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(dbUsers.Count(), actualListUsers.Count());

            IEnumerator<UserDTO> listExp = dbUsers.GetEnumerator();
            IEnumerator<UserDTO> listAct = actualListUsers.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestGetAllUsers()
        {
            var mock = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mock.Object, mockFriend.Object, mockRoles.Object);

            var expectedUsers = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            mock.Setup(u => u.GetAllElements()).Returns(expectedUsers.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId
            }));

            // Act
            var actualUsers = userService.GetAllElements().ToList();


            // Assert
            mock.Verify(u => u.GetAllElements(), Times.Once);

            Assert.AreEqual(expectedUsers.Count(), actualUsers.Count());

            IEnumerator<UserDTO> expextedListUsers = expectedUsers.GetEnumerator();
            IEnumerator<UserDTO> actualListUsers = actualUsers.GetEnumerator();

            while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestDeleteUser()
        {
            //arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var expectedLUser = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var elementId = 3;
            expectedLUser.RemoveAt(elementId - 1);

            mockUser.Setup(u => u.Delete(elementId));
            mockUser.Setup(u => u.GetAllElements()).Returns(expectedLUser.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Password = x.Password,
                Email = x.Email,
                RoleId = x.RoleId
            }));

            //act
            userService.Delete(elementId);
            var actualLUsers = userService.GetAllElements().ToList();

            //assert
            mockUser.Verify(u => u.Delete(It.Is<int>(id => id == elementId)), Times.Once);
            mockUser.Verify(u => u.GetAllElements(), Times.Once);

            Assert.AreEqual(expectedLUser.Count(), actualLUsers.Count());

            IEnumerator<UserDTO> expectedListUsers = expectedLUser.GetEnumerator();
            IEnumerator<UserDTO> actualListUsers = actualLUsers.GetEnumerator();

            while (expectedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expectedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestUpdateUser()
        {
            //arrange
           var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var expectedLUser = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var newUser = new UserDTO
            {
                Id = 3,
                Name = "Ben",
                Login = "Ben",
                Email = "ben@ben.ru",
                Password = "111111",
                RoleId = 3
            };

            expectedLUser.RemoveAt((int)newUser.Id - 1);
            expectedLUser.Insert((int)newUser.Id - 1, newUser);

            mockUser.Setup(u => u.Update(new User
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Login = newUser.Login,
                Password = newUser.Password,
                Email = newUser.Email,
                RoleId = newUser.RoleId
            }));

            mockUser.Setup(u => u.GetAllElements()).Returns(expectedLUser.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Password = x.Password,
                Email = x.Email,
                RoleId = x.RoleId
            }));

            // act
            userService.Update(newUser);
            var actualLUsers = userService.GetAllElements().ToList();

            //assert
            mockUser.Verify(u => u.Update(It.Is<User>(t => t.Id == newUser.Id)), Times.Once);
            mockUser.Verify(u => u.GetAllElements(), Times.Once);

            Assert.AreEqual(expectedLUser.Count(), actualLUsers.Count());

            IEnumerator<UserDTO> expectedListUsers = expectedLUser.GetEnumerator();
            IEnumerator<UserDTO> actualListUsers = actualLUsers.GetEnumerator();

            while (expectedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expectedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestGetUserById()
        {
            //arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object); 

            var expectedLUser = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var searchId = 2;
            var expUser = new UserDTO();
            foreach (var us in expectedLUser)
            {
                if (us.Id == searchId)
                {
                    expUser.Id = us.Id;
                    expUser.Name = us.Name;
                    expUser.Login = us.Login;
                    expUser.Email = us.Email;
                    expUser.Password = us.Password;
                    expUser.RoleId = us.RoleId;
                    break;
                }
                else
                {
                    expUser = new UserDTO();
                }
            }

            mockUser.Setup(u => u.Get(searchId)).Returns(new User
            {
                Id = expectedLUser[searchId - 1].Id,
                Name = expectedLUser[searchId - 1].Name,
                Login = expectedLUser[searchId - 1].Login,
                Password = expectedLUser[searchId - 1].Password,
                Email = expectedLUser[searchId - 1].Email,
                RoleId = expectedLUser[searchId - 1].RoleId
            });

            mockUser.Setup(u => u.GetAllElements()).Returns(expectedLUser.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Password = x.Password,
                Email = x.Email,
                RoleId = x.RoleId
            }));

            //act
            var actualLUsers = userService.GetAllElements().ToList();
            UserDTO actualUser = userService.Get(searchId);

            //assert
            mockUser.Verify(u => u.Get(It.Is<int>(id => id == searchId)), Times.Once);
            mockUser.Verify(u => u.GetAllElements(), Times.Once);

            var check = String.Compare(actualUser.ToString(), expUser.ToString());
            Assert.AreEqual(actualUser.ToString(), expUser.ToString());

        }

        [TestMethod]
        public void TestGetCurrentUserByName()
        {
            //arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var expectedLUser = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var searchUserName = "Sam";
            var expUser = new UserDTO();
            foreach (var us in expectedLUser)
            {
                if (us.Login == searchUserName)
                {
                    expUser.Id = us.Id;
                    expUser.Name = us.Name;
                    expUser.Login = us.Login;
                    expUser.Email = us.Email;
                    expUser.Password = us.Password;
                    expUser.RoleId = us.RoleId;
                    break;
                }
                else
                {
                    expUser = new UserDTO();
                }
            }

            mockUser.Setup(u => u.GetCurrentUser(searchUserName)).Returns(new User
            {
                Id = expUser.Id,
                Name = expUser.Name,
                Login = expUser.Login,
                Password = expUser.Password,
                Email = expUser.Email,
                RoleId = expUser.RoleId
            });

            //act
            UserDTO actualUser = userService.GetCurrentUser(searchUserName);

            //assert
            mockUser.Verify(u => u.GetCurrentUser(It.Is<string>(Login => Login == searchUserName)), Times.Once);

            var check = String.Compare(actualUser.ToString(), expUser.ToString());
            Assert.AreEqual(actualUser.ToString(), expUser.ToString());
        }

        [TestMethod]
        public void TestSearchFriends()
        {
            // arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();
            var mockImage = new Mock<IImageRepository>();

            var friendService = new FriendService(mockFriend.Object, mockImage.Object);
            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
    

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

            var dbUsers = new List<UserDTO>
            {
                 new UserDTO
                {
                    Id = 10,
                    Name = "Lisa",
                    Login = "Lisa",
                    Email = "lisa@nik.ru",
                    Password = "111111",
                    RoleId = 3,
                    Friends = dbFriends
                },
                   new UserDTO
                {
                    Id = 1,
                    Name = "Sara",
                    Login = "Sara",
                    Email = "sara@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Sasha",
                    Login = "Sasha",
                    Email = "sasha@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                 new UserDTO
                {
                    Id = 4,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@nik.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var searchLogin = "sa";
            var listSearchedFriends = new List<UserDTO>();

            for (int i = 0; i < dbUsers.Count(); i++)
            {
                if (dbUsers[i].Login.ToLower().Contains(searchLogin.ToLower()))
                {
                    for (int j = 0; j < dbFriends.Count(); j++)
                    {
                        if (dbUsers[i].Id == dbFriends[j].FriendId)
                        {
                            listSearchedFriends.Add(dbUsers[i]);
                        }
                    }

                }
            }

            mockUser.Setup(u => u.SearchFriends(searchLogin)).Returns(dbUsers.Select(founded => new User
            {
                Id = founded.Id,
                Name = founded.Name,
                Login = founded.Login,
                Email = founded.Email,
                RoleId = founded.RoleId,
                Friends = founded.Friends
            }));

            mockUser.Setup(u => u.GetAllElements()).Returns(listSearchedFriends.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId
            }));

            // Act
            userService.SearchFriends(searchLogin);
            var actualListUsers = userService.GetAllElements().ToList();

            // Assert
            mockUser.Verify(u => u.SearchFriends(It.Is<string>(s => s == searchLogin)), Times.Once);
            mockUser.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(listSearchedFriends.Count(), actualListUsers.Count());

            IEnumerator<UserDTO> listExp = listSearchedFriends.GetEnumerator();
            IEnumerator<UserDTO> listAct = actualListUsers.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestSearchUsers()
        {
            // arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
         
            var dbUsers = new List<UserDTO>
            {
                 new UserDTO
                {
                    Id = 1,
                    Name = "Lisa",
                    Login = "Lisa",
                    Email = "lisa@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                 new UserDTO
                {
                    Id = 4,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@nik.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };
            var searchLogin = "sam";
            var listSearchedUsers = new List<UserDTO>();
            for (int i = 0; i < dbUsers.Count(); i++)
            {
                if (dbUsers[i].Login.ToLower().Contains(searchLogin.ToLower()))
                {
                    listSearchedUsers.Add(dbUsers[i]);
                }
            }

            mockUser.Setup(u => u.SearchUsers(searchLogin)).Returns(listSearchedUsers.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId
            }));

            mockUser.Setup(u => u.GetAllElements()).Returns(listSearchedUsers.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId
            }));

            // Act
            userService.SearchUsers(searchLogin);
            var actualListUsers = userService.GetAllElements().ToList();

            // Assert
            mockUser.Verify(u => u.SearchUsers(It.Is<string>(s => s == searchLogin)), Times.Once);
            mockUser.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(listSearchedUsers.Count(), actualListUsers.Count());

            IEnumerator<UserDTO> listExp = listSearchedUsers.GetEnumerator();
            IEnumerator<UserDTO> listAct = actualListUsers.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestFriendsAndNotFriends()
        {
            // arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
          
            var currentUser = new UserDTO
            {
                Id = 2,
                Name = "Sam",
                Login = "Sam",
                Email = "sam@sam.ru",
                Password = "111111",
                RoleId = 3
            };

            var ListFriends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 3
                },
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 4
                },
                new FriendDTO
                {
                    UserId = 4,
                    FriendId = 7
                },
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 8
                }
            };

            var expLFriends = new List<UserDTO>();

            foreach (var fr in ListFriends)
            {
                if (fr.UserId == currentUser.Id)
                {
                    currentUser.Friends.Add(new Friend
                    {
                        UserId = currentUser.Id,
                        FriendId = fr.FriendId
                    });

                }
                else
                {
                    continue;
                }
            }
            expLFriends.Add(currentUser);

            mockUser.Setup(u => u.Get(currentUser.Id)).Returns(new User
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Login = currentUser.Login,
                Password = currentUser.Password,
                Email = currentUser.Email,
                RoleId = currentUser.RoleId,
                Friends = currentUser.Friends
            });
            mockFriend.Setup(f => f.GetAllFriends(It.IsAny<User>())).Returns(expLFriends.Select(x => new Friend
            {
                UserId = x.Id,
                FriendId = x.Id
            }));
            mockUser.Setup(u => u.GetAllElements()).Returns(expLFriends.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Password = x.Password,
                Email = x.Email,
                RoleId = x.RoleId,
                Friends = x.Friends
            }));

            // Act
            var actLFriends = userService.FriendsAndNotFriends(currentUser).ToList();

            // Assert
            mockUser.Verify(u => u.Get(It.Is<int>(id => id == currentUser.Id)), Times.AtLeastOnce);
            mockUser.Verify(u => u.GetAllElements(), Times.AtLeastOnce);
            mockFriend.Verify(f => f.GetAllFriends(It.Is<User>(t => t.Id == currentUser.Id)), Times.AtLeastOnce);

            Assert.AreEqual(expLFriends.Count(), actLFriends.Count());

            IEnumerator<UserDTO> expextedListFriends = actLFriends.GetEnumerator();
            IEnumerator<UserDTO> actualListFriends = expLFriends.GetEnumerator();

            while (expextedListFriends.MoveNext() && actualListFriends.MoveNext())
            {
                Assert.AreEqual(expextedListFriends.Current.ToString(), actualListFriends.Current.ToString());
            }
        }

        //[TestMethod]
        //public void TestGetAllImageFromUser()
        //{
        //    // arrange
        //    var mockUser = new Mock<IUserRepository>();
        //    var mockFriend = new Mock<IFriendRepository>();

        //    var userService = new UserService(mockUser.Object, mockFriend.Object);

        //    var ListImages = new List<ImageDTO>
        //    {
        //        new ImageDTO
        //        {
        //            Id = 1,
        //            Name = "img1",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "imgpath1",
        //            UserId = 2
        //        },
        //        new ImageDTO
        //        {
        //           Id = 2,
        //            Name = "img2",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "imgpath2",
        //            UserId = 1
        //        },
        //        new ImageDTO
        //        {
        //            Id = 3,
        //            Name = "img3",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "imgpath3",
        //            UserId = 2,
        //        }
        //    };


        //    var user = new UserDTO
        //    {
        //        Id = 2,
        //        Name = "Sam",
        //        Login = "Sam",
        //        Email = "sam@sam.ru",
        //        Password = "111111",
        //        RoleId = 3
        //    };

        //    var expectedLImages = new List<ImageDTO>();

        //    foreach (var img in ListImages)
        //    {
        //        if (img.UserId == user.Id)
        //        {
        //            user.Images.Add(new Image
        //            {
        //                Id = img.Id,
        //                Name = img.Name,
        //                ImageDate = img.ImageDate,
        //                PathImage = img.PathImage,
        //                UserId = img.UserId
        //            });
        //            expectedLImages.Add(img);
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //    }

        //    mockUser.Setup(u => u.Get(user.Id)).Returns(new User
        //    {
        //        Id = user.Id,
        //        Name = user.Name,
        //        Login = user.Login,
        //        Password = user.Password,
        //        Email = user.Email,
        //        RoleId = user.RoleId,
        //    });

        //    mockUser.Setup(u => u.GetAllImageFromFriendsAndUser(It.IsAny<User>())).Returns(expectedLImages.Select(im => new Image
        //    {
        //        Id = im.Id,
        //        Name = im.Name,
        //        ImageDate = im.ImageDate,
        //        PathImage = im.PathImage,
        //        UserId = im.UserId
        //    }));

        //    // Act
        //    var actualLImages = userService.GetAllImageFromFriendsAndUser(user).ToList();

        //    // Assert
        //    mockUser.Verify(u => u.Get(It.Is<int>(id => id == user.Id)), Times.Once);
        //    mockUser.Verify(u => u.GetAllImageFromFriendsAndUser(It.Is<User>(t => t.Id == user.Id)), Times.Once);

        //    Assert.AreEqual(expectedLImages.Count(), actualLImages.Count());

        //    IEnumerator<ImageDTO> expextedListImages = expectedLImages.GetEnumerator();
        //    IEnumerator<ImageDTO> actualListImages = actualLImages.GetEnumerator();

        //    while (expextedListImages.MoveNext() && actualListImages.MoveNext())
        //    {
        //        Assert.AreEqual(expextedListImages.Current.ToString(), actualListImages.Current.ToString());
        //    }
        //}

        //[TestMethod]
        //public void TestGetAllImageForCurrenrUser()
        //{
        //    // arrange
        //    var mock = new Mock<IUserRepository>();
        //    var mockFriend = new Mock<IFriendRepository>();
        //    var mockImage = new Mock<IImageRepository>();

        //    var userService = new UserService(mock.Object, mockFriend.Object);
        //    var imageService = new ImageService(mockImage.Object, mock.Object, mockFriend.Object);

        //    var dbImages = new List<ImageDTO>
        //    {
        //        new ImageDTO
        //        {
        //            Id = 2,
        //            Name = "image 02",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "blabla",
        //            UserId = 10,
        //        },
        //         new ImageDTO
        //        {
        //            Id = 3,
        //            Name = "image 03",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "blabla",
        //            UserId = 10,
        //        },
        //           new ImageDTO
        //        {
        //            Id = 4,
        //            Name = "image 04",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "blabla",
        //            UserId = 3,
        //        },
        //              new ImageDTO
        //        {
        //            Id = 5,
        //            Name = "image 05",
        //            ImageDate = System.DateTime.Now,
        //            PathImage = "blabla",
        //            UserId = 4,
        //        }

        //    };

        //    UserDTO currentUser = new UserDTO
        //    {
        //        Id = 10,
        //        Name = "Lisa",
        //        Login = "Lisa",
        //        Email = "lisa@nik.ru",
        //        Password = "111111",
        //        RoleId = 3,
        //    };
        //    var listFoundedImagesForCurrentUser = new List<ImageDTO>();
        //    for (int j = 0; j < dbImages.Count(); j++)
        //    {
        //        if (currentUser.Id == dbImages[j].UserId)
        //        {
        //            listFoundedImagesForCurrentUser.Add(dbImages[j]);
        //            currentUser.Images.Add(new Image
        //            {
        //                Id = dbImages[j].Id,
        //                ImageDate = dbImages[j].ImageDate,
        //                Name = dbImages[j].Name,
        //                PathImage = dbImages[j].PathImage,
        //                UserId = dbImages[j].UserId
        //            });
        //        }
        //    }


        //    mock.Setup(img => img.GetAllImageForCurrentUser(It.IsAny<User>()))
        //        .Returns(listFoundedImagesForCurrentUser.Select(i => new Image
        //        {
        //            Id = i.Id,
        //            Name = i.Name,
        //            ImageDate = i.ImageDate,
        //            PathImage = i.PathImage,
        //            User = i.User,
        //            UserId = i.UserId,
        //        }));

        //    // Act
        //    IEnumerable<ImageDTO> actualListImagesFromCurrentUser = userService.GetAllImageForCurrentUser(new UserDTO
        //    {
        //        Id = currentUser.Id,
        //        Email = currentUser.Email,
        //        Friends = currentUser.Friends,
        //        Images = currentUser.Images,
        //        Login = currentUser.Login,
        //        Name = currentUser.Name,
        //        Password = currentUser.Password,
        //        PhotoUser = currentUser.PhotoUser,
        //        RoleId = currentUser.RoleId,
        //        Roles = currentUser.Roles
        //    }).ToList();

        //    // Assert
        //    mock.Verify(u => u.Get(It.Is<int>(s => s == currentUser.Id)), Times.AtLeastOnce);
        //    mock.Verify(actual => actual.GetAllImageForCurrentUser(It.IsAny<User>()), Times.Once);

        //    Assert.AreEqual(listFoundedImagesForCurrentUser.Count(), actualListImagesFromCurrentUser.Count());

        //    IEnumerator<ImageDTO> listExp = listFoundedImagesForCurrentUser.GetEnumerator();
        //    IEnumerator<ImageDTO> listAct = actualListImagesFromCurrentUser.GetEnumerator();

        //    while (listExp.MoveNext() && listAct.MoveNext())
        //    {
        //        Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
        //    }
        //}

        //[TestMethod]
        //public void TestRoleIdByRoleName()
        //{
        //    //arrange
        //    var mockUser = new Mock<IUserRepository>();
        //    var mockFriend = new Mock<IFriendRepository>();
        //    var mockRoles = new Mock<IRoleRepository>();

        //    var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
           
        //    var expectedRoles = new List<RoleDTO>
        //    {
        //        new RoleDTO
        //        {
        //            Id = 1,
        //            Name = "admin"
        //        },
        //        new RoleDTO
        //        {
        //            Id = 2,
        //            Name = "moderator"
        //        },
        //        new RoleDTO
        //        {
        //            Id = 3,
        //            Name = "user"
        //        }
        //    };

        //    var searchRoleName = "moderator";

        //    var expRole = new RoleDTO();
        //    foreach (var r in expectedRoles)
        //    {
        //        if (r.Name == searchRoleName)
        //        {
        //            expRole.Id = r.Id;
        //            expRole.Name = r.Name;
        //            break;
        //        }
        //        else
        //        {
        //            expRole = new RoleDTO();
        //        }
        //    }

        //    mockUser.Setup(u => u.GetRoleIdByRoleName(searchRoleName)).Returns(expRole.Id);

        //    //act
        //    var actualRoleId = userService.RoleIdByRoleName(searchRoleName);

        //    //assert
        //    mockUser.Verify(u => u.GetRoleIdByRoleName(It.Is<string>(Name => Name == searchRoleName)), Times.Once);

        //    Assert.AreEqual(actualRoleId, expRole.Id);
        //}

      
        [TestMethod]
        public void TestDeleteAvatar()
        {
            //arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);

            var expectedLUser = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    PhotoUser = "photoNik"
                },
                new UserDTO
                {
                    Id = 2,
                    PhotoUser = "photoSam"
                },
                new UserDTO
                {
                    Id = 3,
                    PhotoUser = "photoTom"
                }
            };

            var expUser = new UserDTO { Id = expectedLUser[2].Id, PhotoUser = expectedLUser[2].PhotoUser };

            foreach (var us in expectedLUser)
            {
                if (us.Id == expUser.Id)
                {
                    expectedLUser[(int)us.Id - 1].PhotoUser = "";
                    break;
                }
                else
                {
                    expUser = new UserDTO { Id = expectedLUser[2].Id, PhotoUser = expectedLUser[2].PhotoUser };
                }
            }

            mockUser.Setup(u => u.DeleteAvatar(expUser.Id));

            mockUser.Setup(u => u.GetAllElements()).Returns(expectedLUser.Select(x => new User
            {
                Id = x.Id,
                PhotoUser = x.PhotoUser
            }));

            //act
            userService.DeleteAvatar(expUser.Id);
            var actualLUsers = userService.GetAllElements().ToList();

            //assert
            mockUser.Verify(u => u.DeleteAvatar(It.Is<long>(us => us == expUser.Id)), Times.Once);
            mockUser.Verify(u => u.GetAllElements(), Times.Once);

            Assert.AreEqual(expectedLUser.Count(), actualLUsers.Count());

            IEnumerator<UserDTO> expectedListUsers = expectedLUser.GetEnumerator();
            IEnumerator<UserDTO> actualListUsers = actualLUsers.GetEnumerator();

            while (expectedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expectedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestGetAllFriends()
        {
            // arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
            
            var ListUsers = new List<UserDTO>
            {
                new UserDTO
                {
                    Id = 1,
                    Name = "Nik",
                    Login = "Nik",
                    Email = "nik@nik.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 2,
                    Name = "Sam",
                    Login = "Sam",
                    Email = "sam@sam.ru",
                    Password = "111111",
                    RoleId = 3
                },
                new UserDTO
                {
                    Id = 3,
                    Name = "Tom",
                    Login = "Tom",
                    Email = "tom@tom.ru",
                    Password = "111111",
                    RoleId = 3
                },
                 new UserDTO
                {
                    Id = 4,
                    Name = "Bill",
                    Login = "Bill",
                    Email = "bill@bill.ru",
                    Password = "111111",
                    RoleId = 3
                }
            };

            var expListFriends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 1,
                    FriendId = 2
                },
                new FriendDTO
                {
                    UserId = 1,
                    FriendId = 4
                }
            };
            
            foreach (var fr in expListFriends)
            {
                if(fr.FriendId == ListUsers[0].Id)
                {
                    ListUsers[0].Friends.Add(new Friend {
                        UserId = ListUsers[0].Id,
                        FriendId = fr.FriendId
                    });
                }
                else
                {
                    continue;
                }
            }

            mockFriend.Setup(u => u.GetAllFriend(ListUsers[0].Id)).Returns(expListFriends.Select(x => new Friend
            {
                UserId = x.UserId,
                FriendId = x.FriendId
            }));

            // Act
            var actLFriends = userService.GetAllFriends(ListUsers[0]).ToList();

            // Assert
            mockFriend.Verify(u => u.GetAllFriend(It.Is<int>(id => id == ListUsers[0].Id)), Times.AtLeastOnce);

            Assert.AreEqual(expListFriends.Count(), actLFriends.Count());

            IEnumerator<FriendDTO> expextedListFriends = actLFriends.GetEnumerator();
            IEnumerator<FriendDTO> actualListFriends = expListFriends.GetEnumerator();

            while (expextedListFriends.MoveNext() && actualListFriends.MoveNext())
            {
                Assert.AreEqual(expextedListFriends.Current.ToString(), actualListFriends.Current.ToString());
            }
        }

        [TestMethod]
        public void TestGetAllFriendsForUser()
        {
            // arrange
            var mockUser = new Mock<IUserRepository>();
            var mockFriend = new Mock<IFriendRepository>();
            var mockRoles = new Mock<IRoleRepository>();

            var userService = new UserService(mockUser.Object, mockFriend.Object, mockRoles.Object);
         
            var currentUser = new UserDTO
            {
                Id = 2,
                Name = "Sam",
                Login = "Sam",
                Email = "sam@sam.ru",
                Password = "111111",
                RoleId = 3
            };

            var ListFriends = new List<FriendDTO>
            {
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 3
                },
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 4
                },
                new FriendDTO
                {
                    UserId = 4,
                    FriendId = 7
                },
                new FriendDTO
                {
                    UserId = 2,
                    FriendId = 8
                }
            };

            var expLFriends = new List<UserDTO>();

            foreach (var fr in ListFriends)
            {
                if (fr.UserId == currentUser.Id)
                {
                    currentUser.Friends.Add(new Friend
                    {
                        UserId = currentUser.Id,
                        FriendId = fr.FriendId
                    });
                    
                }
                else
                {
                    continue;
                }
            }
            expLFriends.Add(currentUser);

            mockUser.Setup(u => u.Get(currentUser.Id)).Returns(new User
            {
                Id = currentUser.Id,
                Name = currentUser.Name,
                Login = currentUser.Login,
                Password = currentUser.Password,
                Email = currentUser.Email,
                RoleId = currentUser.RoleId,
                Friends = currentUser.Friends
            });
            mockUser.Setup(u => u.GetAllFriendsForUser(It.IsAny<User>())).Returns(expLFriends.Select(x => new User
            {
                Id = x.Id,
                Name = x.Name,
                Login = x.Login,
                Password = x.Password,
                Email = x.Email,
                RoleId = x.RoleId,
                Friends = x.Friends
            }));

            // Act
            var actLFriends = userService.GetAllFriendsForUser(currentUser).ToList();

            // Assert
            mockUser.Verify(u => u.Get(It.Is<int>(id => id == currentUser.Id)), Times.AtLeastOnce);
            mockUser.Verify(u => u.GetAllFriendsForUser(It.Is<User>(t => t.Id == currentUser.Id)), Times.AtLeastOnce);
     
            Assert.AreEqual(expLFriends.Count(), actLFriends.Count());

            IEnumerator<UserDTO> expextedListFriends = actLFriends.GetEnumerator();
            IEnumerator<UserDTO> actualListFriends = expLFriends.GetEnumerator();

            while (expextedListFriends.MoveNext() && actualListFriends.MoveNext())
            {
                Assert.AreEqual(expextedListFriends.Current.ToString(), actualListFriends.Current.ToString());
            }
        }
    }
}
