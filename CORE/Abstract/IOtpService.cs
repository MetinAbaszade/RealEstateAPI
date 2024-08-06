using DTO.Auth;
using DTO.Responses;

namespace CORE.Abstract;
public interface IOtpService
{
    public string GenerateOtp();
    public Task<IResult> SendOtpAsync(string contactNumber, string otp);
    public void SaveOtpinCache(string contactNumber, string otp);
    public bool ValidateOtp(string contactNumber, string otp);
}
