using System.Net;
using System.Net.Http;

namespace CRM.Extend.HttpResponseMessages
{
    public class SiginFailureMessage:HttpResponseMessage
    {
        public SiginFailureMessage()
        {
            StatusCode = HttpStatusCode.NotImplemented;
            ReasonPhrase = "Sigin Failure";
        }
    }
}