using System.Collections.Generic;
using System.Linq;
using System.Data;
using Dapper;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System;

namespace Gallery.DAL.RepositoryClasses
{
    public class UserRepository : BaseRepository, IBaseRepository<User>, IUserRepository
    {
        public UserRepository(IDbConnection conn) : base(conn)
        {

        }

        public User GetCurrentUser(string loginName)
        {
            var user = connection.Query<User>("SELECT * FROM Users WHERE Login = @loginName", new { loginName }).FirstOrDefault();
            return user;
        }
        public IEnumerable<User> SearchFriends(string searchLogin)
        {
            var symb = "%" + searchLogin + "%";
            var sql = @"select * 
                        from Friends f
                        left join Users u on f.FriendId = u.Id
                        where u.Login like @symb";
            var searchingFriends = connection.Query<User>(sql, new { symb }).ToList();

            return searchingFriends;
        }

        public IEnumerable<User> SearchUsers(string searchLogin)
        {
            var symb = "%" + searchLogin + "%";
            var sql = @"use dbGallery
                        select * 
                        from Users u
                        left join Friends f on f.FriendId = u.Id
                        where u.Login like @symb and f.FriendId is null";
            var searchingUsers = connection.Query<User>(sql, new { symb}).ToList();

            return searchingUsers;
        }

        public string GetUserRoleByUserName(string loginName)
        {
            var sqlQuery = connection.Query<string>("select r.Name from Roles r Inner join Users u on u.RoleId = r.Id Where u.Login = @loginName", new { loginName }).FirstOrDefault();
            return sqlQuery;
        }

        public string GetUserRolesById(long id)
        {
            var sqlQuery = "SELECT Roles.Name FROM Roles, Users WHERE Users.RoleId = Roles.Id and Users.Id = @id";
            connection.Execute(sqlQuery, new { id });
            return sqlQuery;
        }

      

        public void Create(User element)
        {
            var sqlQuery = "INSERT INTO Users (Name, Login, Email, Password, RoleId, PhotoUser)" +
                " VALUES (@Name, @Login, @Email, @Password, @RoleId, @PhotoUser); ";

            connection.Execute(sqlQuery, element);
        }

        public void Delete(long id)
        {
            var sqlQuery = "DELETE FROM Users WHERE Id = @id";
            connection.Execute(sqlQuery, new { id });
        }

        public User Get(long id)
        {
            var user = connection.Query<User>("SELECT * FROM Users WHERE Id = @id", new { id }).FirstOrDefault();
            return user;
        }
        public void DeleteAvatar(long id)
        {
            var sqlQuery = "UPDATE Users SET PhotoUser = @PhotoUser WHERE Id = @id";
            connection.Execute(sqlQuery, new { PhotoUser = "", Id = id});
        }

        public IEnumerable<User> GetAllElements()
        {
            var users = connection.Query<User>("SELECT * FROM Users").ToList();
            return users;
        }
        public IEnumerable<User> GetAllFriendsForUser(User currentUser)
        {
            long Id = currentUser.Id;
            var sql = @"SELECT * 
                        FROM Users u
                        INNER JOIN Friends f ON f.FriendId = u.id
                        WHERE f.UserId = @Id";

            var friends = connection.Query<User, Friend, User>(sql, (u, f) =>
            {
                u.Friends.Add(f);
                return u;
            }, new { Id }, splitOn: "UserId");

            var groupFriendsUser = friends.GroupBy(user => user.Id, user => user).Select(x => new User
            {
                Id = x.Key,
                Friends = x.SelectMany(f => f.Friends).ToList()

            });

            return friends;
        }
        public void Update(User element)
        {
            var sqlQuery = @"UPDATE Users 
                            SET Name = @Name, Login = @Login, Email = @Email, Password = @Password, RoleId = @RoleId, PhotoUser = @PhotoUser 
                            WHERE Id = @Id";
            connection.Execute(sqlQuery, element);
        }

        public bool ListEquals(List<User> expected, List<User> actual)
        {
            if (expected.Count != actual.Count)
                return false;
            else
            {
                for (var i = 0; i < expected.Count(); i++)
                {
                    if (expected[i].Id.Equals(actual[i].Id) &&
                        expected[i].Name.Equals(actual[i].Name) &&
                        expected[i].Login.Equals(actual[i].Login) &&
                        expected[i].Email.Equals(actual[i].Email) &&
                        expected[i].Password.Equals(actual[i].Password) &&
                        expected[i].RoleId.Equals(actual[i].RoleId) &&
                        expected[i].PhotoUser.Equals(actual[i].PhotoUser))
                        return true;
                }
                return false;
            }
        }   

        public IEnumerable<User> GetFriendsAndNotFriends(User currentUser)
        {
            var Id = currentUser.Id;

            var sql = @"select * 
                        from Users u inner JOIN Friends f
                        ON f.FriendId = u.Id where f.UserId = @Id";

            var friends = connection.Query<User, Friend, User>(sql, (U, F) =>
            {
                U.Friends.Add(F);
                return U;
            }, new { Id }, splitOn: "UserId");

            return friends;
        }
    }
}
