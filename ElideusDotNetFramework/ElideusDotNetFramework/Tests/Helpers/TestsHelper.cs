using Azure;
using ElideusDotNetFramework.Operations;
using ElideusDotNetFramework.Operations.Contracts;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElideusDotNetFramework.Tests.Helpers
{
    public static class TestsHelper
    {
        public static async Task<TOut> SimulateCall<TOperation, TIn, TOut>(TOperation operation, TIn input) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            var callResult = await operation!.Call(input).ConfigureAwait(false);

            var result = (OperationHttpResult)callResult;

            return (TOut)result.Output!;
        }
    }
}
