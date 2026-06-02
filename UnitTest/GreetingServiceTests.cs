using System;
using System.Threading;
using System.Threading.Tasks;
using InterviewApp.Commands;
using InterviewApp.Handlers;
using InterviewApp.Services;
using Microsoft.Extensions.Logging;
using MediatR;
using Moq;
using Xunit;
using InterviewApp.Queries;

namespace InterviewApp.Tests;

public class GreetingServiceTests
{
    [Fact]
    public async Task GreetUserHandler_WithValidEnglishConfig_ReturnsCorrectGreeting()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<GreetUserHandler>>();
        var mockMediator = new Mock<IMediator>();
        var command = new GreetUserCommand
        {
            ConfiguredMessage = "Welcome to the interview app!",
            Language = "English"
        };

        mockMediator.Setup(m => m.Send(It.IsAny<GetTimeGreetingQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Good morning");

        var handler = new GreetUserHandler(mockLogger.Object, mockMediator.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains("Good morning!", result);
        Assert.Contains("Welcome to the interview app!", result);
    }

    [Fact]
    public async Task GreetUserHandler_WithUnsupportedLanguage_FallsBackToEnglish()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<GreetUserHandler>>();
        var mockMediator = new Mock<IMediator>();
        var command = new GreetUserCommand
        {
            ConfiguredMessage = "Welcome to the interview app!",
            Language = "Spanish"
        };

        mockMediator.Setup(m => m.Send(It.IsAny<GetTimeGreetingQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("Good morning");

        var handler = new GreetUserHandler(mockLogger.Object, mockMediator.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Contains("Welcome to the interview app!", result);
    }

    [Fact]
    public async Task GreetUserHandler_WithEmptyMessage_ReturnsError()
    {
        // Arrange
        var mockLogger = new Mock<ILogger<GreetUserHandler>>();
        var mockMediator = new Mock<IMediator>();
        var command = new GreetUserCommand
        {
            ConfiguredMessage = "",
            Language = "English"
        };

        var handler = new GreetUserHandler(mockLogger.Object, mockMediator.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Error: No greeting message configured", result);
    }

    [Fact]
    public void TimeGreetingService_ReturnsCorrectGreetingForTimeOfDay()
    {
        // Arrange
        var service = new TimeGreetingService();

        // Act & Assert - This test's assertion depends on current time
        var result = service.GetTimeBasedGreeting();
        Assert.NotNull(result);
        Assert.Contains(result, new[] { "Good morning", "Good afternoon", "Good evening", "Good night" });
    }
}