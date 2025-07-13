using FakeAPI.Application.Common.Wrappers;
using FluentValidation;
using System.Net;
using System.Text.Json;

namespace FakeAPI.Api.Middleware;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ValidationExceptionMiddleware> _logger;

    public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {   
        _logger.LogInformation(">>> Masuk ke ValidationExceptionMiddleware");
        try
        {
            await _next(context); // lanjut ke pipeline berikutnya
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation failed: {Errors}", ex.Errors);

            var errors = ex.Errors.Select(e => e.ErrorMessage);
            var message = string.Join(", ", errors);

            var response = ApiResponse<string>.FailBadRequest(message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }
    }
}
