using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.ExceptionHandling;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private const string ApplicationJsonType = "application/json";

    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (JsonException ex)
        {
            logger.LogWarning("JSON Exception: {Exception}", ex);

            await HandleExceptionAsync(
                context,
                HttpStatusCode.BadRequest,
                "Invalid JSON format.",
                ex.Message
            );
        }
        catch (Exception ex)
        {
            logger.LogError("Unhandled Exception: {Exception}", ex);

            await HandleExceptionAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Internal Server Error. Please try again later.",
                ex.Message
            );
        }
    }

    private static Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        string details)
    {
        var responseContent = JsonSerializer.Serialize(new HandleExceptionModel
        {
            Code = statusCode,
            Message = message,
            Details = details
        });

        context.Response.ContentType = ApplicationJsonType;
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(responseContent);
    }
}