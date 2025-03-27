using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace ElideusDotNetFramework.Core.Operations
{
    public class OperationsBuilder
    {
        protected void MapGetOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapGet(operation.OperationEndpoint, (IApplicationContext context, TIn input) => operation.Call(input)).Produces<TOut>();
        }

        protected void MapPostOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPost(operation.OperationEndpoint, (IApplicationContext context, TIn input) => operation.Call(input)).Produces<TOut>();
        }

        protected void MapPatchOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPatch(operation.OperationEndpoint, (IApplicationContext context, TIn input) => operation.Call(input)).Produces<TOut>();
        }

        protected void MapPutOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPut(operation.OperationEndpoint, (IApplicationContext context, TIn input) => operation.Call(input)).Produces<TOut>();
        }

        public virtual void MapOperations(ref WebApplication app, IApplicationContext context)
        {
        }
    }
}
