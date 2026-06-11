using FluentValidation.Results;
using MediatR;
using System.Text.Json.Serialization;

namespace Core.Messages
{
    public abstract class Command : Message, IRequest<ValidationResult>
    {
        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; } = new ValidationResult();

        public virtual bool IsValid()
        {
            return ValidationResult.IsValid;
        }   
    }
}
