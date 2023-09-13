using Acme.Base.Domain.CosmosDb.Repository;
using Acme.Base.Domain.Service;
using Acme.Base.Repository.CosmosDb;
using Acme.FlightManager.Common;
using Acme.FlightManager.Common.Domain.Service;
using Acme.FlightManager.WebApi.AspNet.Apis;
using Acme.FlightManager.WebApi.AspNet.Middleware;
using Asp.Versioning;
using Asp.Versioning.Builder;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Scrutor;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var app = SetWebApplicationBuilder().Build();
SetWebApplication(app).Run();

WebApplicationBuilder SetWebApplicationBuilder()
{
    var config = builder.Configuration;

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(SwaggerConfiguration);

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
    builder.Services.AddAuthorization(AuthorizationConfiguration);

    builder.Services.AddValidatorsFromAssemblyContaining<Program>();

    builder.Services.AddApiVersioning(VersioningConfiguration);

    builder.Services.AddSingleton(AddCosmosClient);
    builder.Services.AddScoped<ICosmosDbRepository, AcmeCosmosContext>(AddAcmeContext);
    builder.Services.AddScoped<ICosmosDbUpsertUnitOfWork, AcmeCosmosContext>(AddAcmeContext);
    builder.Services.AddScoped<ICosmosDbDeleteUnitOfWork, AcmeCosmosContext>(AddAcmeContext);
    builder.Services.AddScoped<ICommandDispatcher, CommandDispatcher>();
    builder.Services.AddScoped<IQueryDispatcher, QueryDispatcher>();
    builder.Services.AddSingleton<CurrentTimeService>();
    builder.Services.AddScoped(SetFreezeTime);
    builder.Services.Scan(ApplicationAssembliesForMatchingInterfaces);
    builder.Services.Decorate(typeof(ICommandDispatcher), typeof(CommandValidationDispatcher));

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
    //app.UseExceptionHandler(ExceptionHandlerConfiguration);

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapGroup("plane")
        .MapPlaneApis()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(1.0)
        .WithOpenApi()
        .AllowAnonymous();

    app.MapGroup("flight")
        .MapFlightApis()
        .WithApiVersionSet(versionSet)
        .MapToApiVersion(1.0)
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

static void VersioningConfiguration(ApiVersioningOptions avo)
{
    avo.DefaultApiVersion = new(1.0);
    avo.AssumeDefaultVersionWhenUnspecified = true;
}

CosmosClient AddCosmosClient(IServiceProvider serviceProvider) =>
    new CosmosClientBuilder(builder.Configuration.GetValue<string>("AzureCosmosDbConnectionString"))
        .WithConnectionModeDirect()
        .WithCustomSerializer(new CosmosNewtonsoftJsonSerializer())
        .Build();

static ITimeService SetFreezeTime(IServiceProvider serviceProvider) =>
    new CurrentTimeFreezeService(serviceProvider.GetRequiredService<CurrentTimeService>());

static void ApplicationAssembliesForMatchingInterfaces(ITypeSourceSelector selector)
{
    var onDot = '.';
    var currentAssemblyPartedFullName = Assembly.GetExecutingAssembly().GetName().Name.Split(onDot);
    var companyName = currentAssemblyPartedFullName[0];
    var executingProjectName = currentAssemblyPartedFullName.Last();

    selector
        .FromAssemblies(CustomBuildForThisProject())
        .AddClasses(FromCompanyNamespace)
        .AsImplementedInterfaces()
        .WithScopedLifetime();

    IEnumerable<Assembly> CustomBuildForThisProject() =>
        DependencyContext.Default.RuntimeLibraries
            .Where(RuntimeLibraryIsFromProject)
            .Select(AssemblyFromRuntimeLibrary);

    bool RuntimeLibraryIsFromProject(RuntimeLibrary runtimeLibrary) =>
        runtimeLibrary.Name.Contains(companyName) &&
        !runtimeLibrary.Name.Contains(executingProjectName);

    Assembly AssemblyFromRuntimeLibrary(RuntimeLibrary runtimeLibrary) =>
        Assembly.Load(new AssemblyName(runtimeLibrary.Name));

    void FromCompanyNamespace(IImplementationTypeFilter implementationTypeFilter) =>
        implementationTypeFilter
            .Where(tpe => tpe.Namespace.StartsWith(companyName) &&
                tpe.GetInterfaces().Any() &&
                !tpe.Name.EndsWith("Command") &&
                !tpe.Namespace.Contains("Common") &&
                !tpe.Namespace.EndsWith("DataTransferObject") &&
                !tpe.Namespace.EndsWith("Entity") &&
                !tpe.Namespace.EndsWith("Factory") &&
                !tpe.Namespace.EndsWith("Repository.CosmosDb") &&
                !tpe.Namespace.EndsWith("ValueObject"));
}

static AcmeCosmosContext AddAcmeContext(IServiceProvider serviceProvider) =>
    new(serviceProvider.GetRequiredService<CosmosClient>(),
        nameof(AcmeApplications),
        EnumerationService.GetEnumValues<AcmeApplications>().Select(enm => enm.ToString()).ToList());

static ApiVersionSet GetApiVersionSet(WebApplication webApplication) =>
    webApplication
        .NewApiVersionSet()
        .HasApiVersion(new(1.0))
        .ReportApiVersions()
        .Build();

static void ExceptionHandlerConfiguration(IApplicationBuilder applicationBuilder) =>
    applicationBuilder.Run(async hct => await TypedResults.Problem().ExecuteAsync(hct));
