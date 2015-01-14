namespace CRM.Models
{
    public class CDept
    {
        public int? Id { get; set; }

        public string DeptCode { get; set; }

        public string DeptName { get; set; }

        public string ParentCode { get; set; }

        public CDept[] Childs { get; set; }

        public int People { get; set; }
    }
}