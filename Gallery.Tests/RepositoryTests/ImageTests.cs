﻿using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Gallery.DAL.Models;
using Gallery.DAL.RepositoryClasses;
using Moq.Dapper;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace Gallery.Tests.RepositoryTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void TestMoqGetAll()
        {
            var connection = new Mock<IDbConnection>();

            var expected = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    Name = "image1",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 2,
                    Name = "image2",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 3,
                    Name = "image3",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                }
            };
            var sql = @"SELECT * FROM Images";
            connection.SetupDapper(c => c.Query<Image>(sql, null, null, true, null, null))
                                                        .Returns(expected);

            var img = new ImageRepository(connection.Object);
            var actual = img.GetAllElements().ToList();
            Assert.AreEqual(expected.Count, actual.Count);

            IEnumerator<Image> expextedListUsers = expected.GetEnumerator();
            IEnumerator<Image> actualListUsers = actual.GetEnumerator();

            while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqCreateImage()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "INSERT INTO Images (Name, ImageDate, PathImage, UserId) VALUES (@Name, @ImageDate, @PathImage, @UserId); SELECT CAST(SCOPE_IDENTITY() as int)";

            var expDB = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    Name = "image1",
                    ImageDate = new DateTime(),
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html"
                },
                new Image
                {
                    Id = 2,
                    Name = "image2",
                    ImageDate = new DateTime(),
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html"
                }
            };

            var item = new Image
            {
                Id = 3,
                Name = "image3",
                ImageDate = new DateTime(),
                PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html"
            };

            expDB.Add(item);

            connection.SetupDapper(c => c.Query<Image>(sqlQuery, null, null, true, null, null))
                .Returns(expDB);

            var img = new ImageRepository(connection.Object);
            img.Create(item);
            var actual = img.GetAllElements().ToList();
            Assert.AreEqual(expDB.Count, actual.Count);

            IEnumerator<Image> expextedListUsers = expDB.GetEnumerator();
            IEnumerator<Image> actualListUsers = actual.GetEnumerator();

            while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
            {
                Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
            }
        }

        [TestMethod]
        public void TestMoqDeleteImage()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "DELETE FROM Images WHERE Id = @id";

            var expDB = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    Name = "image1",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 2,
                    Name = "image2",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 3,
                    Name = "image3",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                }
            };

            var id = 2;

            if (id > expDB.Count || id <= 0)
            {
                Assert.Fail();
            }
            else if(id < expDB.Count || id >= 0)
            {
                expDB.RemoveAt(id - 1);

                connection.SetupDapper(c => c.Query<Image>(sqlQuery, null, null, true, null, null))
                 .Returns(expDB);

                var img = new ImageRepository(connection.Object);
                img.Delete(id);

                var db = img.GetAllElements().ToList();
                bool flag = false;
                for (int i = 0; i < expDB.Count; i++)
                {
                    if (expDB[i].Id == id)
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                bool equal = expDB.Count.Equals(db.Count());
                var actual = img.GetAllElements().ToList();
                Assert.AreEqual(expDB.Count, actual.Count);

                IEnumerator<Image> expextedListUsers = expDB.GetEnumerator();
                IEnumerator<Image> actualListUsers = actual.GetEnumerator();

                while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
                {
                    Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
                }
            }
        }

        [TestMethod]
        public void TestMoqGetImage()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "SELECT * FROM Images WHERE Id = @id";
            var expDB = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    Name = "image1",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 2,
                    Name = "image2",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 3,
                    Name = "image3",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                }
            };

            var id = 2;

            connection.SetupDapper(c => c.Query<Image>(sqlQuery, null, null, true, null, null)).Returns(expDB);

            var img = new ImageRepository(connection.Object);

            if (id > expDB.Count || id <= 0)
            {
                Assert.Fail();
            }
            else
            {
                img.Get(id);

                var db = img.GetAllElements().ToList();

                bool flag = false;

                foreach (var item in expDB)
                {
                    if (item.Id.Equals(id))
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                Assert.AreEqual(expDB.Count, db.Count);

                IEnumerator<Image> expextedListUsers = expDB.GetEnumerator();
                IEnumerator<Image> actualListUsers = db.GetEnumerator();

                while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
                {
                    Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
                }
            }
        }


        [TestMethod]
        public void TestMoqUpdateImage()
        {
            var connection = new Mock<IDbConnection>();
            var sqlQuery = "UPDATE Images SET Name = @Name, ImageDate = @ImageDate, PathImage = @PathImage WHERE Id = @Id";

            var expDB = new List<Image>
            {
                new Image
                {
                    Id = 1,
                    Name = "image1",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 2,
                    Name = "image2",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                },
                new Image
                {
                    Id = 3,
                    Name = "image3",
                    PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                    ImageDate = new DateTime()
                }
            };

            var id = 3;
            Image withoutChange = expDB[id - 1];
            var expected = new Image
            {
                Id = id,
                Name = "image888",
                PathImage = "http://bipbap.ru/krasivye-kartinki/krasivye-kartinki-volki-40-foto.html",
                ImageDate = new DateTime()
            };

            expDB.RemoveAt(id - 1);

            expDB.Add(expected);

            connection.SetupDapper(c => c.Query<Image>(sqlQuery, null, null, true, null, null)).Returns(expDB);

            var img = new ImageRepository(connection.Object);
            if (id > expDB.Count || id <= 0)
            {
                Assert.Fail();
            }
            else
            {
                img.Update(expected);

                var db = img.GetAllElements().ToList();
                bool flag = false;
                for (int i = 0; i < expDB.Count; i++)
                {
                    if (expDB[i].Id.Equals(id))
                    {
                        flag = true;
                        break;
                    }
                    else
                    {
                        flag = false;
                    }
                }
                Assert.AreEqual(expDB.Count, db.Count);

                IEnumerator<Image> expextedListUsers = expDB.GetEnumerator();
                IEnumerator<Image> actualListUsers = db.GetEnumerator();

                while (expextedListUsers.MoveNext() && actualListUsers.MoveNext())
                {
                    Assert.AreEqual(expextedListUsers.Current.ToString(), actualListUsers.Current.ToString());
                }
            }
        }
    }
}
