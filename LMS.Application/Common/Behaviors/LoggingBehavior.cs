using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace LMS.Application.Common.Behaviors;

public class LoggingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation("[START] {Request}", name);
        var sw = Stopwatch.StartNew();
        var response = await next();
        _logger.LogInformation("[END] {Request} — {Ms}ms", name, sw.ElapsedMilliseconds);
        return response;
    }
}