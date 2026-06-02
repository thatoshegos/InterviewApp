using InterviewApp.Queries;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InterviewApp.Handlers;

public class GetTimeGreetingHandler : IRequestHandler<GetTimeGreetingQuery, string>
{
    private readonly ITimeGreetingService _timeGreetingService;
    private readonly ILogger<GetTimeGreetingHandler> _logger;

    public GetTimeGreetingHandler(ITimeGreetingService timeGreetingService, ILogger<GetTimeGreetingHandler> logger)
    {
        _timeGreetingService = timeGreetingService;
        _logger = logger;
    }

    public Task<string> Handle(GetTimeGreetingQuery request, CancellationToken cancellationToken)
    {
        var greeting = _timeGreetingService.GetTimeBasedGreeting();
        _logger.LogInformation("Time-based greeting generated: {Greeting}", greeting);
        return Task.FromResult(greeting);
    }
}