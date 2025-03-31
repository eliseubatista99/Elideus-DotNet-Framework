using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;

namespace ElideusDotNetFramework.Core.Operations
{
    [ExcludeFromCodeCoverage]
    public class OperationsBuilder
    {
        protected void MapGetOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapGet(operation.OperationEndpoint, (HttpRequest request, IApplicationContext context, TIn input) => {
                return operation.Call(request, input);
            }).Produces<TOut>();
        }

        protected void MapPostOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPost(operation.OperationEndpoint, (HttpRequest request, IApplicationContext context, TIn input) => {
                return operation.Call(request, input);
            }).Produces<TOut>();
        }

        protected void MapPatchOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPatch(operation.OperationEndpoint, (HttpRequest request, IApplicationContext context, TIn input) => {
                return operation.Call(request, input);
            }).Produces<TOut>();
        }

        protected void MapPutOperation<TOperation, TIn, TOut>(ref WebApplication app, IApplicationContext context, TOperation operation) where TOperation : BaseOperation<TIn, TOut> where TIn : OperationInput where TOut : OperationOutput
        {
            app.MapPut(operation.OperationEndpoint, (HttpRequest request, IApplicationContext context, TIn input) => {
                return operation.Call(request, input);
            }).Produces<TOut>();
        }

        public virtual void MapOperations(ref WebApplication app, IApplicationContext context)
        {
        }
    }
}
