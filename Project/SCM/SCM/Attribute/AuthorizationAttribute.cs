using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using CRM.Bll;
using CRM.Extend.HttpResponseMessages;
using CRM.Models;
using DAL;
using DAL.Interface;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;

namespace CRM.Attribute
{
    /// <summary>
    /// 表示需要用户登录才可以使用的特性
    /// <para>如果不需要处理用户登录，则请指定AllowAnonymousAttribute属性</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AuthorizationAttribute()
        {
            AuthUrl= ConfigurationManager.AppSettings["AuthUrl"];
            AuthSaveKey = ConfigurationManager.AppSettings["AuthSaveKey"];
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        public AuthorizationAttribute(String authUrl)
            : this()
        {
            _authUrl = authUrl;
        }

        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="authUrl"></param>
        /// <param name="saveKey">表示登录用来保存登陆信息的键名</param>
        public AuthorizationAttribute(String authUrl, String saveKey)
            : this(authUrl)
        {
            _authSaveKey = saveKey;
        }

        private String _authUrl = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录地址
        /// <para>如果web.config中未定义AuthUrl的值，则默认为/User/Login</para>
        /// </summary>
        public String AuthUrl
        {
            get { return _authUrl.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于验证用户登录信息的登录地址不能为空！");
                }
                _authUrl = value.Trim();
            }
        }

        private String _authSaveKey = String.Empty;
        /// <summary>
        /// 获取或者设置一个值，改值表示登录用来保存登陆信息的键名
        /// <para>如果web.config中未定义AuthSaveKey的值，则默认为LoginedUser</para>
        /// </summary>
        public String AuthSaveKey
        {
            get { return _authSaveKey.Trim(); }
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException("用于保存登陆信息的键名不能为空！");
                }
                _authSaveKey = value.Trim();
            }
        }

        /// <summary>
        /// 处理用户登录
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext == null)
            {
                throw new Exception("此特性只适合于Web应用程序使用！");
            }
            if (filterContext.HttpContext.Session == null)
            {
                throw new Exception("服务器Session不可用！");
            }
            if (filterContext.ActionDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof (AllowAnonymousAttribute), true))
                return;
            using (var dal =DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {

                var httpCookie = filterContext.HttpContext.Request.Cookies["Token"];
                if (filterContext.HttpContext.Session[_authSaveKey] != null)
                {
/*                    if (httpCookie != null) 
                    //更新Token
                    UpdateToken(filterContext, dal, (CAuthorityModel)filterContext.HttpContext.Session[_authSaveKey]);*/
                    return;
                }
                if (httpCookie != null)
                {
                    try
                    {
                        dal.Open();
                    }
                    catch
                    {
                        throw new HttpResponseException(new SystemExceptionMessage());
                    }
                    //存在Token，进行Token登录
                    var authorityModel = new CSign();

                    if (SignBll.Signin(dal, httpCookie.Values["User"],httpCookie.Values["Value"], authorityModel))
                    {
                        filterContext.HttpContext.Session.Add(ConfigurationManager.AppSettings["AuthSaveKey"], authorityModel);
                        //更新Token
                        UpdateToken(filterContext, dal, authorityModel);
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(_authUrl);
                    }
                    dal.Close();

                }
                else
                {
                    filterContext.Result = new RedirectResult(_authUrl);
                }
            }
        }

        /// <summary>
        /// 更新当前Token
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="dal"></param>
        /// <param name="authorityModel"></param>
        public void UpdateToken(AuthorizationContext filterContext, IDal dal, CSign authorityModel)
        {
            var token = Guid.NewGuid().ToString();
            filterContext.HttpContext.Response.Cookies["Token"].Values["User"] = authorityModel.UserCode;
            filterContext.HttpContext.Response.Cookies["Token"].Values["Value"] =token;
            filterContext.HttpContext.Response.Cookies["Token"].Expires = DateTime.Now.AddDays(30);
            SignBll.UpdateToken(dal, token, authorityModel.UserCode);
        }
    }

}