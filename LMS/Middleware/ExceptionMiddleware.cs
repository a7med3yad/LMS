using LMS.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace LMS.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await Write(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await Write(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (ConflictException ex)
        {
            await Write(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (ValidationException ex)
        {
            await Write(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (PaymentException ex)
        {
            await Write(context, HttpStatusCode.PaymentRequired, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unhandled exception on {Method} {Path}. Message: {Message}",
                context.Request.Method,
                context.Request.Path,
                ex.Message);

            var isDev = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            await Write(context, HttpStatusCode.InternalServerError,
                isDev ? ex.ToString() : "An unexpected error occurred.");
        }
    }

    private static Task Write(HttpContext ctx, HttpStatusCode code, string message)
    {
        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)code;
        return ctx.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            StatusCode = (int)code,
            Error = message
        }));
    }
}