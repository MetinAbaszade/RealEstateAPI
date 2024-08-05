using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class User : Auditable, IEntity
{
    public Guid Id { get; set; }
    public string? Image { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ContactNumber { get; set; }
    public required string Salt { get; set; }
    public bool Verified { get; set; } = false;
}