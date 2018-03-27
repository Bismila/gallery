using Gallery.DAL.Models;
using System;
using System.Collections.Generic;

namespace Gallery.BAL.DTO
{
    public class CreateUpdateDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime ImageDate { get; set; }

        public string PathImage { get; set; }

        public User User { get; set; }

        public long UserId { get; set; }

        public long CountLikes { get; set; }

        public bool isLike { get; set; }
        public List<CommentDTO> Comments { get; set; }

        public CreateUpdateDto()
        {
            Comments = new List<CommentDTO>();
        }
    }
}
