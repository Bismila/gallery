using Gallery.BAL.DTO;
using Gallery.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.WEB.Models
{
    public class ImageViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime ImageDate { get; set; }

        public string PathImage { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public long CountLikes { get; set; }

        public bool isLike { get; set; }

        public List<CommentDTO> Comments { get; set; }

        public ImageViewModel()
        {
            Comments = new List<CommentDTO>();
        }

    }
}