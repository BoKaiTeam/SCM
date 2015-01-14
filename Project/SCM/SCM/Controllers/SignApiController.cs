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
    public class SignApiController : ApiController
    {
        public CSign Post(CSign value)
        {
            using (var dal =DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                try
                {
                    dal.Open();
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}",value.UserCode,value.UserName),
                        LogContent = string.Format("{0}#{1}", "Signin", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                var tokenCookie = HttpContext.Current.Request.Cookies["Token"];
                if (value.Remain && tokenCookie != null && string.IsNullOrEmpty(value.UserCode) && string.IsNullOrEmpty(value.UPwd))
                {
                    //Token不为空 用户名和密码为空，则使用token登录
                    ok = SignBll.Signin(dal, tokenCookie.Values["User"],tokenCookie.Values["Value"], value);
                }
                else
                {
                    //使用用户名密码登录
                    ok=SignBll.Signin(dal, value);
                }
                if (!ok)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]] = value;
                //生成Token
                var token = Guid.NewGuid().ToString();
                SignBll.UpdateToken(dal,token,value.UserCode);
                HttpContext.Current.Response.Cookies["Token"].Values["User"] = value.UserCode;
                HttpContext.Current.Response.Cookies["Token"].Values["Value"] =token;

                HttpContext.Current.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(30);
                if (value.Remain) return value;
                HttpContext.Current.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(-1);
                SignBll.DropToken(dal, value.UserCode);
                dal.Close();
                return value;
            }
        }
    }
}
