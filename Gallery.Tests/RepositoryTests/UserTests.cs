using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data;
using System.Collections.Generic;
using Gallery.DAL.Models;
using Moq.Dapper;
using Dapper;
using Gallery.DAL.RepositoryClasses;
using System.Linq;

namespace Gallery.Tests.RepositoryTests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void TestMethodGetAllUsers()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "SELECT * FROM Users";
            var expected = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Tom",
                    Login = "LogTom",
                    Email = "tom@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 2,
                    Name = "Ben",
                    Login = "LogBen",
                    Email = "ben@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 3,
                    Name = "Sam",
                    Login = "LogSam",
                    Email = "sam@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                }
            };

            connection.SetupDapper(c => c.Query<User>(sqlQuery, null, null, true, null, null))
                      .Returns(expected);

            var repo = new UserRepository(connection.Object);
            var actualRepo = repo.GetAllElements().ToList();
            var isEq = repo.ListEquals(expected, actualRepo);

            bool isEqual = expected.Count.Equals(actualRepo.Count());
            if (isEqual)
            {
                Assert.AreEqual(expected.Count, actualRepo.Count);
            }
            else
                Assert.Fail();
            //Assert.AreEqual(expected.Count, actualRepo.Count);
        }

        [TestMethod]
        public void TestMethodCreateUser()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "INSERT INTO Users (Id, Name, Login, Email, Password, PhotoUser) VALUES (@Id, @Name, @Login, @Email, @Password, @PhotoUser); SELECT CAST(SCOPE_IDENTITY() as int)";
            var expected = new List<User>
            {
               new User
               {
                   Id = 2,
                   Name = "Ben",
                   Login = "LogBen",
                   Email = "ben@gmail.com",
                   Password = "111",
                   PhotoUser = "url(...)"
               },
               new User
               {
                   Id = 3,
                   Name = "Sam",
                   Login = "LogSam",
                   Email = "sam@gmail.com",
                   Password = "111",
                   PhotoUser = "url(...)"
               }

            };
            var element = new User
            {
                Id = 1,
                Name = "Tom",
                Login = "LogTom",
                Email = "tom@gmail.com",
                Password = "111",
                PhotoUser = "url(...)"
            };

            expected.Add(element);

            connection.SetupDapper(c => c.Query<User>(sqlQuery, element, null, true, null, null)).Returns(expected);

            var repo = new UserRepository(connection.Object);
            repo.Create(element);

            var actualRepo = repo.GetAllElements().ToList();
            var isEq = repo.ListEquals(expected, actualRepo);
        }


        [TestMethod]
        public void TestMethodDeleteUser()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "DELETE FROM Users WHERE Id = @id";
            var expected = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Tom",
                    Login = "LogTom",
                    Email = "tom@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 2,
                    Name = "Ben",
                    Login = "LogBen",
                    Email = "ben@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 3,
                    Name = "Sam",
                    Login = "LogSam",
                    Email = "sam@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                }
             };

            var elementId = 3;
            expected.RemoveAt(elementId - 1);

            connection.SetupDapper(c => c.Query<User>(sqlQuery, null, null, true, null, null)).Returns(expected);

            var repo = new UserRepository(connection.Object);
            var actualRepo = repo.GetAllElements().ToList();
            var isEqual = repo.ListEquals(expected, actualRepo);
            if (isEqual)
            {
                Assert.AreEqual(expected.Count, actualRepo.Count);
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void TestMethodUpdateUser()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "UPDATE Users SET id = @Id, Name = @Name, Login = @Loddin, Email = @Email, Password = @Password, PhotoUser = @PhotoUser WHERE Id = @Id";
            var expected = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "",
                    Login = "",
                    Email = "",
                    Password = "111",
                    PhotoUser = "url()"
                },
                new User
                {
                    Id = 2,
                    Name = "Ben",
                    Login = "LogBen",
                    Email = "ben@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 3,
                    Name = "Sam",
                    Login = "LogSam",
                    Email = "sam@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                }

            };
            var element = new User
            {
                Id = 1,
                Name = "Tom",
                Login = "LogTom",
                Email = "tom@gmail.com",
                Password = "111",
                PhotoUser = "url(...)"
            };
            expected.RemoveAt((int)element.Id - 1);
            expected.Insert((int)element.Id - 1, element);

            connection.SetupDapper(c => c.Query<User>(sqlQuery, element, null, true, null, null)).Returns(expected);

            var repo = new UserRepository(connection.Object);
            repo.Update(element);

            var actualRepo = repo.GetAllElements().ToList();
            var isEq = repo.ListEquals(expected, actualRepo);
            if (isEq)
            {
                Assert.AreEqual(expected.Count, actualRepo.Count);
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void TestMethodGetUserId()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "SELECT * FROM Roles WHERE Id = @id";
            var expected = new List<User>
            {
                new User
                {
                    Id = 1,
                    Name = "Tom",
                    Login = "LogTom",
                    Email = "tom@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 2,
                    Name = "Ben",
                    Login = "LogBen",
                    Email = "ben@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                },
                new User
                {
                    Id = 3,
                    Name = "Sam",
                    Login = "LogSam",
                    Email = "sam@gmail.com",
                    Password = "111",
                    PhotoUser = "url(...)"
                }
            };
            

            var elemId = 2;

            connection.SetupDapper(c => c.Query<User>(sqlQuery, elemId, null, true, null, null))
                .Returns(expected);

            var repo = new UserRepository(connection.Object);
            var actualRepo = repo.GetAllElements().ToList();
            var isEq = repo.ListEquals(expected, actualRepo);
            if (isEq)
            {
                Assert.AreEqual(expected.Count, actualRepo.Count);
            }
            else
                Assert.Fail();
        }
    }
}
