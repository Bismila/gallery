using System;

namespace Gallery.DAL.Models
{
    public class Comment
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public long ImageId { get; set; }

        public string Text { get; set; }

        public DateTime CommentData { get; set; }

        public long ParentId { get; set; }
    }
}
