using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gallery.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PhotoUser { get; set; }
        //public int RoleId { get; set; }

        public List<ImageModel> Images { get; set; }
        ////public List<Friend> Friends { get; set; }
        //public List<Role> Roles { get; set; }

        public UserModel()
        {
            Images = new List<ImageModel>();
            //Friends = new List<Friend>();
            //Roles = new List<Role>();
        }

    }
}