using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Pedido.API.Idempotency
{
    public class IdempotencyFilter : IAsyncActionFilter
    {
        private readonly IIdempotencyService _idempotencyService;
        private readonly IdempotencyOptions _options;

        public IdempotencyFilter(IIdempotencyService idempotencyService, IOptions<IdempotencyOptions> options)
        {
            _idempotencyService = idempotencyService;
            _options = options.Value;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cancellationToken = context.HttpContext.RequestAborted;

            var idempotencyKey = GetIdepotencyKey(context);

            if (string.IsNullOrWhiteSpace(idempotencyKey))
            {
                context.Result = new BadRequestObjectResult(new
                {
                    message = $"Header '{_options.HeaderName}' é obrigatório para essa operação."
                });

                return;
            }

            var requestHash = await GenerateRequestHashAsync(context.HttpContext.Request, cancellationToken);

            var existingRecord = await _idempotencyService.GetAsync(idempotencyKey, cancellationToken);

            if (existingRecord is not null)
            {
                if (existingRecord.RequestHash != requestHash)
                {
                    context.Result = new ConflictObjectResult(new
                    {
                        message = "A mesma Idempotency-Key foi usada com um corpo de requisição diferente."
                    });

                    return;
                }

                if (existingRecord.Status == IdempotencyStatus.Completed)
                {
                    context.Result = new ContentResult
                    {
                        StatusCode = existingRecord.StatusCode ?? StatusCodes.Status200OK,
                        Content = existingRecord.ResponsBody,
                        ContentType = "application/json"
                    };

                    return;
                }

                if (existingRecord.Status == IdempotencyStatus.Processing)
                {
                    context.Result = new ConflictObjectResult(new
                    {
                        message = "Essa operação já está em processamento."
                    });

                    return;
                }
            }

            var started = await _idempotencyService.TryStartProcessingAsyn(
                idempotencyKey,
                requestHash,
                _options.Expiration,
                cancellationToken);

            if (!started)
            {
                context.Result = new ConflictObjectResult(new
                {
                    message = "Essa operação já foi iniciada por outra requisição."
                });

                return;
            }

            var executedContext = await next();

            if (executedContext.Exception is not null)
                return;

            var responseData = ExtractResponseData(executedContext.Result);

            if (ShouldSaveResponse(responseData.StatusCode))
            {
                await _idempotencyService.CompleteAsync(
                    idempotencyKey,
                    requestHash,
                    responseData.StatusCode,
                    responseData.Body,
                    _options.Expiration,
                    cancellationToken);
            }
            else
            {
                await _idempotencyService.FailAsync(
                    idempotencyKey,
                    requestHash,
                    responseData.StatusCode, 
                    responseData.Body, 
                    _options.Expiration,
                    cancellationToken);
            }
        }

        private string? GetIdepotencyKey(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_options.HeaderName, out var headerValues))
                return null;

            return headerValues.FirstOrDefault();
        }

        private static async Task<string> GenerateRequestHashAsync(
            HttpRequest request,
            CancellationToken cancellationToken)
        {
            request.EnableBuffering();

            request.Body.Position = 0;

            using var reader = new StreamReader(
                request.Body,
                Encoding.UTF8,
                detectEncodingFromByteOrderMarks: false,
                leaveOpen: true);

            var body = await reader.ReadToEndAsync(cancellationToken);

            request.Body.Position = 0;

            var raw = $"{request.Method}:{request.Path}:{body}";

            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            return Convert.ToHexString(bytes);
        }

        private static IdempotencyResponseData ExtractResponseData(IActionResult? result)
        {
            if (result is ObjectResult objectResult)
            {
                var statusCode = objectResult.StatusCode ?? StatusCodes.Status200OK;

                var body = JsonSerializer.Serialize(
                    objectResult.Value,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                return new IdempotencyResponseData(statusCode, body);
            }

            if (result is JsonResult jsonResult)
            {
                var statusCode = jsonResult.StatusCode ?? StatusCodes.Status200OK;

                var body = JsonSerializer.Serialize(
                    jsonResult.Value,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                return new IdempotencyResponseData(statusCode, body);
            }

            if (result is ContentResult contentResult)
            {
                var statusCode = contentResult.StatusCode ?? StatusCodes.Status200OK;

                var body = JsonSerializer.Serialize(
                    contentResult.Content,
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                return new IdempotencyResponseData(statusCode, body);
            }

            if (result is StatusCodeResult statusCodeResult)
            {
                return new IdempotencyResponseData(
                    statusCodeResult.StatusCode,
                    string.Empty);
            }

            if (result is EmptyResult)
            {
                return new IdempotencyResponseData(
                    StatusCodes.Status204NoContent,
                    string.Empty);
            }

            return new IdempotencyResponseData(
                StatusCodes.Status200OK,
                string.Empty);
        }

        private static bool ShouldSaveResponse(int statusCode)
        {
            return statusCode is >= 200 and < 300;
        }

        private sealed record IdempotencyResponseData(
            int StatusCode,
            string Body);
    }
}
