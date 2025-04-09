using ElideusDotNetFramework.Core.Operations;

namespace ElideusDotNetFramework.ExternalServices
{
    public interface IExternalServiceProvider
    {
        public Task<TOut> CallExternalPostOperation<TIn, TOut>(string endpoint, TIn input) where TIn : OperationInput where TOut : OperationOutput;

    }
}
