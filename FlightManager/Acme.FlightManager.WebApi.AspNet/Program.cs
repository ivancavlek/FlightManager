using Acme.FlightManager.WebApi.AspNet.Apis;
using Acme.FlightManager.WebApi.AspNet.Middleware;
using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
var app = SetWebApplicationBuilder(builder).Build();
SetWebApplication(app).Run();

static WebApplicationBuilder SetWebApplicationBuilder(WebApplicationBuilder builder)
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(SwaggerConfiguration);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    builder.Services.AddAuthorization(AuthorizationConfiguration);

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddApiVersioning(VersioningConfiguration);

    return builder;
}

static WebApplication SetWebApplication(WebApplication app)
{
    var versionSet = GetApiVersionSet(app);

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseMiddleware<ValidationMiddleware>();
    app.UseExceptionHandler(ExceptionHandlerConfiguration);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGroup("flight")
        .MapFlightApis()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(1)
        .WithOpenApi()
        .AllowAnonymous();

    return app;
}

static void SwaggerConfiguration(SwaggerGenOptions sgo)
{
    sgo.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
    sgo.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
}

static void AuthorizationConfiguration(AuthorizationOptions aon) =>
    aon.FallbackPolicy = new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();

static ApiVersionSet GetApiVersionSet(WebApplication webApplication) =>
    webApplication
        .NewApiVersionSet()
        .HasApiVersion(new(1.0))
        .ReportApiVersions()
        .Build();

static void VersioningConfiguration(ApiVersioningOptions avo)
{
    avo.DefaultApiVersion = new(1.0);
    avo.AssumeDefaultVersionWhenUnspecified = true;
}

static void ExceptionHandlerConfiguration(IApplicationBuilder applicationBuilder) =>
    applicationBuilder.Run(async hct => await TypedResults.Problem().ExecuteAsync(hct));
