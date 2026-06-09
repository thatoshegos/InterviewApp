using InterviewApp.Commands;
using InterviewApp.Handlers;
using InterviewApp.Queries;
using InterviewApp.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Time.Testing;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

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
        // Arrange
        var fakeTimeProvider = new FakeTimeProvider();
        fakeTimeProvider.SetUtcNow(new DateTime(2026, 6, 9, 1, 0, 0));
        var service = new TimeGreetingService(fakeTimeProvider);

        // Act & Assert - This test's assertion depends on current time
        var result = service.GetTimeBasedGreeting();
        Assert.NotNull(result);
        Assert.Equal("Good night", result);
    }
}