using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcApplication.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public String Username { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }

        public String FirstName { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}