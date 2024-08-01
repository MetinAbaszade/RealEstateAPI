using ENTITIES.Entities.Generic;

namespace ENTITIES.Entities;

public class Permission : Auditable, IEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Key { get; set; }
    public List<Role>? Roles { get; set; }
}