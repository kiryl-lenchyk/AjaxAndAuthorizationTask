using System;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User name is required")]
        [Display(Name = "User name*")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password*")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password*")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Email*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First name*")]
        public String FirstName { get; set; }
    }
}