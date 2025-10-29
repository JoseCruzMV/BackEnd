using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Application.Behaviors;

public class UnhandledExceptionBehavior<TRequest, TRespose> : IPipelineBehavior<TRequest, TRespose>
    where TRequest : IRequest<TRespose>
{
    private readonly ILogger<IRequest> _logger;

    public UnhandledExceptionBehavior(ILogger<IRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TRespose> Handle(
        TRequest request,
        RequestHandlerDelegate<TRespose> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex,
                "Sucedio una excepcion para el request {name} {@request}", requestName, request);
            throw new Exception("Application Request con errores");
        }
    }
}