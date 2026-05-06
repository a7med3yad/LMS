<<<<<<< HEAD
using LMS.Application.Common.Exceptions;
=======
﻿using LMS.Application.Common.Exceptions;
>>>>>>> 4450aad95aa0059499e5c99c961c831b227af253
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
<<<<<<< HEAD
        catch (PaymentException ex)
        {
            await Write(context, HttpStatusCode.PaymentRequired, ex.Message);
        }
=======
>>>>>>> 4450aad95aa0059499e5c99c961c831b227af253
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await Write(context, HttpStatusCode.InternalServerError,
                "An unexpected error occurred.");
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