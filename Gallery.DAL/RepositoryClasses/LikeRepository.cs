using Gallery.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Gallery.DAL.Models;
using System.Data;

namespace Gallery.DAL.RepositoryClasses
{
    public class LikeRepository : BaseRepository, ILikeRepository
    {
        public LikeRepository(IDbConnection conn) : base(conn)
        {

        }
        public void AddLike(long ImageId, long UserId)
        {
            var sqlQuery = @"INSERT INTO Likes (ImageId, UserId) VALUES (@ImageId, @UserId)";
            connection.Execute(sqlQuery, new { UserId, ImageId });
        }

        public long CountLikes(long ImageId)
        {
            var sql = @"Select count(ImageId) from Likes where ImageId = @ImageId";
          var countLikes = connection.Query<long>(sql, new { ImageId }).FirstOrDefault();
            return countLikes;
        }

        public void DeleteLike(long ImageId, long UserId)
        {
            var sqlQuery = "DELETE FROM Likes WHERE UserId = @UserId and ImageId = @ImageId";
            connection.Execute(sqlQuery, new { ImageId, UserId });
        }

        public IEnumerable<Like> GetLikesByUserId(long userId)
        {
            var allLikes = connection.Query<Like>("SELECT * FROM Likes WHERE UserId = @userId", new { userId }).ToList(); 
            return allLikes;
        }

        public bool IsLikeThisPhoto(long idImg, long idUser)
        {
            var allLikesForThisImageFromCurrentUser = GetLikesByUserId(idUser).Where(i=>i.ImageId==idImg);

            if(allLikesForThisImageFromCurrentUser.FirstOrDefault() != null)
            {
                return true;
            }
            return false;
        }
    }
}
