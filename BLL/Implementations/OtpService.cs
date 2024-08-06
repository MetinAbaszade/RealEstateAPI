using CORE.Abstract;
using CORE.Localization;
using DTO.Auth;
using DTO.Responses;
using Microsoft.Extensions.Caching.Memory;
using static System.Net.WebRequestMethods;

namespace BLL.Implementations;
internal class OtpService(IMemoryCache memoryCache) : IOtpService
{
    public async Task<IResult> SendOtpAsync(string contactNumber, string otp)
    {
        // Here Will be real otp implementation of sending otp
        Console.WriteLine(otp);
        await Task.Delay(2000);
        return new SuccessResult(EMessages.VerificationCodeSent.Translate());
    }

    public string GenerateOtp()
    {
        var otp = new Random().Next(100_000, 999_999).ToString();
        return otp;
    }

    public void SaveOtpinCache(string contactNumber, string otp)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2),
        };
        memoryCache.Set(contactNumber, otp, cacheEntryOptions);
    }

    public bool ValidateOtp(string contactNumber, string otp)
    {
        if (memoryCache.TryGetValue(contactNumber, out string cachedOtp))
        {
            return cachedOtp == otp;
        }

        return false;
    }
}

