namespace BIT.Domain.Entities;

public class User : Entity
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }

    public virtual UserLogin? UserLogin { get; set; }
    public virtual ICollection<UserRole>? UserRoles { get; set; }
}
