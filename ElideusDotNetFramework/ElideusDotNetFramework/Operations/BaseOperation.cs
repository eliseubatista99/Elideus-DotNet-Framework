#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

using ElideusDotNetFramework.Operations.Contracts;
using ElideusDotNetFramework.Errors.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net;
using ElideusDotNetFramework.Providers.Contracts;

namespace ElideusDotNetFramework.Operations
{
    public class BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
    {
        public string OperationEndpoint { get; set; } = "/BaseOperation";

        protected IApplicationContext executionContext;

        public BaseOperation(IApplicationContext context, string endpoint)
        {
            OperationEndpoint = endpoint;
            executionContext = context;
        }

        /// <summary>
        /// This is the second method to be executed.
        /// Validates the operation input
        /// </summary>
        /// <param name="input">The operation input</param>
        /// <returns>A possible error and status code</returns>
        protected virtual async Task<(HttpStatusCode? StatusCode, Error? Error)> ValidateInput(OperationInput input)
        {
            return (HttpStatusCode.OK, null);
        }

        /// <summary>
        /// This is the first method to be executed.
        /// Initializes the operation.
        /// </summary>
        protected virtual async Task InitAsync()
        {
        }

        /// <summary>
        /// This is the third method to be executed.
        /// Executes the operation logic.
        /// </summary>
        /// <param name="input">The operation input</param>
        /// <returns>The operation output</returns>
        protected virtual async Task<TOut?> ExecuteAsync(TIn input)
        {
            return default;
        }

        /// <summary>
        /// Executes when the operation is called
        /// </summary>
        /// <param name="input">The operation input</param>
        /// <returns>An operation result with a status code the operation output</returns>
        public virtual async Task<IResult> Call(TIn input)
        {
            await InitAsync();

            var validateInputResult = await ValidateInput(input);

            // If the input is invalid, return the error and status code
            if (validateInputResult.Error != null || validateInputResult.StatusCode != HttpStatusCode.OK)
            {
                // If no code was specified, only the error, assume the bad request code
                var code = validateInputResult.StatusCode ?? HttpStatusCode.BadRequest;

                return new OperationHttpResult(new OperationOutput
                {
                    StatusCode = code,
                    Error = validateInputResult.Error
                });
            }

            var executionResponse = await ExecuteAsync(input).ConfigureAwait(false);

            if (executionResponse == null)
            {
                return new OperationHttpResult(new OperationOutput
                {
                    StatusCode = HttpStatusCode.NoContent
                });
            }

            if (executionResponse.StatusCode == null)
            {
                executionResponse.StatusCode = HttpStatusCode.OK;
            }

            return new OperationHttpResult(executionResponse);
        }
    }
}
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
