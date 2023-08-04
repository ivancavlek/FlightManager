using Acme.FlightManager.WebApi.AspNet.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Acme.FlightManager.WebApi.AspNet.Middleware;

public sealed class ValidationMiddleware
{
    public readonly RequestDelegate _next;

    public ValidationMiddleware(RequestDelegate next) =>
        _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            await context.Response.WriteAsJsonAsync(exception.Errors.ToValidationProblem());
        }
    }
}