namespace CRM.Models
{
    public class CLog
    {
        public int Id { get; set; }

        public string LogDate { get; set; }

        public string LogContent { get; set; }

        public LogType LogType { get; set; }

        public string LogUser { get; set; }
    }

    public enum LogType
    {
        系统异常=-1,
        操作失败=0,
        操作成功=1
    }
}