﻿using API.Attributes;
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
        return Ok(response);
    }

    [SwaggerOperation(Summary = "create user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Add([FromBody] UserCreateRequestDto dto)
    {
        var response = await userService.AddAsync(dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "update user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateRequestDto dto)
    {
        var response = await userService.UpdateAsync(id, dto);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "delete user")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var response = await userService.SoftDeleteAsync(id);
        await authService.LogoutRemovedUserAsync(id);
        return Ok(response);
    }

    [SwaggerOperation(Summary = "upload logged in user image")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("upload/image")]
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        Guid userId = tokenService.GetUserIdFromToken();
        return await UploadImage(userId, file);
    }

    [SwaggerOperation(Summary = "upload image by user id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("{id}/upload/image")]
    public async Task<IActionResult> UploadImage([FromRoute] Guid id, IFormFile file)
    {
        string fileExtension = System.IO.Path.GetExtension(file.FileName);
        Guid fileNewName = Guid.NewGuid();

        if (!Constants.AllowedImageExtensions.Contains(fileExtension))
        {
            return BadRequest(new ErrorDataResult<string>(EMessages.ThisFileTypeIsNotAllowed.Translate()));
        }

        var path = utilService.GetEnvFolderPath(utilService.GetFolderName(EFileType.UserImages));
        await FileHelper.WriteFile(file, $"{fileNewName}{fileExtension}", path);

        await userService.SetImageAsync(id, $"{fileNewName}{fileExtension}");

        return Ok(new SuccessResult(EMessages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "delete logged in user image")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("image")]
    public async Task<IActionResult> DeleteImage()
    {
        Guid userId = tokenService.GetUserIdFromToken();
        var response = await DeleteImage(userId);
        return response;
    }

    [SwaggerOperation(Summary = "delete image by id")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpDelete("{id}/image")]
    public async Task<IActionResult> DeleteImage([FromRoute] Guid id)
    {
        var existFile = await userService.GetImageAsync(id);

        if (existFile is null || !existFile.Success)
        {
            return BadRequest(new ErrorDataResult<string>(EMessages.UserIsNotExist.Translate()));
        }

        if (existFile!.Data is null)
        {
            return Ok(new SuccessResult(EMessages.Success.Translate()));
        }

        var path = utilService.GetEnvFolderPath(utilService.GetFolderName(EFileType.UserImages));
        var fullPath = System.IO.Path.Combine(path, existFile!.Data);

        if (System.IO.File.Exists(fullPath))
        {
            System.IO.File.Delete(fullPath);
        }

        await userService.SetImageAsync(id);

        return Ok(new SuccessResult(EMessages.Success.Translate()));
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
            return BadRequest(response);
        }

        await authService.LogoutRemovedUserAsync(id);
        return Ok(response);
    }
}