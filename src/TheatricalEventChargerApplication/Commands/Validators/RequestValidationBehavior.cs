using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TheatricalEventChargerApplication.Commands.Validators
{

    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse> 
            where TResponse : class
    {
        private readonly IEnumerable<IValidator> _validators;

        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var failures = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            return failures.Any()
                ? Errors(failures)
                : next();
        }

        private static Task<TResponse> Errors(ICollection<ValidationFailure> failures)
        {
            Type responseStaticType = typeof(TResponse);
            var response = responseStaticType.GetMethod("Fail", new Type[] { typeof(ICollection<ValidationFailure>) })
                .Invoke(null, new ICollection<ValidationFailure>[] { failures });

            return Task.FromResult(response as TResponse);
        }
    }
}
