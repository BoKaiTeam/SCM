namespace CRM.Models
{
    public class CUserGroup
    {
        public int? Id { get; set; }

        public string GroupCode { get; set; }

        public string GroupName { get; set; }

        public int People { get; set; }

        public CUserGroupFun[] GroupFun { get; set; }

        public int Fun { get; set; }

        public GroupType GroupType { get; set; }
    }

    public enum GroupType
    {
        维护者=0,
        使用者=1
    }
}