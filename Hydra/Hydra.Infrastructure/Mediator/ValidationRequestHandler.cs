namespace Hydra.Infrastructure.Mediator
{
    using System.Linq;
    using System.Threading.Tasks;
    using FluentValidation;

    public class ValidationRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;
        private readonly IValidator<TRequest>[] validators;

        public ValidationRequestHandler(IRequestHandler<TRequest, TResponse> inner, IValidator<TRequest>[] validators)
        {
            this.inner = inner;
            this.validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request)
        {
            ValidationContext context = new ValidationContext(request);

            var failures = this.validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return await this.inner.Handle(request);
        }
    }
}