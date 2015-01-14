using System;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class SignController : Controller
    {
        //
        // GET: /Sign/

        public ActionResult SignIn()
        {
            return View();
        }

        public PartialViewResult NoAuthority()
        {
            return PartialView();
        }

        public ViewResult Signout()
        {
            var httpCookie = HttpContext.Request.Cookies["Token"];
            if (httpCookie != null)
            {
                HttpContext.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(-1);
            }
            HttpContext.Session.RemoveAll();
            return View("Signin");
        }
    }
}
