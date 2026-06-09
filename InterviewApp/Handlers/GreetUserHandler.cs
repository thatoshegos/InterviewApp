using InterviewApp.Commands;
using InterviewApp.Queries;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Handlers;

public class GreetUserHandler : IRequestHandler<GreetUserCommand, string>
{
    private readonly ILogger<GreetUserHandler> _logger;
    private readonly IMediator _mediator;

    public GreetUserHandler(ILogger<GreetUserHandler> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<string> Handle(GreetUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate configuration
            if (string.IsNullOrWhiteSpace(request.ConfiguredMessage))
            {
                _logger.LogError("Configured message is null or empty");
                return "Error: No greeting message configured";
            }

            if (string.IsNullOrWhiteSpace(request.Language))
            {
                _logger.LogError("Configured language is null or empty");
                return "Error: No language configured";
            }

            // Get the greeting message based on language
            var greetingMessage = GetLocalizedMessage(request.Language, request.ConfiguredMessage);

            // Get time-based greeting
            var timeGreeting = await _mediator.Send(new GetTimeGreetingQuery(), cancellationToken);

            var finalMessage = $"{timeGreeting}! {greetingMessage}";

            _logger.LogInformation("Final greeting message: {Message}", finalMessage);
            return finalMessage;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while generating greeting");
            return "Error: Unable to generate greeting";
        }
    }

    private string GetLocalizedMessage(string language, string defaultMessage)
    {
        return language.ToLower() switch
        {
            "english" => defaultMessage,
            "afrikaans" => defaultMessage,
            "zulu" => defaultMessage,
            _ => HandleUnsupportedLanguage(language, defaultMessage)
        };
    }

    private string HandleUnsupportedLanguage(string language, string defaultMessage)
    {
        _logger.LogWarning("Unsupported language '{Language}' requested, falling back to English", language);
        return defaultMessage;
    }
}