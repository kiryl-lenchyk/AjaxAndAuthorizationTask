using System.Web.Mvc;
using MvcApplication.Infrasctracture;

namespace MvcApplication.Controllers
{
    public class BaseController : Controller
    {
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }
    }
}
