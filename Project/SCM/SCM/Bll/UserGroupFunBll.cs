using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserGroupFunBll
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public static CUserGroupFun[] List(IDal dal, string groupCode)
        {
            int i;
            DataTable dt;
            if (groupCode.Equals("*"))
            {
                dt = dal.Select("select 0 as Id ,'' as GroupCode,FunCode,FunName,GroupType,0 as Queriable,0 as Creatable,0 as Changable,0 as Deletable,0 as Checkable from tFunction Order By FunCode", out i);
            }
            else
            {
                dt =
                    dal.Select(
                        "select ISNULL(b.Id,0) as Id,ISNULL( b.GroupCode,@GroupCode) as GroupCode,a.FunCode,a.FunName,ISNULL( b.Queriable,0) as Queriable,ISNULL( b.Creatable,0) as Creatable,ISNULL( b.Changable,0) as Changable,ISNULL( b.Deletable,0) as Deletable,ISNULL( b.Checkable,0) as Checkable, a.GroupType from tFunction a left join tUserGroupFun b on a.FunCode=b.FunCode WHERE GroupCode=@GroupCode or GroupCode is null Order By a.FunCode",
                        out i,
                        dal.CreateParameter("@GroupCode", groupCode));
            }

            return (from DataRow row in dt.Rows
                    select new CUserGroupFun
                    {
                        Id = Convert.ToInt16(row["Id"]),
                        GroupCode = Convert.ToString(row["GroupCode"]),
                        FunCode = Convert.ToString(row["FunCode"]),
                        FunName = Convert.ToString(row["FunName"]),
                        GroupType = (GroupType)Convert.ToInt16(row["GroupType"]),
                        Queriable = Convert.ToBoolean(row["Queriable"]),
                        Changable = Convert.ToBoolean(row["Changable"]),
                        Deletable = Convert.ToBoolean(row["Deletable"]),
                        Checkable = Convert.ToBoolean(row["Checkable"]),
                        Creatable = Convert.ToBoolean(row["Creatable"])
                    }).ToArray();
        }

        /// <summary>
        /// 计算用户组中功能数量
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public static int CountGroupFun(IDal dal, string groupCode)
        {
            int i;
            var dt = dal.Select("select count(1) as Fun from tFunction a left join tUserGroupFun b on a.FunCode =b.FunCode where a.Enabled=1 and b.Queriable=1 and b.GroupCode=@GroupCode", out i,
                dal.CreateParameter("@GroupCode", groupCode));
            return Convert.ToInt16(dt.Rows[0]["Fun"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroupFun"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CUserGroupFun userGroupFun,string user)
        {
            int i;
            dal.Execute("INSERT INTO tUserGroupFun( GroupCode ,FunCode ,Queriable ,Creatable ,Changable ,Deletable ,Checkable , BuildUser,EditUser) VALUES  ( @GroupCode , @FunCode ,@Queriable ,@Creatable ,@Changable ,@Deletable ,@Checkable,@BuildUser,@EditUser )", out i,
                dal.CreateParameter("@GroupCode",userGroupFun.GroupCode.Trim()),
                dal.CreateParameter("@FunCode",userGroupFun.FunCode.Trim()),
                dal.CreateParameter("@Queriable",userGroupFun.Queriable),
                dal.CreateParameter("@Creatable",userGroupFun.Creatable),
                dal.CreateParameter("@Changable",userGroupFun.Changable),
                dal.CreateParameter("@Deletable",userGroupFun.Deletable),
                dal.CreateParameter("@Checkable",userGroupFun.Checkable),
                dal.CreateParameter("@BuildUser",user),
                dal.CreateParameter("@EditUser",user));
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id FROM tUserGroupFun WHERE GroupCode=@GroupCode AND FunCode=@FunCode", out i,
                dal.CreateParameter("@GroupCode", userGroupFun.GroupCode),
                dal.CreateParameter("@FunCode", userGroupFun.FunCode));
            if (i == 0) return false;
            userGroupFun.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroupFun"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CUserGroupFun userGroupFun,string user)
        {
            int i;
            dal.Execute("UPDATE tUserGroupFun SET Changable=@Changable,Checkable=@Checkable,Deletable=@Deletable,Queriable=@Queriable,Creatable=@Creatable WHERE Id=@Id",out i,
                dal.CreateParameter("@Changable",userGroupFun.Changable),
                dal.CreateParameter("@Checkable",userGroupFun.Checkable),
                dal.CreateParameter("@Deletable",userGroupFun.Deletable),
                dal.CreateParameter("@Queriable",userGroupFun.Queriable),
                dal.CreateParameter("@Creatable",userGroupFun.Creatable),
                dal.CreateParameter("@Id",userGroupFun.Id)
                );
            return i != 0 || Create(dal, userGroupFun, user);
        }

    }
}