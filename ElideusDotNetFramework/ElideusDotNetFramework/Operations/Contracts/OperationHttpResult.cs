using Microsoft.AspNetCore.Http;
using System.Net;

namespace ElideusDotNetFramework.Operations.Contracts
{
    public class OperationHttpResult : IResult
    {
        public HttpStatusCode? StatusCode { get; private set; }

        public OperationOutput? Output { get; private set; }

        public OperationHttpResult(HttpStatusCode code)
        {
            this.StatusCode = code;
        }

        public OperationHttpResult(OperationOutput? output)
        {
            this.StatusCode = output?.StatusCode;
            this.Output = output;
        }

        public Task ExecuteAsync(HttpContext httpContext)
        {
            if (StatusCode == null)
            {
                StatusCode = HttpStatusCode.OK;
            }

            return StatusCode switch
            {
                HttpStatusCode.NotFound => Results.NotFound(Output).ExecuteAsync(httpContext),
                HttpStatusCode.BadRequest => Results.BadRequest(Output).ExecuteAsync(httpContext),
                HttpStatusCode.NoContent => Results.NoContent().ExecuteAsync(httpContext),
                HttpStatusCode.Unauthorized => Results.Unauthorized().ExecuteAsync(httpContext),
                _ => Results.Ok(Output).ExecuteAsync(httpContext),
            };
        }
    }

}