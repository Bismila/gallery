using System.Collections.Generic;

namespace Gallery.DAL.Models
{
    public class User
    {
        public User()
        {
            Friends = new List<Friend>();
            Images = new List<Image>();
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhotoUser { get; set; }

        public long RoleId { get; set; }

        public virtual List<Friend> Friends { get; set; }

        public virtual List<Image> Images { get; set; }
    }
}