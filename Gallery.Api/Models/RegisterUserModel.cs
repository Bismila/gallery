using Gallery.DAL.Models;
using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Gallery.Api.Models
{
    public class RegisterUserModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string PhotoUser { get; set; }
        //public int RoleId { get; set; }

       

    }
   
}


