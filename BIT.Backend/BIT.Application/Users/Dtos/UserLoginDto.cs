namespace BIT.Application.Users.Dtos;

public class UserLoginDto
{
    public int UserId { get; set; }
    public required string Username { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LastLoginDate { get; set; }
}
