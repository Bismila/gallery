using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.DAL.Models
{
    public class Like
    {
        public long ImageId { get; set; }
        public long UserId { get; set; }
    }
}
