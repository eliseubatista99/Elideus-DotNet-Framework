using ElideusDotNetFramework.Core;
using ElideusDotNetFramework.Core.Operations;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ElideusDotNetFramework.ExternalServices
{
    public class ExternalServiceProvider : IExternalServiceProvider
    {
        protected IApplicationContext applicationContext { get; set; }
        protected static HttpClient httpClient { get; set; } = new HttpClient();

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
            httpClient.BaseAddress = new Uri(GetServiceUrl());
            httpClient.DefaultRequestHeaders
                  .Accept
                  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, endpoint);

            request.Content = new StringContent(JsonSerializer.Serialize(input),
                                                Encoding.UTF8,
                                                "application/json");//CONTENT-TYPE header



            var response = await httpClient.SendAsync(request);
            var operationResult = await response.Content.ReadFromJsonAsync<TOut>();

            return operationResult!;
        }
    }
}
