using ElideusDotNetFramework.Operations;
using ElideusDotNetFramework.Operations.Contracts;

namespace ElideusDotNetFramework.Tests;

public class OperationTest<TOperation, TIn, TOut> where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
{
    protected TOperation? OperationToTest;


    protected ElideusDotNetFrameworkTestsBuilder TestsBuilder;

    public OperationTest(ElideusDotNetFrameworkTestsBuilder _testBuilder)
    {
        this.TestsBuilder = _testBuilder;
    }
}
