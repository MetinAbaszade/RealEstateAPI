using CORE.Abstract;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using IResult = DTO.Responses.IResult;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class OtpController(IOtpService otpService, IUserService userService) : ControllerBase
{
    [SwaggerOperation(Summary = "Send Otp")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("sendotp")]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequestDto dto)
    {
        var getUserResult = await userService.GetAsync(u => u.ContactNumber == dto.ContactNumber);
        if (getUserResult is not SuccessDataResult<User>)
        {
            return BadRequest(new ErrorResult(EMessages.UserIsNotExist.Translate()));
        }

        // var otp = otpService.GenerateOtp();
        var otp = "123456";
        otpService.SaveOtpinCache(dto.ContactNumber, otp);
        await otpService.SendOtpAsync(dto.ContactNumber, otp);

        return Ok(new SuccessResult(EMessages.Success.Translate()));
    }

    [SwaggerOperation(Summary = "validate otp")]
    [SwaggerResponse(StatusCodes.Status200OK, type: typeof(IResult))]
    [HttpPost("validateotp")]
    public IActionResult ValidateOtp([FromBody] ValidateOtpRequestDto dto)
    {
        var isValid = otpService.ValidateOtp(dto.ContactNumber, dto.Otp);
        if (!isValid)
        {
            return BadRequest(new ErrorResult(EMessages.InvalidVerificationCode.Translate()));
        }

        return Ok(new SuccessResult(EMessages.Success.Translate()));
    }
}

