namespace CRM.Models
{
    /// <summary>
    /// 菜单分类
    /// </summary>
    public class CMenuCategory
    {
        public int Id { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public short SerialNo { get; set; }

        public CMenu[] Menus { get; set; }
    }
}