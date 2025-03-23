using ElideusDotNetFramework.Operations.Contracts;
using ElideusDotNetFramework.Providers.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ElideusDotNetFramework.Operations
{
    public class OperationsBuilder
    {
        protected virtual void MapPostOperation<TOperation, TIn, TOut>(ref WebApplication app, string endPoint, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPost(endPoint, (IApplicationContext context, TIn input) => operation.Call(input)).Produces<TOut>();
        }

        public virtual void MapOperations(ref WebApplication app)
        {
        }
    }
}
