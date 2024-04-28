using System.Net;
using DataSanitize.services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using DataSanitization.model;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace DataSanitization.functions
{
    public class InputWithFluentValidation
    {
        private readonly ILogger _logger;
        private readonly IDataService _dataService;
        private readonly IValidator<TestFluent> _validator;

        public InputWithFluentValidation(ILoggerFactory loggerFactory, IDataService dataService, IValidator<TestFluent> validator)
        {
            _logger = loggerFactory.CreateLogger<InputWithFluentValidation>();
            _dataService = dataService;
            _validator = validator;
        }

        [Function(nameof(InputWithFluentValidation))]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");


            var response = req.CreateResponse(HttpStatusCode.OK);

            // Deserialize object
            var dataModel = await req.ReadFromJsonAsync<TestFluent>();


            //Validating
            var validationResult = await _validator.ValidateAsync(dataModel);

            if (!validationResult.IsValid)
            {
                
                await response.WriteAsJsonAsync(validationResult.Errors.ToDictionary(x=> x.PropertyName, x => x.ErrorMessage));
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                await response.WriteAsJsonAsync(dataModel);
            }

            return response;
        }
    }
}
