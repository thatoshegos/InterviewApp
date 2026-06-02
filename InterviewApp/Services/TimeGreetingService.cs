using System;

namespace InterviewApp.Services;

public class TimeGreetingService : ITimeGreetingService
{
    public string GetTimeBasedGreeting()
    {
        var hour = DateTime.Now.Hour;

        return hour switch
        {
            >= 5 and < 12 => "Good morning",
            >= 12 and < 17 => "Good afternoon",
            >= 17 and < 21 => "Good evening",
            _ => "Good night"
        };
    }
}