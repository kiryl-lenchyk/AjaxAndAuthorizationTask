using System.IO;
using System.Web;

namespace CastomHandler
{
    /// <summary>
    /// Summary description for Handler
    /// </summary>
    public class Handler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";

            try
            {
                using (StreamReader stream = new StreamReader(context.Server.MapPath("~/json.json")))
                {
                    context.Response.Write(stream.ReadToEnd());
                }
            }
            catch (IOException ex)
            {
                context.Response.Write(string.Format("ERROR{0}", ex.ToString()));
            }
            
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}