using FluentValidation;
using MediatR;

namespace Application.Behaviour
{
    public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                    throw new ValidationException(failures);

                //var context = new ValidationContext<TRequest>(request);

                //var validationFailures = await Task.WhenAll(
                //    _validators.Select(validator => validator.ValidateAsync(context)));

                //var errors = validationFailures
                //    .Where(validationResult => !validationResult.IsValid)
                //    .SelectMany(validationResult => validationResult.Errors)
                //    .Select(validationFailure => new ValidationError(
                //        validationFailure.PropertyName,
                //        validationFailure.ErrorMessage))
                //    .ToList();

                //if (errors.Any())
                //{
                //    throw new Exceptions.ValidationException(errors);
                //}

            }
            return await next();
        }
    }
}
