using Azure.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Encodings.Web;
using DataSanitization.extension;

namespace DataSanitization.middleware
{
    public class InputValidationMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly FunctionExecutionDelegate _next;
        private readonly HtmlEncoder _htmlEncoder;

        public InputValidationMiddleware(FunctionExecutionDelegate next, HtmlEncoder htmlEncoder)
        {
            _next = next;
            _htmlEncoder = htmlEncoder;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var requestData = context.GetCloudEventData();

            context.BindingContext.BindingData.TryGetValue("eventGridTrigger", out var triggerData);
            //if (context.BindingContext.BindingData.TryGetValue("ServiceBusTrigger", out var triggerData)
            //    && triggerData is ServiceBusReceivedMessage receivedMessage)
            //{
            //    if (receivedMessage.ApplicationProperties.ContainsKey("CorrelationId"))
            //    {
            //        var correlationId = (string)receivedMessage.ApplicationProperties["CorrelationId"];

            //        // Log the correlation ID without modifying the message
            //        _logger.LogInformation("Correlation ID: {correlationId}", correlationId);
            //    }
            //}

            await next(context);
        }
    }

}
