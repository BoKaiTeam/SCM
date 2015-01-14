using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/

        public PartialViewResult CustomerRegister()
        {
            ViewBag.MenuCategory = "客户管理";
            ViewBag.Menu = "客户登记";
            return PartialView();
        }

        public PartialViewResult ContactRegister(string customerId)
        {
            ViewBag.MenuCategory = "客户管理";
            ViewBag.Menu = "联系人登记";
            ViewBag.CustomerId = customerId;
            return PartialView();
        }

        public PartialViewResult PublicCustomer()
        {
            ViewBag.MenuCategory = "客户管理";
            ViewBag.Menu = "公共客户";
            return PartialView();
        }

        public PartialViewResult PrivateCustomer()
        {
            ViewBag.MenuCategory = "客户管理";
            ViewBag.Menu = "私有客户";
            return PartialView();
        }
    }
}
