using Core.Data;
using FluentValidation.Results;

namespace Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string errorMessage)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, errorMessage));
        }

        protected async Task<ValidationResult> PersistData(IUnitOfWork unitOfWork)
        {
            if (!await unitOfWork.Commit()) AddError("Erro ao persistir os dados.");

            return ValidationResult;
        }
    }
}
