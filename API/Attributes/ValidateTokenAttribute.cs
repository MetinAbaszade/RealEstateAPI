using CORE.Abstract;
using CORE.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes;

public class ValidateTokenAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (hasAllowAnonymous)
        {
            return;
        }

        var configSettings = (context.HttpContext.RequestServices.GetService(typeof(ConfigSettings)) as ConfigSettings)!;
        var tokenService = (context.HttpContext.RequestServices.GetService(typeof(ITokenService)) as ITokenService)!;

        string? jwtToken = tokenService.GetTokenString();
        string? refreshToken = context.HttpContext.Request.Headers[configSettings.AuthSettings.RefreshTokenHeaderName];

        jwtToken = tokenService.TrimToken(jwtToken);

        var validationResult = tokenService.CheckValidationAsync(jwtToken, refreshToken!).Result;

        if (!validationResult.Success)
        {
            context.Result = new UnauthorizedObjectResult(validationResult);
        }
    }
}