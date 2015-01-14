namespace CRM.Models
{
    public class CAuthorityModel
    {
        public int? Id { get; set; }

        public string UserCode { get; set; }

        public string UserName { get; set; }

        public string UPwd { get; set; }

        public string DeptCode { get; set; }

        public string GroupCode { get; set; }

        public bool Remain { get; set; }
    }
}