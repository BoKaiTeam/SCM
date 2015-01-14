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
    public class UserGroupApiController : ApiController
    {
        // GET api/usergroupapi
        public IEnumerable<CUserGroup> Get()
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CUserGroup[] userGroups;
                try
                {
                    dal.Open();
                    userGroups = UserGroupBll.List(dal);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UserGroup.List", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (userGroups == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return userGroups;
            }
        }

        // GET api/usergroupapi/5
        public CUserGroup Get(int id)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (
                var dal =
                    DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0)
                )
            {
                CUserGroup userGroup;
                try
                {
                    dal.Open();
                    userGroup = UserGroupBll.Get(dal, id);
                    dal.Close();
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}","UserGroup.List",ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (userGroup == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return userGroup;
            }
        }

        /// <summary>
        /// 过滤列表
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public IEnumerable<CUserGroup> Get(string condition)
        {
            var user = (CSign)HttpContext.Current.Session[ConfigurationManager.AppSettings["AuthSaveKey"]];
            if (user == null)
            {
                throw new HttpResponseException(new SiginFailureMessage());
            }
            using (var dal = DalBuilder.CreateDal(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString, 0))
            {
                CUserGroup[] userGroups;
                try
                {
                    dal.Open();
                    userGroups = UserGroupBll.Filter(dal,condition);
                    dal.Close();
                }
                catch (Exception ex)
                {
                    LogBll.Write(dal, new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UserGroup.Filter", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (userGroups == null)
                {
                    throw new HttpResponseException(new DataNotFoundMessage());
                }
                return userGroups;
            }
        }

        // POST api/usergroupapi
        public CUserGroup Post(CUserGroup value)
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
                    ok = UserGroupBll.Create(dal, value, string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UserGroup.Post", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent=string.Format("新建用户组{0}-{1}",value.GroupCode,value.GroupName),
                        LogType = LogType.操作失败
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("新建用户组{0}-{1}", value.GroupCode, value.GroupName),
                    LogType = LogType.操作成功
                });
                dal.Close();
                return value;
            }
        }

        // PUT api/usergroupapi/5
        public CUserGroup Put(int id, CUserGroup value)
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
                    ok=UserGroupBll.Update(dal, value,string.Format("{0}-{1}", user.UserCode, user.UserName));
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UserGroup.Put", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("修改用户组{0}-{1}", value.GroupCode, value.GroupName),
                        LogType = LogType.操作失败
                    });
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("修改用户组{0}-{1}", value.GroupCode, value.GroupName),
                    LogType = LogType.操作成功
                });
                dal.Close();
                return value;
            }
        }

        // DELETE api/usergroupapi/5
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
                CUserGroup hisUserGroup;
                try
                {
                    dal.Open();
                    ok = UserGroupBll.Delete(dal, id,out hisUserGroup);
                }
                catch(Exception ex)
                {
                    LogBll.Write(dal,new CLog
                    {
                        LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                        LogContent = string.Format("{0}#{1}", "UserGroup.Delete", ex.Message),
                        LogType = LogType.系统异常
                    });
                    throw new HttpResponseException(new SystemExceptionMessage());
                }
                if (!ok)
                {
                    if (hisUserGroup != null)
                    {
                        LogBll.Write(dal,new CLog
                        {
                            LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                            LogContent = string.Format("删除用户组{0}-{1}", hisUserGroup.GroupCode, hisUserGroup.GroupName),
                            LogType = LogType.操作失败
                        });
                    }
                    throw new HttpResponseException(new DealFailureMessage());
                }
                LogBll.Write(dal,new CLog
                {
                    LogUser = string.Format("{0}-{1}", user.UserCode, user.UserName),
                    LogContent = string.Format("新建用户组{0}-{1}", hisUserGroup.GroupCode, hisUserGroup.GroupName),
                    LogType = LogType.操作成功
                });
                dal.Close();
            }
        }
    }
}
