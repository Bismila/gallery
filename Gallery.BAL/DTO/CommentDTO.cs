using Gallery.DAL.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gallery.BAL.DTO
{
    public class CommentDTO
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string UserLogin { get; set; }

        public string UserPhoto { get; set; }

        public long ImageId { get; set; }

        public string Text { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime CommentData { get; set; }

        public long ParentId { get; set; }

        public bool IsEditComment { get; set; }
    }
}
