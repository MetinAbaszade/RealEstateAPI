using API.Attributes;
using API.Filters;
using CORE.Abstract;
using CORE.Constants;
using CORE.Enums;
using CORE.Helpers;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using DTO.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sentry;
using Swashbuckle.AspNetCore.Annotations;
using Constants = CORE.Constants.Constants;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateToken]
public class UsersController(IUserService userService,
                             IUtilService utilService,
                             ITokenService tokenService,
                             IAuthService authService,
                             IOtpService otpService) : Controller
{
    [SwaggerOperation(Summary = "get users as paginated list")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserResponseDto>>))]
    [HttpGet("{pageIndex}/{pageSize}")]
    public async Task<IActionResult> GetAsPaginated([FromRoute] int pageIndex, int pageSize)
    {
        var response = await userService.GetAsPaginatedListAsync(pageIndex, pageSize);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get users")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserResponseDto>>))]
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var response = await userService.GetListAsync();
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get profile info")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<List<UserResponseDto>>))]
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfileInfo()
    {
        Guid userId = tokenService.GetUserIdFromToken();
        IDataResult<UserByIdResponseDto> response = await userService.GetAsync(userId);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "get user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IDataResult<UserResponseDto>))]
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var response = await userService.GetAsync(id);
        if (response is ErrorDataResult<UserByIdResponseDto>)
            return NotFound(response);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Add([FromBody] UserCreateRequestDto dto)
    {
        var response = await userService.AddAsync(dto);
        if (response is ErrorResult)
            return Conflict(response);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateRequestDto dto)
    {
        var response = await userService.UpdateAsync(id, dto);
        if (response is ErrorResult)
            return NotFound(response);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "reset user password by id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("{id}/resetPassword")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromRoute] Guid id, [FromBody] ResetPasswordRequestDto request)
    {
        var isOtpValid = otpService.ValidateOtp(request.ContactNumber, request.Otp);
        if (!isOtpValid)
        {
            return BadRequest(EMessages.InvalidVerificationCode.Translate());
        }
        var response = await userService.ResetPasswordAsync(id, request.Password);
        if (!response.Success)
        {
            return NotFound(response);
        }

        await authService.LogoutRemovedUserAsync(id);
        return Ok(response);
    }
}