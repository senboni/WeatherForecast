using MediatR;
using Serilog;
using System.Threading;
using System.Threading.Tasks;

namespace WeatherForecast.Host.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;

        _logger.Information("Executing request. {RequestName} {@Request}", requestName, request);

        var response = await next();

        _logger.Information("Finished executing request. {ResponseName} {@Response}", responseName, response);

        return response;
    }
}
