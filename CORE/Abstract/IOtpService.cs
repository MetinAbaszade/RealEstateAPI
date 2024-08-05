using DTO.Auth;

namespace CORE.Abstract;
public interface IOtpService
{
    public string GenerateOtp();
    public Task SendOtpAsync(string contactNumber, string otp);
    public void SaveOtpinCache(string contactNumber, string otp);
    public bool ValidateOtp(ValidateOtpRequestDto dto);
}
