
namespace DTO.User;

public record UserResponseDto()
{
    public required Guid Id { get; set; }
    public required string ContactNumber { get; set; }
}