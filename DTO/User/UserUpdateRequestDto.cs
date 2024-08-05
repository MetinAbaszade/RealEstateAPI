namespace DTO.User;

public record UserUpdateRequestDto()
{
    public required string ContactNumber { get; set; }
    public required string Username { get; set; }
    public required bool Verified { get; set; }
    public Guid? RoleId { get; set; }
}