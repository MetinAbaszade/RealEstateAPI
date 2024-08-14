using API.Attributes;
using API.Containers;
using API.Filters;
using API.Middlewares;
using BLL.Mappers;
using CORE.Config;
using CORE.Constants;
using DAL.Context;
using DTO;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var config = new ConfigSettings();

builder.Configuration.GetSection(nameof(ConfigSettings)).Bind(config);

builder.Services.TryAddSingleton(config);

builder.Services.AddControllers(opt => opt.Filters.Add(typeof(ModelValidatorActionFilter)))
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<DtoObject>();

builder.Services.AddAutoMapper(Automapper.GetAutoMapperProfilesFromAllAssemblies().ToArray());

builder.Services.AddHttpContextAccessor();

builder.Services.AddMemoryCache();

// configure max request body size as 60 MB
builder.Services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = 60 * 1024 * 1024);

builder.Services.RegisterRepositories();
builder.Services.RegisterApiVersioning();

builder.Services.AddDbContext<PropertyDbContext>(options => options.UseSqlServer(config.ConnectionStrings.AppDb));

builder.Services.RegisterAuthentication(config);

builder.Services.AddCors(o => o
                .AddPolicy(Constants.CORS_POLICY_NAME, b => b
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()));

builder.Services.AddScoped<ModelValidatorActionFilter>();
builder.Services.AddScoped<ValidateTokenAttribute>();

builder.Services.AddEndpointsApiExplorer();

if (config.SwaggerSettings.IsEnabled)
{
    builder.Services.RegisterSwagger(config);
}

builder.Services.RegisterMiniProfiler();

var app = builder.Build();

if (config.SwaggerSettings.IsEnabled)
{
    app.UseSwagger();
}

if (config.SwaggerSettings.IsEnabled)
{
    app.UseSwaggerUI(c =>
    {
        c.EnablePersistAuthorization();
        c.InjectStylesheet(config.SwaggerSettings.Theme);
    });
}

app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Use((context, next) =>
{
    context.Request.EnableBuffering();
    return next();
});

app.UseStaticFiles();

app.UseCors(Constants.CORS_POLICY_NAME);

app.UseMiniProfiler();

app.MapControllers();

app.Run();