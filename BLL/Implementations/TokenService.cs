using AutoMapper;
using BLL.Helpers;
using CORE.Abstract;
using CORE.Config;
using CORE.Localization;
using DAL.EntityFramework.Abstract;
using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using DTO.User;
using ENTITIES.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Implementations;

public class TokenService(ConfigSettings configSettings,
                         IMapper mapper,
                         ITokenRepository tokenRepository,
                         IHttpContextAccessor context,
                         IUtilService utilService) : ITokenService
{

    public async Task<DTO.Responses.IResult> AddAsync(LoginResponseDto dto)
    {
        var data = mapper.Map<Token>(dto);
        await tokenRepository.AddAsync(data);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public async Task<IDataResult<TokenToListDto>> GetAsync(string accessToken, string refreshToken)
    {
        var token = await tokenRepository.GetAsync(m => m.AccessToken == accessToken &&
                                                                    m.RefreshToken == refreshToken &&
                                                                    m.RefreshTokenExpireDate > DateTime.UtcNow);

        if (token == null)
        {
            return new ErrorDataResult<TokenToListDto>(EMessages.PermissionDenied.Translate());
        }

        var data = mapper.Map<TokenToListDto>(token);

        return new SuccessDataResult<TokenToListDto>(data, EMessages.Success.Translate());
    }

    public async Task<DTO.Responses.IResult> CheckValidationAsync(string accessToken, string refreshToken)
    {
        return await tokenRepository.IsValidAsync(accessToken, refreshToken)
               ? new SuccessResult(EMessages.Success.Translate())
               : new ErrorResult(EMessages.PermissionDenied.Translate());
    }

    public async Task<IDataResult<LoginResponseDto>> CreateTokenAsync(UserResponseDto dto)
    {
        var securityHelper = new SecurityHelper(configSettings, utilService);
        var accessTokenExpireDate = DateTime.UtcNow.AddHours(configSettings.AuthSettings.TokenExpirationTimeInHours);

        var loginResponseDto = new LoginResponseDto()
        {
            User = dto,
            AccessToken = securityHelper.CreateTokenForUser(dto, accessTokenExpireDate),
            AccessTokenExpireDate = accessTokenExpireDate,
            RefreshToken = GenerateRefreshToken(),
            RefreshTokenExpireDate = accessTokenExpireDate.AddMinutes(configSettings.AuthSettings.RefreshTokenAdditionalMinutes)
        };

        await AddAsync(loginResponseDto);

        return new SuccessDataResult<LoginResponseDto>(loginResponseDto, EMessages.Success.Translate());
    }

    public async Task<DTO.Responses.IResult> DeleteAsync(Guid id)
    {
        var data = await tokenRepository.GetAsync(id);
        await tokenRepository.DeleteAsync(data!);

        return new SuccessResult(EMessages.Success.Translate());
    }

    public string GetTokenString()
    {
        return context.HttpContext?.Request.Headers[configSettings.AuthSettings.HeaderName].ToString()!;
    }

    public Guid GetUserIdFromToken()
    {
        var token = GetJwtSecurityToken();
        var userId = utilService.Decrypt(token.Claims.First(c => c.Type == configSettings.AuthSettings.TokenUserIdKey).Value);
        return Guid.Parse(userId);
    }

    public Guid? GetCompanyIdFromToken()
    {
        var token = GetJwtSecurityToken();
        if (token is null)
        {
            return null;
        }

        var companyIdClaim = token.Claims.First(c => c.Type == configSettings.AuthSettings.TokenCompanyIdKey);

        if (companyIdClaim is null || string.IsNullOrEmpty(companyIdClaim.Value))
        {
            return null;
        }

        return Guid.Parse(companyIdClaim.Value);
    }

    public bool IsValidToken()
    {
        var tokenString = GetTokenString();

        if (string.IsNullOrEmpty(tokenString) || tokenString.Length < 7)
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = Encoding.ASCII.GetBytes(configSettings.AuthSettings.SecretKey);
        try
        {
            tokenHandler.ValidateToken(tokenString[7..], new TokenValidationParameters
            {
                ValidateIssuerSigningKey = false,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public string TrimToken(string? jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken) || jwtToken.Length < 7)
        {
            throw new Exception();
        }

        return jwtToken[7..];
    }

    private JwtSecurityToken GetJwtSecurityToken()
    {
        var tokenString = GetTokenString();
        return new JwtSecurityToken(tokenString[7..]);
    }
}