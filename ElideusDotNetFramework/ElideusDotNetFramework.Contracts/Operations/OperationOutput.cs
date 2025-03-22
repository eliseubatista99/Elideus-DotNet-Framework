using ElideusDotNetFramework.Contracts.Errors;
using System.Net;

namespace ElideusDotNetFramework.Contracts.Operations
{
    public class OperationOutput
    {
        public HttpStatusCode? StatusCode { get; set; }

        public Error? Error { get; set; }
    }
}