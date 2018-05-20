using System.Text;

namespace PVLog.Controllers
{
    using System.Web.Mvc;

    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        public ActionResult API()
        {
            return View();
        }

        public ActionResult Impressum()
        {
            return View();
        }

        public ActionResult Datenschutz()
        {
            return View();
        }
        
        public ActionResult Robots()
        {
            StringBuilder stringBuilder = new StringBuilder();
        
            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: /");
        
            return this.Content(stringBuilder.ToString(), "text/plain", Encoding.UTF8);
        }
    }
}