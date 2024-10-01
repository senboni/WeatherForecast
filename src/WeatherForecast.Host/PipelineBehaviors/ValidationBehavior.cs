using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WeatherForecast.Host.Common;

namespace WeatherForecast.Host.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : ApiResponse<object>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationContext = new ValidationContext<TRequest>(request);

        var errors = _validators.Select(x => x.Validate(validationContext))
            .SelectMany(x => x.Errors)
            .Select(x => x.ErrorMessage)
            .Where(x => x is not null)
            .Distinct()
            .ToArray();

        if (errors.Length != 0)
        {
            var failure = ApiResponse<object>.Failure(errors, message: "Validation failed");
            return (TResponse)failure;
        }

        return await next();
    }
}
