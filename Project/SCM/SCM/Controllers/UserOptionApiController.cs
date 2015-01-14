using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Http;
using CRM.Bll;
using CRM.Extend.HttpResponseMessages;
using CRM.Models;
using DAL;

namespace CRM.Controllers
{
    public class UserOptionApiController : ApiController
    {
        // GET api/userselecterapi
        public IEnumerable<CLenovoInputOption> Get(string condition)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CLenovoInputOption[] userSelecterItems;
                try
                {
                    dal.Open();
                    userSelecterItems = UserBll.GetLenovoInputOption(dal,condition);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "Dept.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (userSelecterItems == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return userSelecterItems;
            }
        }
    }
}
