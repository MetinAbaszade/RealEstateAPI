namespace DTO.Auth;

public record LoginRequestDto()
{
    public required string ContactNumber { get; set; }
    public required string Password { get; set; }
}