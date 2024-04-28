using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Text;
using System.Text.Encodings.Web;

namespace DataSanitization.middleware
{
    internal sealed class EncodeDataMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly FunctionExecutionDelegate _next;
        private readonly HtmlEncoder _htmlEncoder;

        public EncodeDataMiddleware(FunctionExecutionDelegate next, HtmlEncoder htmlEncoder)
        {
            _next = next;
            _htmlEncoder = htmlEncoder;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            var httpRequestData = await context.GetHttpRequestDataAsync();

            if (httpRequestData != null)
            {
                using var inputReader = new StreamReader(httpRequestData.Body, Encoding.UTF8, false, 1024, leaveOpen: true);
                var inputBody = await inputReader.ReadToEndAsync();

                //logics to validate the input body here

               // this is required, otherwise model binding will return null
                httpRequestData.Body.Seek(0L, SeekOrigin.Begin);
            }

            //Call the next middleware or function in the pipeline
            await next(context);

            // This happens after function execution. We can inspect the context after the function
            // was invoked
            //if (context.Items.TryGetValue("functionitem", out object value) && value is string message)
            //{

            //    var sadfasf = message;
            //}

            var httpResponseData = context.GetHttpResponseData();

            // Stream pointer was ending up at the end of stream. need to reset the pointer before reading the content of stream.
            // Otherwise the stream will always results in empty string when reading it.
            httpResponseData!.Body.Seek(0, SeekOrigin.Begin); 

            using var outputReader = new StreamReader(httpResponseData.Body, null, false, 1024, leaveOpen: true);
            var outputBody = await outputReader.ReadToEndAsync();
            var newResponse = Encoding.UTF8.GetBytes(outputBody);

            var stream = new MemoryStream(newResponse);

            if (httpResponseData != null)
            {
                context.GetHttpResponseData()!.Body = stream;
                //await context.GetHttpResponseData()!.Body.WriteAsync(Encoding.UTF8.GetBytes(outputBody),0, Encoding.UTF8.GetBytes(outputBody).Length);
            }

        }
    }

}
