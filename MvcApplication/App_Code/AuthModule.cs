using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MvcApplication.Infrasctracture;
using MvcApplication.Models;
using Newtonsoft.Json;

namespace MvcApplication
{
    public class AuthModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += ContextOnPostAuthenticateRequest;
        }

        private void ContextOnPostAuthenticateRequest(object sender, EventArgs eventArgs)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpCookie authCookie = application.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket;
                try
                {
                    authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                }
                catch (Exception)
                {
                    return;
                }

                CustomPrincipalSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(authTicket.UserData);
                CustomPrincipal newUser = new CustomPrincipal(authTicket.Name)
                {
                    UserId = serializeModel.UserId,
                    FirstName = serializeModel.FirstName,
                    Roles = serializeModel.Roles
                };

                HttpContext.Current.User = newUser;
            }
 
        }

        private void ContextOnEndRequest(object sender, EventArgs eventArgs)
        {
            //HttpApplication application = (HttpApplication)sender;
         
            //application.Response.Write(string.Format("<span>Request end at {0}</span>", DateTime.Now));
        }

        private void ContextOnBeginRequest(object sender, EventArgs eventArgs)
        {
            //HttpApplication application = (HttpApplication) sender;
            //application.Response.Write(string.Format("<span>Request start at {0}</span>", DateTime.Now));
        }

        public void Dispose()
        {
           
        }
    }
}