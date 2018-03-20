using Gallery.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.WEB.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

       

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Login { get; set; }

        public string OldLogin { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name = "New Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public string PhotoUser { get; set; }

        public int RoleId { get; set; }

        public List<Image> Images { get; set; }
        public List<Friend> Friends { get; set; }
        public List<Role> Roles { get; set; }
        public string UpdateUserRole { get; set; }

        public EditUserViewModel()
        {
            Images = new List<Image>();
            Friends = new List<Friend>();
            Roles = new List<Role>();
        }
    }
}