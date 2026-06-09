using System;

namespace InterviewApp.Services;

public class TimeGreetingService : ITimeGreetingService
{
    private readonly TimeProvider _timeProvider;

    public TimeGreetingService() : this(TimeProvider.System)
    {

    }
    public TimeGreetingService(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }
    public string GetTimeBasedGreeting()
    {
        var hour = _timeProvider.GetLocalNow().Hour;

        return hour switch
        {
            >= 5 and < 12 => "Good morning",
            >= 12 and < 17 => "Good afternoon",
            >= 17 and < 21 => "Good evening",
            _ => "Good night"
        };
    }
}