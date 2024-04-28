using DataSanitize.services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FluentValidation.AspNetCore;
using DataSanitization.model;
using FluentValidation;
using System.ComponentModel.DataAnnotations;
using DataSanitization.middleware;
using DataSanitization.functions;
using Microsoft.AspNetCore.Authentication;

var host = new HostBuilder()
    .ConfigureServices(services =>
    {
        //Add HtmlEncoder, JavaScriptEncoder and UrlEncoder to the service container
        services.AddWebEncoders();

        //Register our custom service
        services.AddTransient<IDataService, DataService>();
    })

    .ConfigureFunctionsWorkerDefaults(workerApplication =>
    {
        //workerApplication.UseMiddleware<EncodeDataMiddleware>();
        workerApplication.UseWhen<EncodeDataMiddleware>((context) =>
        {
            return 
            context.FunctionDefinition.InputBindings.Values
            .First(a => a.Direction == BindingDirection.In).Type == "httpTrigger" 
            && context.FunctionDefinition.Name == nameof(OutputWithEncode);
        });
        workerApplication.UseWhen<InputValidationMiddleware>((context) =>
        {
            // We want to use this middleware only for http trigger invocations.
            return context.FunctionDefinition.InputBindings.Values
                          .First(a => a.Type.EndsWith("Trigger")).Type == "eventGridTrigger";
        });
    })
    .Build();

host.Run();
