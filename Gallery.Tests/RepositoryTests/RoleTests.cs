using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Moq;
using Moq.Dapper;
using Dapper;
using System.Linq;
using Gallery.DAL.Models;
using System.Collections.Generic;
using Gallery.DAL.RepositoryClasses;

namespace Gallery.Tests.RepositoryTests
{
    [TestClass]
    public class RoleTests
    {
        [TestMethod]
        public void TestMethodGetAllRoles()
        {
            var connection = new Mock<IDbConnection>();

            var expected = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Moderator"},
                new Role { Id = 3, Name = "Guest"}
            };

            connection.SetupDapper(c => c.Query<Role>("SELECT * FROM Roles", null, null, true, null, null))
                      .Returns(expected);

            var repo = new RoleRepository(connection.Object);
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
        public void TestMethodCreateRole()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "INSERT INTO Roles (Id, Name) VALUES (@Id, @Name); SELECT CAST(SCOPE_IDENTITY() as int)";
            var expected = new List<Role>
            {
                new Role { Id = 2, Name = "Moderator"},
                new Role { Id = 3, Name = "Guest"}
            };
            var element = new Role { Id = 1, Name = "Admin" };
            expected.Add(element);

            connection.SetupDapper(c => c.Query<Role>(sqlQuery, element, null, true, null, null))
                .Returns(expected);

            var repo = new RoleRepository(connection.Object);
            repo.Create(element);
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
        public void TestMethodDeleteRole()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "DELETE FROM Roles WHERE Id = @id";
            var expected = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Moderator"},
                new Role { Id = 3, Name = "Guest"}
            };
            var elementId = 3;
            expected.RemoveAt(elementId - 1);

            connection.SetupDapper(c => c.Query<Role>(sqlQuery, null, null, true, null, null)).Returns(expected);

            var repo = new RoleRepository(connection.Object);
            repo.Delete(elementId);

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
        public void TestMethodUpdateRole()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "UPDATE Roles SET Id = @Id, Name = @Name WHERE Id = @Id";
            var expected = new List<Role>
            {
                new Role { Id = 1, Name = "NO Role" },
                new Role { Id = 2, Name = "Moderator"},
                new Role { Id = 3, Name = "Guest"}
            };
            var element = new Role { Id = 1, Name = "Admin" };
            expected.RemoveAt((int)element.Id - 1);
            expected.Insert((int)element.Id - 1, element);

            connection.SetupDapper(c => c.Query<Role>(sqlQuery, element, null, true, null, null)).Returns(expected);

            var repo = new RoleRepository(connection.Object);
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
        public void TestMethodGetRoleId()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "SELECT * FROM Roles WHERE Id = @id";
            var expected = new List<Role>
            {
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 15, Name = "Moderator"},
                new Role { Id = 3, Name = "Guest"}
            };
            var elemId = 15;
            var elemGetId = new Role();
            for(var i = 0; i < expected.Count; i++)
            {
                if(expected[i].Id == elemId)
                {
                    elemGetId = expected[i];
                }
            }

            connection.SetupDapper(c => c.Query<Role>(sqlQuery, new { Id = elemId }, null, true, null, null))
                .Returns(expected);

            var repo = new RoleRepository(connection.Object);
            var addElemToDb = repo.Get(elemId);
            Assert.AreEqual(addElemToDb.Id.ToString(), elemGetId.Id.ToString());
            Assert.AreEqual(addElemToDb.Name.ToString(), elemGetId.Name.ToString());

        }
    }
}
