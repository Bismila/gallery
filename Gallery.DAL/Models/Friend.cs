using System.Collections.Generic;

namespace Gallery.DAL.Models
{
    public class Friend
    {
        public long UserId { get; set; }
        public long FriendId { get; set; }
        public virtual List<User> Users { get; set; }
        public Friend()
        {
            Users = new List<User>();
        }
    }
}
