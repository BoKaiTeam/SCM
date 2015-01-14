namespace CRM.Models
{
    public class CUserGroupFun
    {
        public int Id { get; set; }

        public string GroupCode { get; set; }

        public string FunCode { get; set; }

        public string FunName { get; set; }

        public bool Queriable { get; set; }

        public bool Creatable { get; set; }

        public bool Changable { get; set; }

        public bool Deletable { get; set; }

        public bool Checkable { get; set; }

        public GroupType GroupType { get; set; }

    }
}