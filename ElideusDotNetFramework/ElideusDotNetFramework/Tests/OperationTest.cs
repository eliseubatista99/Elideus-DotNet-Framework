using ElideusDotNetFramework.Operations;
using ElideusDotNetFramework.Operations.Contracts;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Xunit;

namespace ElideusDotNetFramework.Tests;

public class OperationTest<TOperation, TIn, TOut> where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
{
    protected TOperation? operationToTest;

    protected virtual void Setup()
    {

    }

    protected virtual async Task<TOut> SimulateCall(TOperation operation, TIn input)
    {
        var callResult = await operation!.Call(input).ConfigureAwait(false);

        var result = (OperationHttpResult) callResult;

        return (TOut) result.Output!;
    }
}
