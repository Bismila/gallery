using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gallery.Api.Models
{
    public class ImageModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime ImageDate { get; set; }

        public string PathImage { get; set; }

        //public User User { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public long CountLikes { get; set; }

        public bool isLike { get; set; }

        //public List<CommentDTO> Comments { get; set; }

        //public ImageViewModel()
        //{
        //    Comments = new List<CommentDTO>();
        //}

    }
}