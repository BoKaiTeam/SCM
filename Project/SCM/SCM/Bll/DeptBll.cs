using System;
using System.Data;
using System.Linq;
using CRM.Models;
using DAL.Interface;

namespace CRM.Bll
{
    public class DeptBll
    {
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        public static CDept[] List(IDal dal)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tDept ORDER BY DeptCode", out i);
            if (i == 0)
            {
                return null;
            }
            var people = CountPeople(dal);
            return (from DataRow row in dt.Rows
                where Convert.ToString(row["DeptCode"]) == "root"
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode =Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    ParentCode =Convert.ToString(row["ParentCode"]),
                    Childs = GetChilds(Convert.ToString(row["DeptCode"]), dt,people),
                    People = people.Any(p => p.Id == Convert.ToInt16(row["Id"]))?people.First(p=>p.Id==Convert.ToInt16(row["Id"])).People:0
                }).ToArray();
        }

        private static CDept[] CountPeople(IDal dal)
        {
            int i;
            var dt = dal.Select("select a.Id,count(1) as People from tDept a , tUser b where a.DeptCode=b.DeptCode group by a.Id order by a.Id", out i);
            return (from DataRow row in dt.Rows
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    People = Convert.ToInt16(row["People"])
                }).ToArray();
        }

        /// <summary>
        /// 获取指定节点的子节点
        /// </summary>
        /// <returns></returns>
        private static CDept[] GetChilds(string parentCode,DataTable dt,CDept[] people)
        {
            if (parentCode == null) return null;
            return (from DataRow row in dt.Rows
                where Convert.ToString(row["ParentCode"]) == parentCode
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode =Convert.ToString(row["DeptCode"]),
                    DeptName =Convert.ToString(row["DeptName"]),
                    ParentCode =Convert.ToString(row["ParentCode"]),
                    Childs = GetChilds(Convert.ToString(row["DeptCode"]), dt,people),
                    People = people.Any(p => p.Id == Convert.ToInt16(row["Id"])) ? people.First(p => p.Id == Convert.ToInt16(row["Id"])).People : 0
                }
                ).ToArray();
        }

        /// <summary>
        /// 读取指定Dept
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static CDept Get(IDal dal, int id)
        {
            int i;
            var dt = dal.Select("SELECT * FROM tDept WHERE Id=@Id", out i,
                dal.CreateParameter("@Id", id));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    ParentCode = Convert.ToString(row["ParentCode"])
                }).First();
        }

        /// <summary>
        /// 筛选的部门
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="condition">条件，Code或者Name</param>
        /// <returns></returns>
        public static CDept[] Filter(IDal dal, string condition)
        {
            int i;
            var dt = dal.Select(" SELECT * FROM tDept WHERE DeptCode LIKE @Condition+'%' OR DeptName LIKE '%'+@Condition +'%' ", out i,
                dal.CreateParameter("@Condition", condition));
            if (i == 0) return null;
            return (from DataRow row in dt.Rows
                select new CDept
                {
                    Id = Convert.ToInt16(row["Id"]),
                    DeptCode = Convert.ToString(row["DeptCode"]),
                    DeptName = Convert.ToString(row["DeptName"]),
                    ParentCode = Convert.ToString(row["ParentCode"])
                }).ToArray();
        }

        /// <summary>
        /// 新建部门
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="dept"></param>
        /// <param name="editUser"></param>
        /// <returns></returns>
        public static bool Create(IDal dal, CDept dept,string editUser)
        {
            int i;
            dal.Execute("INSERT INTO tDept( DeptCode ,DeptName ,ParentCode ,BuildUser ,EditUser) VALUES  ( @DeptCode ,@DeptName ,@ParentCode ,@BuildUser ,@EditUser )",out i,
                dal.CreateParameter("@DeptCode",dept.DeptCode.Trim()),
                dal.CreateParameter("@DeptName",dept.DeptName.Trim()),
                dal.CreateParameter("@ParentCode",dept.ParentCode.Trim()),
                dal.CreateParameter("@BuildUser", editUser),
                dal.CreateParameter("@EditUser", editUser));
            if (i == 0) return false;
            var dt = dal.Select("SELECT Id FROM dbo.tDept WHERE DeptCode=@DeptCode",out i,
                dal.CreateParameter("@DeptCode",dept.DeptCode));
            if (i == 0) return false;
            dept.Id = Convert.ToInt16(dt.Rows[0]["Id"]);
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dal"></param>
        /// <param name="dept"></param>
        /// <param name="editUser"></param>
        /// <returns></returns>
        public static bool Update(IDal dal, CDept dept, string editUser)
        {
            int i;
            dal.Execute("UPDATE tDept SET DeptName=@DeptName ,EditDate=GETDATE(),EditUser=@EditUser WHERE Id=@Id", out i,
                dal.CreateParameter("@DeptName",dept.DeptName.Trim()),
                dal.CreateParameter("@EditUser", editUser),
                dal.CreateParameter("@Id",dept.Id));
            return i == 1;
        }

        public static bool Delete(IDal dal, int id,out CDept hisDept)
        {
            int i;
            hisDept = Get(dal, id);
            if (hisDept == null) return false;
            dal.BeginTran();
            dal.Execute("UPDATE tUser SET DeptCode=null WHERE DeptCode=@DeptCode", out i,
                dal.CreateParameter("@DeptCode",hisDept.DeptCode));
            dal.Execute("DELETE FROM tDept WHERE Id=@Id",out i,
                dal.CreateParameter("@Id",id));
            dal.CommitTran();
            return true;
        }
    }
}