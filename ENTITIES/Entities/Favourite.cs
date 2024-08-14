namespace ENTITIES.Entities;

public partial class Favourite
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required Guid UserId { get; set; }

    public required int PropertyId { get; set; }

    public virtual Property Property { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
