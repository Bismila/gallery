using Gallery.BAL.DTO;
using Gallery.DAL.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gallery.WEB.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Login is required")]
        public string Login { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter valid email.")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords don't match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string PhotoUser { get; set; }

        public int RoleId { get; set; }

        public List<CreateUpdateDto> Images { get; set; }
        public List<Friend> Friends { get; set; }
        public List<Role> Roles { get; set; }

        public UserViewModel()
        {
            Images = new List<CreateUpdateDto>();
            Friends = new List<Friend>();
            Roles = new List<Role>();
        }
    }
}