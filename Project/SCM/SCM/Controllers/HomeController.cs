using System.Configuration;
using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ViewResult Index()
        {
            ViewBag.Title = ConfigurationManager.AppSettings["BrandName"]+"SCM";
            ViewBag.SignUser = HttpContext.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            return View();
        }

        public PartialViewResult HomePage()
        {
            return PartialView();
        }
    }
}
