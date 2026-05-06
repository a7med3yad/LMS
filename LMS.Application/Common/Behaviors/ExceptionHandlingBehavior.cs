using LMS.Application.Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Common.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(
        ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, "[NOT FOUND] {Request}", typeof(TRequest).Name);
            throw;
        }
        catch (ForbiddenException ex)
        {
            _logger.LogWarning(ex, "[FORBIDDEN] {Request}", typeof(TRequest).Name);
            throw;
        }
        catch (ConflictException ex)
        {
            _logger.LogWarning(ex, "[CONFLICT] {Request}", typeof(TRequest).Name);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ERROR] {Request}", typeof(TRequest).Name);
            throw;
        }
    }
}