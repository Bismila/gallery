using System;
using System.Collections.Generic;


namespace Gallery.DAL.Models
{
    public class Image
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public DateTime ImageDate { get; set; }

        public string PathImage { get; set; }

        public virtual User User { get; set; }
        public long UserId { get; set; }
        public string userName { get; set; }


    }
}
