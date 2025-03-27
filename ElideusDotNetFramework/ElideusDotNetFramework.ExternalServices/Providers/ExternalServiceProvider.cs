using ElideusDotNetFramework.Core;
using ElideusDotNetFramework.Core.Operations;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ElideusDotNetFramework.ExternalServices
{
    public class ExternalServiceProvider : IExternalServiceProvider
    {
        protected IApplicationContext applicationContext { get; set; }

        public ExternalServiceProvider(IApplicationContext _applicationContext)
        {
            this.applicationContext = _applicationContext;
        }

        protected virtual string GetServiceUrl()
        {
            return string.Empty;
        }

        protected virtual async Task<TOut> CallExternalPostOperation<TIn, TOut>(string endpoint, TIn input) where TIn : OperationInput where TOut : OperationOutput
        {
            var httpClient = new HttpClient();
            var serviceUrl = GetServiceUrl();
            var requestUrl = $"{serviceUrl}{endpoint}";

            using StringContent jsonInput = new(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await httpClient.PostAsync(requestUrl, jsonInput);

            //response.EnsureSuccessStatusCode();

            var operationHttpResult = await response.Content.ReadFromJsonAsync<OperationHttpResult>();
            var operationResult = (TOut)operationHttpResult!.Output!;

            return operationResult;
        }


    }
}
