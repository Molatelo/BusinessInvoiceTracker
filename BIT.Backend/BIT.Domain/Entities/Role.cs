namespace BIT.Domain.Entities;

public class Role : Entity
{
    public required string Name { get; set; }
    public string? Code { get; private set; }
    public string? Description { get; set; }

    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
