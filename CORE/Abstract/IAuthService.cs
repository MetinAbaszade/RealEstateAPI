using DTO.Auth;
using DTO.Responses;
using DTO.User;

namespace CORE.Abstract;

public interface IAuthService
{
    Task<string?> GetUserSaltAsync(string contactNumber);

    Task<IDataResult<UserResponseDto>> LoginAsync(LoginRequestDto dto);

    Task<IDataResult<UserResponseDto>> LoginByTokenAsync();

    Task<IResult> LogoutAsync(string accessToken);

    Task<IResult> LogoutRemovedUserAsync(Guid userId);
}