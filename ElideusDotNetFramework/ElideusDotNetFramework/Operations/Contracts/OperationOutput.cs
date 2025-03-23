using ElideusDotNetFramework.Errors.Contracts;
using System.Net;

namespace ElideusDotNetFramework.Operations.Contracts
{
    public class OperationOutput
    {
        public HttpStatusCode? StatusCode { get; set; }

        public Error? Error { get; set; }
    }
}