
using Microsoft.Azure.Functions.Worker;
using Azure.Messaging;

namespace DataSanitization.functions
{
    public class EventGridTriggerFunction
    {
        [Function(nameof(EventGridTriggerFunction))]
        public async Task Run([EventGridTrigger] CloudEvent eventGridEvent, FunctionContext context)
        {
            var logger = context.GetLogger("EventGridTriggerFunctions");
        }
    }
}
