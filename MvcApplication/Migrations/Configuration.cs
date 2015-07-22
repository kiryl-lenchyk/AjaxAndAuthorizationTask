using System.Collections.Generic;
using MvcApplication.Models;

namespace MvcApplication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MvcApplication.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MvcApplication.Models.DataContext context)
        {
            Role role1 = new Role { RoleName = "Admin" };
            Role role2 = new Role { RoleName = "User" };

            User user1 = new User
            {
                Username = "admin",
                Email = "admin@ymail.com",
                FirstName = "Admin",
                Password = "123456",
                Roles = new List<Role>()
            };
            User user2 = new User
            {
                Username = "user1",
                Email = "user1@ymail.com",
                FirstName = "User1",
                Password = "123456",
                Roles = new List<Role>()
            };
            user1.Roles.Add(role1);
            user2.Roles.Add(role2);
            context.Users.Add(user1);
            context.Users.Add(user2);
        }
    }
}
