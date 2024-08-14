using CORE.Config;
using CORE.Localization;
using DTO.Responses;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly ConfigSettings _config;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, ConfigSettings config)
    {
        _next = next;
        _logger = logger;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("An error occurred: {Exception}", ex);
            LogError(httpContext, ex);
            await HandleExceptionAsync(httpContext);
        }
    }

    private void LogError(HttpContext httpContext, Exception ex)
    {
        var traceIdentifier = httpContext.TraceIdentifier;
        var clientIp = httpContext.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
        var path = httpContext.Request.Path;
        string? stackTrace = ex.StackTrace?.Length > 2000 ? ex.StackTrace[..2000] : ex.StackTrace;

        // Log details to a file (or other logging providers)
        _logger.LogError("TraceIdentifier: {TraceIdentifier}", traceIdentifier);
        _logger.LogError("Client IP: {ClientIp}", clientIp);
        _logger.LogError("Request Path: {Path}", path);
        _logger.LogError("Error Message: {ErrorMessage}", ex.Message);
        _logger.LogError("Stack Trace: {StackTrace}", stackTrace);
    }

    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

        var response = new ErrorResult(EMessages.GeneralError.Translate());
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}

