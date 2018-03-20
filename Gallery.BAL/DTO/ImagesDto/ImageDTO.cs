using Gallery.DAL.Models;
using System;

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
    }
}
