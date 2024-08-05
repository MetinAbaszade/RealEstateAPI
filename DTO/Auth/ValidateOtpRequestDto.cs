
namespace DTO.Auth;
public class ValidateOtpRequestDto
{
    public required string ContactNumber { get; set; }
    public required string Otp { get; set; }
}
