using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class UserBll
    {
        public static CUser[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT a.*,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode ORDER BY UserCode", out i);
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUser
                {
                    Id = Convert.ToInt16(row["Id"]),
                    UserCode = Convert.ToString(row["UserCode"]),
                    UserName = Convert.ToString(row["UserName"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName =Convert.ToString(row["GroupName"])
                }).ToArray();
        }

        /// <summary>
        /// 获取用户选项
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static CLenovoInputOption[] GetLenovoInputOption(IDal dal, string condition)
        {
            int i;
            var dt = dal.Select("SELECT Id,UserCode,UserName FROM tUser WHERE UserCode LIKE @Condition+'%' OR UserName LIKE '%'+@Condition +'%' ", out i,
                dal.CreateParameter("@Condition", condition));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CLenovoInputOption
                {
                    Value = Convert.ToString(row["UserCode"]),
                    Display = string.Format("{0}-{1}", Convert.ToString(row["UserCode"]), Convert.ToString(row["UserName"]))
                }).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public static int CountPeople(IDal dal, string groupCode)
        {
            int i;
            var c = dal.Select("SELECT COUNT(1) AS People FROM tUser WHERE GroupCode=@GroupCode", out i,
                dal.CreateParameter("@GroupCode", groupCode));
            return Convert.IsDBNull(c.Rows[0]["People"]) ? 0 : Convert.ToInt16(c.Rows[0]["People"]);
        }

        public static CUser Get(IDal dal, int id)
        {
            int i;
            var dt =
                dal.Select(
                    "SELECT a.*,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode where a.Id=@Id",
                    out i,
                    dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CUser
                {
                    Id = Convert.ToInt16(row["Id"]),
                    UserCode = Convert.ToString(row["UserCode"]),
                    UserName = Convert.ToString(row["UserName"]),
                    BuildDate = Convert.ToDateTime(row["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    BuildUser = Convert.ToString(row["BuildUser"]),
                    EditDate = Convert.ToDateTime(row["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]),
                    EditUser = Convert.ToString(row["EditUser"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    Enabled = Convert.ToBoolean(row["Enabled"]),
                    GroupCode = Convert.ToString(row["GroupCode"]),
                    GroupName =Convert.ToString(row["GroupName"])
                }).First();
        }

        public static bool Create(IDal dal, CUser user,string editUser)
        {
            int i;
            var pwd = MD5.Create().ComputeHash(Encoding.Default.GetBytes(user.UserCode+ user.Md5));
            var deptCode = dal.CreateParameter("@DeptCode", DbType.String);
            if (string.IsNullOrEmpty(user.DeptCode))
            {
                deptCode.Value = DBNull.Value;
            }
            else
            {
                deptCode.Value = user.DeptCode.Trim();
            }
            var groupCode = dal.CreateParameter("@GroupCode", DbType.String);
            if (string.IsNullOrEmpty(user.GroupCode))
            {
                groupCode.Value = DBNull.Value;
            }
            else
            {
                groupCode.Value = user.GroupCode.Trim();
            }
            dal.Execute("INSERT INTO tUser( UserCode ,UserName ,UPassword ,DeptCode ,GroupCode ,Enabled  ,BuildUser ,EditUser) VALUES  ( @UserCode ,@UserName,@UPassword, @DeptCode , @GroupCode, @Enabled , @BuildUser,@EditUser)", out i,
                dal.CreateParameter("@UserCode",user.UserCode.Trim()),
                dal.CreateParameter("@UserName",user.UserName.Trim()),
                dal.CreateParameter("@UPassword",pwd),
                deptCode,
                groupCode,
                dal.CreateParameter("@Enabled",user.Enabled),
                dal.CreateParameter("@BuildUser", editUser),
                dal.CreateParameter("@EditUser", editUser));
            if (i == 0) return false;
            var dt =
                dal.Select(
                    "SELECT a.Id,b.DeptName,c.GroupName,a.BuildUser,a.BuildDate,a.EditUser,a.EditDate,b.DeptName,c.GroupName FROM tUser a Left JOIN tDept b ON a.DeptCode=b.DeptCode Left JOIN tUserGroup c ON a.GroupCode=c.GroupCode where a.UserCode=@UserCode",
                    out i,
                    dal.CreateParameter("@UserCode", user.UserCode));
            if (i == 0) return false;
            user.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            user.BuildDate = Convert.ToDateTime(dt.Rows[0]["BuildDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            user.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            user.DeptName =Convert.ToString(dt.Rows[0]["DeptName"]);
            user.GroupName =Convert.ToString(dt.Rows[0]["GroupName"]);
            user.BuildUser = Convert.ToString(dt.Rows[0]["BuildUser"]);
            user.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]);
            return true;
        }

        public static bool Update(IDal dal, CUser user,string editUser)
        {
            int i;
            var deptCode = dal.CreateParameter("@DeptCode", DbType.String);
            if (string.IsNullOrEmpty(user.DeptCode))
            {
                deptCode.Value = DBNull.Value;
            }
            else
            {
                deptCode.Value = user.DeptCode.Trim();
            }
            var groupCode = dal.CreateParameter("@GroupCode", DbType.String);
            if (string.IsNullOrEmpty(user.GroupCode))
            {
                groupCode.Value = DBNull.Value;
            }
            else
            {
                groupCode.Value = user.GroupCode.Trim();
            }
            dal.Execute(
                "UPDATE tUser SET UserName=@UserName,DeptCode=@DeptCode,GroupCode=@GroupCode ,EditDate=GETDATE(),EditUser=@EditUser WHERE Id=@Id",
                out i,
                dal.CreateParameter("@UserName", user.UserName.Trim()),
                deptCode,
                groupCode,
                dal.CreateParameter("@EditUser", editUser),
                dal.CreateParameter("@Id", user.Id));
            if (i == 0) return false;
            var dt=dal.Select("SELECT Id,EditUser,EditDate FROM tUser WHERE Id=@Id", out i,
                dal.CreateParameter("@Id",user.Id));
            if (i == 0) return false;
            user.EditUser = Convert.ToString(dt.Rows[0]["EditUser"]);
            user.EditDate = Convert.ToDateTime(dt.Rows[0]["EditDate"]).ToString(ConfigurationManager.AppSettings["DateFormate"]);
            return true;
        }

        public static bool Delete(IDal dal, int id,out CUser hisUser)
        {
            int i;
            hisUser = Get(dal, id);
            if (hisUser == null) return false;
            dal.Execute("DELETE FROM tUser WHERE Id=@id",out i,
                dal.CreateParameter("@Id",id));
            return i == 1;
        }
    }
}