using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    public class SystemExceptionMessage:HttpResponseMessage
    {
        public SystemExceptionMessage()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "System Exception";
        }
    }
}