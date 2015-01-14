using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserGroupBll
    {
        /// <summary>
        /// 列表用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <returns></returns>
        public static CUserGroup[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroup Order by GroupCode", out i);
            if (i == 0)
            {
                return null;
            }
            return (from DataRow row in dt.Rows
                select new CUserGroup
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"]),
                    GroupType = (GroupType)Convert.ToInt16(row["GroupType"]),
                    People = UserBll.CountPeople(dal,Convert.ToString(row["GroupCode"])),
                    Fun = UserGroupFunBll.CountGroupFun(dal, Convert.ToString(row["GroupCode"]))
                }).ToArray();
        }

        /// <summary>
        /// 获取指定用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CUserGroup Get(IDal dal,int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tUserGroup WHERE Id=@Id", out i,
                dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUserGroup
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"]),
                    GroupType = (GroupType)Convert.ToInt16(row["GroupType"]),
                    People = UserBll.CountPeople(dal, Convert.ToString(row["GroupCode"])),
                    Fun = UserGroupFunBll.CountGroupFun(dal, Convert.ToString(row["GroupCode"]))
                }).First();
        }

        /// <summary>
        /// 筛选用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static CUserGroup[] Filter(IDal dal, string condition)
        {
            int i;
            var dt = dal.Select(" SELECT * FROM tUserGroup WHERE GroupCode LIKE @Condition+'%' OR GroupName LIKE '%'+@Condition +'%' ", out i,
                dal.CreateParameter("@Condition", condition));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUserGroup
                {
                    Id = Convert.ToInt16(row["Id"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName = Convert.ToString(row["GroupName"]),
                    GroupType = (GroupType) Convert.ToInt16(row["GroupType"]),
                    People = UserBll.CountPeople(dal, Convert.ToString(row["GroupCode"])),
                    Fun = UserGroupFunBll.CountGroupFun(dal, Convert.ToString(row["GroupCode"]))
                }).ToArray();
        }

        /// <summary>
        /// 新建用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroup"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CUserGroup userGroup,string user)
        {
            int i;
            dal.BeginTran();
            dal.Execute(
                "INSERT INTO tUserGroup( GroupCode ,GroupName ,BuildUser ,EditUser,GroupType) VALUES  ( @GroupCode , @GroupName,@BuildUser,@EditUser,@GroupType)",
                out i,
                dal.CreateParameter("@GroupCode", userGroup.GroupCode.Trim()),
                dal.CreateParameter("@GroupName", userGroup.GroupName.Trim()),
                dal.CreateParameter("@GroupType", (short) (userGroup.GroupType)),
                dal.CreateParameter("@BuildUser", user),
                dal.CreateParameter("@EditUser", user)
                );

            if (i == 0)return false;
            var dt = dal.Select("SELECT Id FROM tUserGroup WHERE GroupCode =@GroupCode", out i,
                dal.CreateParameter("@GroupCode", userGroup.GroupCode));
            if (i == 0) return false;
            userGroup.Id = Convert.ToInt16(dt.Rows[0]["Id"]);

            foreach (var fun in userGroup.GroupFun)
            {
                fun.GroupCode = userGroup.GroupCode;
                UserGroupFunBll.Create(dal, fun, user);
            }
            dal.CommitTran();
            userGroup.Fun = UserGroupFunBll.CountGroupFun(dal, userGroup.GroupCode);
            userGroup.People = UserBll.CountPeople(dal, userGroup.GroupCode);
            return true;
        }

        /// <summary>
        /// 修改用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="userGroup"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CUserGroup userGroup,string user)
        {
            int i;
            dal.BeginTran();
            dal.Execute("UPDATE tUserGroup SET GroupName=@GroupName,EditUser=@EditUser,EditDate=GETDATE(),GroupType=@GroupType WHERE Id=@Id", out i,
                dal.CreateParameter("@GroupName",userGroup.GroupName.Trim()),
                dal.CreateParameter("@EditUser",user),
                dal.CreateParameter("@Id",userGroup.Id),
                dal.CreateParameter("@GroupType",userGroup.GroupType));
            if (i == 0) return false;

            foreach (var fun in userGroup.GroupFun)
            {
                UserGroupFunBll.Update(dal, fun, user);
            }
            dal.CommitTran();
            userGroup.Fun = UserGroupFunBll.CountGroupFun(dal, userGroup.GroupCode);
            userGroup.People = UserBll.CountPeople(dal, userGroup.GroupCode);
            return true;
        }

        /// <summary>
        /// 删除用户组
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <param name="hisUserGroup"></param>
        /// <returns></returns>
        public static bool Delete(IDal dal, int id ,out CUserGroup hisUserGroup)
        {
            int i;
            hisUserGroup = Get(dal, id);
            if (hisUserGroup == null) return false;

            dal.BeginTran();
            dal.Execute("UPDATE tUser SET GroupCode=null WHERE GroupCode=@GroupCode",out i,
                dal.CreateParameter("@GroupCode",hisUserGroup.GroupCode));
            dal.Execute("DELETE FROM tUserGroupFun WHERE GroupCode=@GroupCode",out i,
                dal.CreateParameter("@GroupCode",hisUserGroup.GroupCode));
            dal.Execute("DELETE FROM tUserGroup WHERE Id=@Id",out i,
                dal.CreateParameter("@Id",id));
            dal.CommitTran();
            return true;
        }
    }
}