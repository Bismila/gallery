using System.Collections.Generic;
using Gallery.DAL.Models;
namespace Gallery.BAL.DTO
{
    public class UserDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PhotoUser { get; set; }

        public long RoleId { get; set; }

        public List<Image> Images { get; set; }

        public List<Friend> Friends { get; set; }

        public List<Role> Roles { get; set; }

        public UserDTO()
        {
            Images = new List<Image>();
            Friends = new List<Friend>();
            Roles = new List<Role>();
        }
    }
}
