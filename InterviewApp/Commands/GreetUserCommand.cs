using MediatR;

namespace InterviewApp.Commands;

public class GreetUserCommand : IRequest<string>
{
    public string ConfiguredMessage { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}