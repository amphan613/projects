using System.Net;
using DataSanitize.services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DataSanitize
{
    public class GetHTML
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;

        public GetHTML(ILoggerFactory loggerFactory, IDataService dataService)
        {
            _logger = loggerFactory.CreateLogger<GetHTML>();
            _dataService = dataService;
        }

        [Function(nameof(GetHTML))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string data = await  _dataService.GetHtmlData();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteStringAsync(data);

            return response;
        }
    }
}
