using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using MvcApplication.Infrasctracture;
using MvcApplication.Models;
using MvcApplication.ViewModels;


namespace MvcApplication.Controllers
{
    public class HomeController : BaseController
    {

        private static readonly int PageSize;
        
        private static readonly List<string> Comments = new List<string>(); 

        static HomeController()
        {
            if (!Int32.TryParse(WebConfigurationManager.AppSettings["PageSize"],
                out PageSize))
            {
                PageSize = 10;
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorize]
        public ActionResult AutorisedAction()
        {
            return View();
        }

        public async Task<ActionResult> JsonActionPartial(int? page)
        {
            JsonActionPageViewModel viewModel;
            try
            {
                viewModel = await GetJsonActionModel(page);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                viewModel = new JsonActionPageViewModel(){Contacts = new List<Contact>()};
            }
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("_AjaxActionPage", viewModel);
            }
            return View(viewModel);

        }

        public async Task<ActionResult> JsonActionJavaScript(int? page)
        {
            JsonActionPageViewModel viewModel;
            try
            {
                viewModel = await GetJsonActionModel(page);
            }
            catch (Exception ex)
            {
                if (Request.IsAjaxRequest())
                {
                    return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
                }
                ViewBag.Error = ex.Message;
                viewModel = new JsonActionPageViewModel() { Contacts = new List<Contact>() };
            }

            if (Request.IsAjaxRequest())
            {
                return
                    Json(
                        new
                        {
                            contact = viewModel.Contacts,
                            pageNumber = viewModel.PageNumber,
                            pagesCount = viewModel.PagesCount
                        }, JsonRequestBehavior.AllowGet);
            }
            return View(viewModel);
        }

        public ActionResult AddComment(string comment)
        {
            Comments.Add(comment);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Comment", comment);
            }
            return RedirectToAction("JsonActionPartial");

        }

        public ActionResult AddCommentJson(string comment)
        {
            Comments.Add(comment);
            if (Request.IsAjaxRequest())
            {
                return Json(new { comment});
            }
            return RedirectToAction("JsonActionJavaScript");
        }
        

        private async Task<JsonActionPageViewModel> GetJsonActionModel(int? page)
        {
            int pageNumber = page ?? 1;

            IList<string> enumerable = await GetJsonStringFromCache();
            IList<Contact> resultJsons = enumerable.Skip((pageNumber - 1)*PageSize)
                .Take(PageSize).Select(Deserialize<Contact>).ToList();


            int contactsCount = enumerable.Count();
            int pagesCount = contactsCount/PageSize + (contactsCount%PageSize == 0 ? 0 : 1);

            return new JsonActionPageViewModel
            {
                PageNumber = pageNumber,
                PagesCount = pagesCount,
                Contacts = resultJsons,
                Comments = Comments
            };
        }

        private async Task<IList<string>> GetJsonStringFromCache()
        {
            if (HttpContext.Cache["Json"] == null)
            {
                HttpContext.Cache.Insert("Json", await LoadJsonStringsAsync(), new CacheDependency(Server.MapPath("~/json.json")));
            }
            return (IList<string>) HttpContext.Cache["Json"];

        }
        
        private async Task<IList<string>> LoadJsonStringsAsync()
        {
            var result = new List<string>();
            
            using (var json = new StreamReader(new FileStream(Server.MapPath("~/json.json"), FileMode.Open)))
            {
                await JsonReader.RemoveArrayOpenChar(json);
                while (!(await JsonReader.IsEnd(json)))
                {
                    result.Add( await JsonReader.ReadOneArrayItem(json));
                }
            }
            return result;
        }

        private static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }


        
    }
}
