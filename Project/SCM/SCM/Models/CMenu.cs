namespace CRM.Models
{
    /// <summary>
    /// 菜单对象
    /// </summary>
    public class CMenu
    {
        public int Id { get; set; }

        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        public string MenuCmd { get; set; }

        public short SerialNo { get; set; }

        public string ParentCode { get; set; }
    }
}