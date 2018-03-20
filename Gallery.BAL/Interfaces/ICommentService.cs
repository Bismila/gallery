using Gallery.BAL.DTO;
using System.Collections.Generic;

namespace Gallery.BAL.Interfaces
{
    public interface ICommentService
    {
        CommentDTO AddComment(CommentDTO comment);

        void DeleteComment(long Id);

        void UpdateComment(CommentDTO comment);

        IEnumerable<CommentDTO> GetAllCommentsForImage(long ImageId);

        CommentDTO Get(long ImageId);
    }
}
