using API.Attributes;
using API.Filters;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Config;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using DTO.User;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;
using Result = DTO.Responses.Result;

namespace API.Controllers;

[Route("api/[controller]")]
[ServiceFilter(typeof(LogActionFilter))]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController(IAuthService authService,
                            ConfigSettings configSettings,
                            IUserService userService,
                            ITokenService tokenService,
                            IOtpService otpService) : Controller
{
    [SwaggerOperation(Summary = "login")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var userSalt = await authService.GetUserSaltAsync(dto.ContactNumber);

        if (string.IsNullOrEmpty(userSalt))
        {
            return Ok(new ErrorDataResult<Result>(EMessages.InvalidUserCredentials.Translate()));
        }

        dto = dto with { Password = SecurityHelper.HashPassword(dto.Password, userSalt) };

        var loginResult = await authService.LoginAsync(dto);
        if (!loginResult.Success)
        {
            return Unauthorized(loginResult);
        }

        var response = await tokenService.CreateTokenAsync(loginResult.Data!);

        return Ok(response);
    }

    [SwaggerOperation(Summary = "register")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateRequestDto dto)
    {
        var registerResult = await userService.AddAsync(dto);
        if (!registerResult.Success)
        {
            return BadRequest(registerResult);
        }

        var otp = otpService.GenerateOtp();
        otpService.SaveOtpinCache(dto.ContactNumber, otp);
        await otpService.SendOtpAsync(dto.ContactNumber, otp);

        return Ok(registerResult);
    }

    [SwaggerOperation(Summary = "validat eotp")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("validateotp")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateOtp([FromBody] ValidateOtpRequestDto dto)
    {
        var isValid = otpService.ValidateOtp(dto.ContactNumber, dto.Otp);
        if (!isValid)
        {
            return BadRequest(new ErrorResult(EMessages.InvalidVerificationCode.Translate()));
        }

        // Change user's verified status to true
        var result = await userService.UpdateVerifiedStatusAsync(dto.ContactNumber, true);

        if (!result.Success)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }

    [SwaggerOperation(Summary = "refesh access token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("refresh/token")]
    public async Task<IActionResult> RefreshToken()
    {
        var jwtToken = tokenService.TrimToken(HttpContext.Request.Headers[configSettings.AuthSettings.HeaderName]!);

        string refreshToken = HttpContext.Request.Headers[configSettings.AuthSettings.RefreshTokenHeaderName]!;

        var tokenResponse = await tokenService.GetAsync(jwtToken, refreshToken);
        if (tokenResponse.Success)
        {
            await tokenService.SoftDeleteAsync(tokenResponse.Data!.Id);
            var response = await tokenService.CreateTokenAsync(tokenResponse.Data.User);
            return Ok(response);
        }

        return Unauthorized();
    }

    [SwaggerOperation(Summary = "login by token")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<LoginResponseDto>))]
    [ValidateToken]
    [HttpGet("loginInfo")]
    public async Task<IActionResult> LoginInfoByToken()
    {
        if (string.IsNullOrEmpty(HttpContext.Request.Headers.Authorization))
        {
            return Unauthorized(new ErrorResult(EMessages.CanNotFoundUserIdInYourAccessToken.Translate()));
        }

        var loginByTokenResponse = await authService.LoginByTokenAsync();
        if (!loginByTokenResponse.Success)
        {
            return BadRequest(loginByTokenResponse.Data);
        }

        LoginInfoByTokenResponseDto loginInfoByTokenResponseDto = new()
        {
            User = loginByTokenResponse.Data!,
            AccessToken = tokenService.TrimToken(tokenService.GetTokenString())!
        };

        return Ok(new SuccessDataResult<LoginInfoByTokenResponseDto>(loginInfoByTokenResponseDto, EMessages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "logout")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [ValidateToken]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var accessToken = tokenService.TrimToken(tokenService.GetTokenString()!);
        var response = await authService.LogoutAsync(accessToken);

        return Ok(response);
    }
}