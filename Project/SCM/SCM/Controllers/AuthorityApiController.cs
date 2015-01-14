using System;
using System.Configuration;
using System.Web;
using System.Web.Http;
using CRM.Bll;
using CRM.Extend.HttpResponseMessages;
using CRM.Models;
using DAL;

namespace CRM.Controllers
{
    public class AuthorityApiController : ApiController
    {
        public CAuthorityModel Post(CAuthorityModel value)
        {
            using (var dal =DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                try
                {
                    dal.Open();
                }
                catch
                {
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                var httpCookie = HttpContext.Current.Request.Cookies["Token"];
                if (value.Remain && httpCookie != null && string.IsNullOrEmpty(value.UserCode) && string.IsNullOrEmpty(value.UPwd))
                {
                    //Token不为空 用户名和密码为空，则使用token登录
                    ok=AuthorityBll.Signin(dal, httpCookie.Value, value);
                }
                else
                {
                    //使用用户名密码登录
                    ok=AuthorityBll.Signin(dal, value);
                }
                if (!ok)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                HttpContext.Current.Session["SignUser"] = value;
                //生成Token
                var token = Guid.NewGuid().ToString();
                AuthorityBll.UpdateToken(dal,token,value.UserCode);
                HttpContext.Current.Response.Cookies["Token"].Value =token;
                HttpContext.Current.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(30);
                if (value.Remain) return value;
                HttpContext.Current.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(-1);
                AuthorityBll.DropToken(dal, value.UserCode);
                return value;
            }
        }
    }
}
