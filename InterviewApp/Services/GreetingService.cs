using System;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using InterviewApp.Models;
using MediatR;
using InterviewApp.Commands;
using System.Threading.Tasks;

namespace InterviewApp.Services
{
    public class GreetingService : IGreetingService
    {
        private readonly GreetingOptions _options;
        private readonly ILogger<GreetingService> _logger;
        private readonly IMediator _mediator;

        public GreetingService(IOptions<GreetingOptions> options, ILogger<GreetingService> logger, IMediator mediator)
        {
            _options = options.Value;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Greeting Service started");

            try
            {
                var command = new GreetUserCommand
                {
                    ConfiguredMessage = _options.Message,
                    Language = _options.Language
                };

                var result = await _mediator.Send(command);
                Console.WriteLine(result);
                _logger.LogInformation("Greeting displayed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to execute greeting command");
                Console.WriteLine("An error occurred while displaying the greeting.");
            }
        }
    }
}