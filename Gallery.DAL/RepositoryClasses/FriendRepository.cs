using Dapper;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Gallery.DAL.RepositoryClasses
{
    public class FriendRepository : BaseRepository, IFriendRepository
    {
        public FriendRepository(IDbConnection conn) : base(conn)
        {

        }
        public IEnumerable<Friend> GetAllFriends(User currentUser)
        {
            long Id = currentUser.Id;
            var sql = "SELECT * FROM Friends f INNER JOIN Users u ON f.FriendId = u.id WHERE f.UserId = @Id";

            var friends = connection.Query<Friend, User, Friend>(sql, (f, u) =>
            {
                f.Users.Add(u);
                return f;

            }, new { Id }, splitOn: "Id");

            return friends;
        }
        public void AddFriend(long UserId, long FriendId)
        {
            var sqlQuery = "INSERT INTO Friends (UserId, FriendId)" +
               " VALUES (@UserId, @FriendId)";
            connection.Execute(sqlQuery, new  { UserId, FriendId });
        }

        public void DelFriend(long FriendId)
        {
            var sqlQuery = "DELETE FROM Friends WHERE FriendId = @FriendId";
            connection.Execute(sqlQuery, new { FriendId });
        }

        public void DeleteAllFriendsByUser(long userId)
        {
            var sqlQuery = @"delete from Friends
                        where Friends.UserId = @userId
                        delete from Friends
                        where Friends.FriendId = @userId";
            connection.Execute(sqlQuery, new { userId });
        }

        public IEnumerable<Friend> GetAllFriend(long Id)
        {
            var friends = connection.Query<Friend>("SELECT * FROM Friends WHERE UserId = @Id", new { Id }).ToList();
            return friends;
        } 

        public IEnumerable<Image> GetAllImageFriends(long currentUserId)
        {
            var sql = "select * from Images I left JOIN Friends F ON F.FriendId = I.UserId ";
            var friendsImages = connection.Query<Image>(sql).ToList();
            return friendsImages;
        }
    }
}
