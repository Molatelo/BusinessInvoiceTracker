namespace BIT.Domain.Entities;

public class UserLogin : Entity
{
    public required int UserId { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public required string PasswordSalt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LastLoginDate { get; set; }

    public virtual required User User { get; set; }
}
