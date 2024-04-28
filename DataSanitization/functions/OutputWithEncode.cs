using System.Net;
using DataSanitize.services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace DataSanitization.functions
{
    public class OutputWithEncode
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;

        public OutputWithEncode(ILoggerFactory loggerFactory, IDataService dataService)
        {
            _logger = loggerFactory.CreateLogger<OutputWithEncode>();
            _dataService = dataService;
        }

        [Function(nameof(OutputWithEncode))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req, FunctionContext context)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string data = await _dataService.GetHtmlData();
            var response = req.CreateResponse(HttpStatusCode.OK);



            await response.WriteAsJsonAsync(data);

            return response;
        }
    }
}
