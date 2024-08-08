using DTO.Auth;
using DTO.Responses;
using DTO.Token;
using DTO.User;
using ENTITIES.Entities;

namespace CORE.Abstract;

public interface ITokenService
{
    Task<IResult> AddAsync(LoginResponseDto dto);

    Task<IResult> DeleteAsync(Guid id);
    Task<IResult> DeleteRangeAsync(List<Token> tokenstobeDeleted);

    Task<IDataResult<TokenToListDto>> GetAsync(string accessToken, string refreshToken);

    Task<IResult> CheckValidationAsync(string accessToken, string refreshToken);

    Task<IDataResult<LoginResponseDto>> CreateTokenAsync(UserResponseDto dto);

    public string? GetTokenString();

    public bool IsValidToken();

    public Guid GetUserIdFromToken();

    public Guid? GetCompanyIdFromToken();

    public string GenerateRefreshToken();

    public string TrimToken(string? jwtToken);
}