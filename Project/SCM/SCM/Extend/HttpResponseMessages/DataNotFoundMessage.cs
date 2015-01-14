using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    public class DataNotFoundMessage:HttpResponseMessage
    {
        public DataNotFoundMessage()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "Data Not Found";
        }
    }
}