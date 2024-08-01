using DTO.User;

namespace DTO.Auth;
public record LoginInfoByTokenResponseDto
{
    public required UserResponseDto User { get; set; }
    public required string AccessToken { get; set; }
}