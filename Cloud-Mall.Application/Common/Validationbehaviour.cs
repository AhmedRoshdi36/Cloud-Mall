using FluentValidation;
using MediatR;

namespace Cloud_Mall.Application.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

                // Dynamically create a failure response using reflection
                var type = typeof(TResponse);
                var method = type.GetMethod("Failure", new[] { typeof(List<string>) });

                if (method is not null)
                {
                    var response = method.Invoke(null, new object[] { errorMessages });
                    return (TResponse)response!;
                }

                throw new ValidationException(failures); // fallback
            }

            return await next();
        }
    }
}
