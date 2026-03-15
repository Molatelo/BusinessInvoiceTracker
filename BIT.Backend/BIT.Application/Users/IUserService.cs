using BIT.Application.Users.Dtos;

namespace BIT.Application.Users;

public interface IUserService
{
    Task<UserDto> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken);
    Task<UserDto> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<UserDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<UserDto> UpdateAsync(int id, UpdateUserDto updateDto, CancellationToken cancellationToken);
    Task DeleteAsync(int id, CancellationToken cancellationToken);
    Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UsernameExistAsync(string username, CancellationToken cancellationToken = default);
}
