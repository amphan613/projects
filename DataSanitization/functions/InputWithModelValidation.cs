using System.Net;
using DataSanitize.services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using DataSanitization.model;
using System.ComponentModel.DataAnnotations;

namespace DataSanitization.functions
{
    public class InputWithDataAnnotation
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;

        public InputWithDataAnnotation(ILoggerFactory loggerFactory, IDataService dataService)
        {
            _logger = loggerFactory.CreateLogger<InputWithDataAnnotation>();
            _dataService = dataService;
        }

        [Function(nameof(InputWithDataAnnotation))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

        
            var response = req.CreateResponse(HttpStatusCode.OK);
            

            var postDTO = await req.ReadFromJsonAsync<TestDTO>();

            try
            {
                Validator.ValidateObject(postDTO!, new ValidationContext(postDTO!), validateAllProperties: true);
                await response.WriteAsJsonAsync(postDTO);
            }
            catch (ValidationException exception)
            {
                // Handle validation failures...
                ValidationResult result = exception.ValidationResult;
                await response.WriteAsJsonAsync(result);
            }

            return response;
        }
    }
}
