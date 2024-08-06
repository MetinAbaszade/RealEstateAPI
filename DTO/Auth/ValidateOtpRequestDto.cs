
namespace DTO.Auth;
public class ValidateOtpRequestDto
{
    public ValidateOtpRequestDto(string contactNumber, string otp)
    {
        ContactNumber = contactNumber;
        Otp = otp;
    }

    public required string ContactNumber { get; set; }
    public required string Otp { get; set; }
}
