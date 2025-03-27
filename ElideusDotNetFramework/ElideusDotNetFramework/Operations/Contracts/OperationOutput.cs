using ElideusDotNetFramework.Core.Errors;
using System.Net;

namespace ElideusDotNetFramework.Core.Operations
{
    public class OperationOutput
    {
        public HttpStatusCode? StatusCode { get; set; }

        public Error? Error { get; set; }
    }
}