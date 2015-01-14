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
    public class UserApiController : ApiController
    {
        // GET api/userapi
        public IEnumerable<CUser> Get()
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CUser[] users;
                try
                {
                    dal.Open();
                    users = UserBll.List(dal);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "User.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if(users==null) throw new HttpResponseException(new DataNotFoundMessage());
                return users;
            }
        }

        // GET api/userapi/5
        public CUser Get(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CUser cUser;
                try
                {
                    dal.Open();
                    cUser = UserBll.Get(dal, id);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "User.Get", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if(cUser==null) throw new HttpResponseException(new DataNotFoundMessage());
                return cUser;
            }
        }

        // POST api/userapi
        public CUser Post(CUser value)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                try
                {
                    dal.Open();
                    ok = UserBll.Create(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "User.Post", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("新建用户{0}-{1}",value.UserCode,value.UserName),
                        LogType = LogType.操作失败
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("新建用户{0}-{1}", value.UserCode, value.UserName),
                    LogType = LogType.操作成功
                });
                dal.Close();
                return value;
            }
        }

        // PUT api/userapi/5
        public CUser Put(int id, CUser value)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                try
                {
                    dal.Open();
                    ok = UserBll.Update(dal, value,string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "User.Put", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("修改用户{0}-{1}", value.UserCode, value.UserName),
                        LogType = LogType.操作失败
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("修改用户{0}-{1}", value.UserCode, value.UserName),
                    LogType = LogType.操作成功
                });
                dal.Close();
                return value;
            }
        }

        // DELETE api/userapi/5
        public void Delete(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                bool ok;
                CUser hisUser;
                try
                {
                    dal.Open();
                    ok = UserBll.Delete(dal, id,out hisUser);
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "User.Delete", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    if (hisUser != null)
                    {
                        LogBll.Write(dal,new CLog
                        {
                            LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                            LogContent = string.Format("删除用户{0}-{1}", hisUser.UserCode, hisUser.UserName),
                            LogType = LogType.操作失败
                        });
                    }
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("删除用户{0}-{1}", hisUser.UserCode, hisUser.UserName),
                    LogType = LogType.操作成功
                });
                dal.Close();
            }
        }
    }
}
