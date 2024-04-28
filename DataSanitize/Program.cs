using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DataSanitize.services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        //Add HtmlEncoder, JavaScriptEncoder and UrlEncoder to the service container
        services.AddWebEncoders();

        //Register our custom service
        services.AddTransient<IDataService, DataService>();
    })
    .Build();
