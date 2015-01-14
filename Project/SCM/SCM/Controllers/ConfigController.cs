using System.Web.Mvc;
using CRM.Attribute;

namespace CRM.Controllers
{
    [Authorization]
    public class ConfigController : Controller
    {
        //
        // GET: /Config/

        public PartialViewResult DeptManage()
        {
            ViewBag.MenuCategory = "系统设置";
            ViewBag.Menu = "部门设置";
            return PartialView();
        }

        public PartialViewResult UserGroupManage()
        {
            ViewBag.MenuCategory = "系统设置";
            ViewBag.Menu = "用户组";
            return PartialView();
        }

        public PartialViewResult UserManage()
        {
            ViewBag.MenuCategory = "系统设置";
            ViewBag.Menu = "用户管理";
            return PartialView();
        }
    }
}
