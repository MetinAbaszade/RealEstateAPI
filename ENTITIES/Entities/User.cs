using ENTITIES.Entities.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENTITIES.Entities;

[Table("web_user")]
public class User : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Password { get; set; }
    public required string ContactNumber { get; set; }
    public required string Salt { get; set; }
    public required bool Verified { get; set; } = false;
    public virtual ICollection<Token> Tokens { get; set; } = [];
    public virtual ICollection<Favourite> Favourites { get; set; } = [];
}