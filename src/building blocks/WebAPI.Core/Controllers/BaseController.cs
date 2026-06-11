using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Core.Controllers
{
    [ApiController]
    public class BaseController : Controller
    {
        protected ICollection<string> Errors = new List<string>();

        protected ActionResult CustomResponse(object? result = null)
        {
            if (ValidOperation()) { return Ok(result); }

            return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
            {
                { "Messages", Errors.ToArray() }
            }));
        }

        protected ActionResult CustomResponse(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddError(error.ErrorMessage);
            }
            return CustomResponse();
        }

        protected bool ValidOperation()
        {
            return !Errors.Any();
        }

        protected void AddError(string erro)
        {
            Errors.Add(erro);
        }

        protected void ClearErrors()
        {
            Errors.Clear();
        }
    }
}
