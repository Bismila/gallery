using Gallery.DAL.Models;
using System.Collections.Generic;


namespace Gallery.DAL.IRepository
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        void DeleteComment(long Id);
        void UpdateComment(Comment comment);
        IEnumerable<Comment> GetAllCommentsForImage(long ImageId);
        Comment Get(long ImageId);
    }
}
