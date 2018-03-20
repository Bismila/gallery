using Dapper;
using Gallery.DAL.IRepository;
using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.RepositoryClasses
{
    public class CommentRepository: BaseRepository, ICommentRepository
    {
        public CommentRepository(IDbConnection conn) : base(conn)
        {
                
        }

        public void AddComment(Comment comment)
        {
            var sql = "INSERT INTO Comments(UserId, ImageId, Text, CommentData, ParentId) VALUES (@UserId, @ImageId, @Text, @CommentData, @ParentId)";
            connection.Execute(sql, comment);
        }

        public void DeleteComment(long Id)
        {
            var sql = "DELETE FROM Comments WHERE Id = @Id";
            connection.Execute(sql, new { Id });
        }

        public void UpdateComment(Comment comment)
        {
            var sql = "UPDATE Comments SET UserId = @UserId, ImageId = @ImageId, Text = @Text, CommentData = @CommentData, ParentId = @ParentId WHERE Id = @Id and UserId = @UserId";
            connection.Execute(sql, comment);
        }

        public IEnumerable<Comment> GetAllCommentsForImage(long ImageId)
        {
            var sql = "SELECT * FROM Comments Where ImageId = @ImageId";
            List<Comment> listComments = connection.Query<Comment>(sql, new { ImageId }).ToList();
            return listComments;
        }

        public Comment Get(long ImageId)
        {
            var sql = @"SELECT *  FROM Comments
                      where Id = (select max(Id) from Comments)";
            Comment comm = connection.Query<Comment>(sql, new { ImageId }).FirstOrDefault();
            return comm;
        }
    }
}
