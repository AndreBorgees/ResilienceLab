using Core.Messages;
using FluentValidation.Results;

namespace Core.Mediator
{
    public interface IMediatorHandler
    {
        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}
