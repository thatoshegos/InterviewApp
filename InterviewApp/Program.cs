using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using InterviewApp.Services;
using InterviewApp.Models;
using System.Reflection;
using System;

class Program
{
    static async Task Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                // Register configuration
                services.Configure<GreetingOptions>(context.Configuration.GetSection("Greeting"));

                // Register services
                services.AddTransient<IGreetingService, GreetingService>();
                services.AddTransient<ITimeGreetingService, TimeGreetingService>();

                // Add MediatR
                services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

                // Add logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                });

                // Validate configuration on startup
                services.AddOptions<GreetingOptions>()
                    .Validate(options =>
                    {
                        if (string.IsNullOrWhiteSpace(options.Message))
                        {
                            throw new ArgumentException("Greeting:Message cannot be null or empty");
                        }
                        if (string.IsNullOrWhiteSpace(options.Language))
                        {
                            throw new ArgumentException("Greeting:Language cannot be null or empty");
                        }
                        return true;
                    }, "Invalid greeting configuration");
            })
            .Build();

        var greetingService = host.Services.GetRequiredService<IGreetingService>();
        await greetingService.RunAsync();

        await host.RunAsync();
    }
}