using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    /// <summary>
    /// 无权限异常返回的信息
    /// </summary>
    public class NoAuthorityMessage:HttpResponseMessage
    {
        public NoAuthorityMessage()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "No Authority";
        }
    }
}