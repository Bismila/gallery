using System;
using System.Collections.Generic;
using System.Linq;
using Gallery.BAL.DTO;
using Gallery.BAL.Services;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gallery.Tests.ServicesTests
{
    [TestClass]
    public class RolesTests
    {
        [TestMethod]
        public void TestMoqCreateRole()
        {
            // arrange
            var mockRole = new Mock<IRoleRepository>();

            var roleService = new RoleService(mockRole.Object);

            var listRolesDB = new List<RoleDTO>
            {
                 new RoleDTO
                {
                    Id = 1,
                    Name = "admin"
                },
                  new RoleDTO
                {
                    Id = 2,
                    Name = "moderator"
                }
            };
            var role = new RoleDTO
            {
                Id = 3,
                Name = "user"
            };

            listRolesDB.Add(role);

            mockRole.Setup(u => u.Create(new Role
            {
                Id = role.Id,
                Name = role.Name
            }));

            mockRole.Setup(r => r.GetAllElements()).Returns(listRolesDB.Select(rr => new Role
            {
                Id = rr.Id,
                Name = rr.Name
            }));

            // Act
            roleService.Create(role);
            var actualLisRoles = roleService.GetAllElements().ToList();

            // Assert
            mockRole.Verify(i => i.Create(It.Is<Role>(t => t.Id == 3)), Times.Once);
            mockRole.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(listRolesDB.Count(), actualLisRoles.Count());

            IEnumerator<RoleDTO> listExp = listRolesDB.GetEnumerator();

            IEnumerator<RoleDTO> listAct = actualLisRoles.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestMoqDeleteRole()
        {
            // arrange
            var mockRole = new Mock<IRoleRepository>();

            var roleService = new RoleService(mockRole.Object);

            var listRolesDB = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Id = 1,
                    Name = "admin"
                },
               new RoleDTO
                {
                    Id = 2,
                    Name = "moderator"
                },
               new RoleDTO
                {
                    Id = 3,
                    Name = "user"
                }
            };

            var delRoleId = 2;
            for (int i = 0; i < listRolesDB.Count(); i++)
            {
                if(listRolesDB[i].Id == delRoleId)
                {
                    listRolesDB.RemoveAt(delRoleId-1);
                }
            }

            mockRole.Setup(r => r.Delete(delRoleId));

            mockRole.Setup(r => r.GetAllElements()).Returns(listRolesDB.Select(rr => new Role
            {
                Id = rr.Id,
                Name = rr.Name
            }));

            // Act
            roleService.Delete(delRoleId);
            var actualLisRoles = roleService.GetAllElements().ToList();

            // Assert
            mockRole.Verify(r => r.Delete(It.Is<int>(id => id == delRoleId)), Times.Once);
            mockRole.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(listRolesDB.Count(), actualLisRoles.Count());

            IEnumerator<RoleDTO> listExp = listRolesDB.GetEnumerator();

            IEnumerator<RoleDTO> listAct = actualLisRoles.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }

        }

        [TestMethod]
        public void TestMoqUpdateRole()
        {
            // arrange
            var mockRole = new Mock<IRoleRepository>();

            var roleService = new RoleService(mockRole.Object);

            var listRolesDB = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Id = 1,
                    Name = "admin"
                },
                new RoleDTO
                {
                    Id = 2,
                    Name = "moderator"
                },
                new RoleDTO
                {
                    Id = 3,
                    Name = "user"
                }
            };
            var role = new RoleDTO
            {
                Id = 3,
                Name = "super user"
            };

            for (int i = 0; i < listRolesDB.Count(); i++)
            {
                if (listRolesDB[i].Id == role.Id)
                {
                    listRolesDB[i] = role;
                }
            }

            mockRole.Setup(u => u.Update(new Role
            {
                Id = role.Id,
                Name = role.Name
            }));

            mockRole.Setup(r => r.GetAllElements()).Returns(listRolesDB.Select(rr => new Role
            {
                Id = rr.Id,
                Name = rr.Name
            }));

            // Act
            roleService.Update(role);
            var actualLisRoles = roleService.GetAllElements().ToList();

            // Assert
            mockRole.Verify(i => i.Update(It.Is<Role>(t => t.Id == 3)), Times.Once);
            mockRole.Verify(actual => actual.GetAllElements(), Times.Once);

            Assert.AreEqual(listRolesDB.Count(), actualLisRoles.Count());

            IEnumerator<RoleDTO> listExp = listRolesDB.GetEnumerator();

            IEnumerator<RoleDTO> listAct = actualLisRoles.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqGetRole()
        {
            // arrange
            var mockRole = new Mock<IRoleRepository>();

            var roleService = new RoleService(mockRole.Object);

            var getRoleId = 2;

            var listRolesDB = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Id = 1,
                    Name = "admin"
                },
                new RoleDTO
                {
                    Id = 2,
                    Name = "moderator"
                },
                new RoleDTO
                {
                    Id = 3,
                    Name = "user"
                }
            };
            RoleDTO findElement = new RoleDTO();
            for (int i = 0; i < listRolesDB.Count(); i++)
            {
                if (listRolesDB[i].Id == getRoleId)
                {
                    findElement = listRolesDB[getRoleId - 1];
                }
            }
            mockRole.Setup(r => r.Get(getRoleId)).Returns(new Role
            {
                Id = findElement.Id,
                Name = findElement.Name
            });

            // Act
            
            var actualRole = roleService.Get(getRoleId);

            // Assert
            mockRole.Verify(r => r.Get(It.Is<int>(id => id == getRoleId)), Times.AtLeastOnce);

            Assert.AreEqual(findElement.ToString(), actualRole.ToString());
        }

        [TestMethod]
        public void TestMoqGetAllRoles()
        {
            // arrange
            var mockRole = new Mock<IRoleRepository>();

            var roleService = new RoleService(mockRole.Object);

            var listRolesDB = new List<RoleDTO>
            {
                new RoleDTO
                {
                    Id = 1,
                    Name = "admin"
                },
                new RoleDTO
                {
                    Id = 2,
                    Name = "moderator"
                },
                new RoleDTO
                {
                    Id = 3,
                    Name = "user"
                }
            };
          
            mockRole.Setup(r => r.GetAllElements()).Returns(listRolesDB.Select(role =>new Role
            {
                Id = role.Id,
                Name = role.Name
            }));

            // Act
            var actualLisRoles = roleService.GetAllElements();

            //Assert
            Assert.AreEqual(listRolesDB.Count(), actualLisRoles.Count());

            IEnumerator<RoleDTO> listExp = listRolesDB.GetEnumerator();

            IEnumerator<RoleDTO> listAct = actualLisRoles.GetEnumerator();

            while (listExp.MoveNext() && listAct.MoveNext())
            {
                Assert.AreEqual(listExp.Current.ToString(), listAct.Current.ToString());
            }
        }
    }
}
