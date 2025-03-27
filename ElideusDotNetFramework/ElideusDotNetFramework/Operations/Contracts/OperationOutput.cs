using ElideusDotNetFramework.Core.Errors;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace ElideusDotNetFramework.Core.Operations
{
    [ExcludeFromCodeCoverage]
    public class OperationOutput
    {
        public HttpStatusCode? StatusCode { get; set; }

        public Error? Error { get; set; }
    }
}