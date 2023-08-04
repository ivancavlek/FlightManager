using Acme.FlightManager.WebApi.AspNet.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Filters;

public class ValidatorFilter<T> : IEndpointFilter where T : class
{
    private readonly IValidator<T> _validator;

    public ValidatorFilter(IValidator<T> validator) =>
        _validator = validator;

    async ValueTask<object> IEndpointFilter.InvokeAsync(
        EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatable = context.Arguments.SingleOrDefault(dto => dto is T) as T;
        var validationResult = await _validator.ValidateAsync(validatable);

        return validationResult.IsValid ? await next(context) : validationResult.Errors.ToValidationProblem();
    }
}