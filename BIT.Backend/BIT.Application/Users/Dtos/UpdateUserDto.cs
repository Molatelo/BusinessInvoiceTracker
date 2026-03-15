namespace BIT.Application.Users.Dtos;

public class UpdateUserDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; }
}
