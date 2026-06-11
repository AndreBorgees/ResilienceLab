using External.Pagamento.MockApi.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace External.Pagamento.MockApi.Controllers
{
    [ApiController]
    [Route("oauth")]
    public sealed class AuthController : ControllerBase
    {
        [HttpPost("token")]
        public IActionResult Token()
        {
            var response = new TokenResponse
            {
                AccessToken = Guid.NewGuid().ToString("N"),
                TokenType = "Bearer",
                ExpiresIn = 3600
            };

            return Ok(response);
        }
    }
}
