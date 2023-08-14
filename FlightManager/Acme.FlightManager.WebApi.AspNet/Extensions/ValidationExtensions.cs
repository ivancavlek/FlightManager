using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Collections.Generic;
using System.Linq;

namespace Acme.FlightManager.WebApi.AspNet.Extensions;

internal static class ValidationExtensions
{
    internal static ValidationProblem ToValidationProblem(this IEnumerable<ValidationFailure> validationFailures) =>
        TypedResults.ValidationProblem(
            validationFailures
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    gpg => gpg.Key,
                    gpg => gpg.Select(x => x.ErrorMessage).ToArray()));
}