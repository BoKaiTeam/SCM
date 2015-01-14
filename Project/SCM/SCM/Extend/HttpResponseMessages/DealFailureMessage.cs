using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    public class DealFailureMessage:HttpResponseMessage
    {
        public DealFailureMessage()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "Deal Failure";
        }
    }
}