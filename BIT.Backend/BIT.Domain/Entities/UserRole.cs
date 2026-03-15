namespace BIT.Domain.Entities;

public class UserRole : Entity
{
    public required int UserId { get; set; }
    public required int RoleId { get; set; }

    public virtual required User User { get; set; }
    public virtual required Role Role { get; set; }
}
