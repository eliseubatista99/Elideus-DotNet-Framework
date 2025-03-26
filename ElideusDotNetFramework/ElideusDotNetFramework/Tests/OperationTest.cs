using ElideusDotNetFramework.Operations;

namespace ElideusDotNetFramework.Tests;

public class OperationTest<TOperation, TIn, TOut> where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
{
    protected TOperation? OperationToTest;


    protected ElideusDotNetFrameworkTestsBuilder TestsBuilder;

    protected virtual async Task<TOut> SimulateOperationToTestCall(TIn input)
    {
        return await TestsHelper.SimulateCall<TOperation, TIn, TOut>(OperationToTest!, input);
    }

    public OperationTest(ElideusDotNetFrameworkTestsBuilder _testBuilder)
    {
        this.TestsBuilder = _testBuilder;
    }
}
