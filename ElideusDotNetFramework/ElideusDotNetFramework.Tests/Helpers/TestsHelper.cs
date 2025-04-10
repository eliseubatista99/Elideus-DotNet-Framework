using ElideusDotNetFramework.Core.Operations;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestsHelper
    {
        public static async Task<TOut> SimulateCall<TOperation, TIn, TOut>(TOperation operation, TIn input) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            var callResult = await operation!.Call(new DefaultHttpContext().Request, input).ConfigureAwait(false);

            var result = (OperationHttpResult)callResult;

            return (TOut)result.Output!;
        }
    }
}
