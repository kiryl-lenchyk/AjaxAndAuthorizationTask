using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace MvcApplication.Infrasctracture
{
    public class CustomPrincipal : IPrincipal
    {

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public IIdentity Identity { get; private set; }

        public int UserId { get; set; }

        public string FirstName { get; set; }

        public IEnumerable<string> Roles { get; set; } 

        public bool IsInRole(string role)
        {
            if (Roles == null) return false;
            return Roles.Any(role.Contains);
        }

        
    }
}