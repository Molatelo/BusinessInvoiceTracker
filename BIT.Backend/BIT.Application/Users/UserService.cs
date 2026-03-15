using BIT.Application.Common.Services;
using BIT.Application.Users.Dtos;
using BIT.Common.Utilities;
using BIT.Domain.Entities;
using BIT.Domain.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BIT.Application.Users;

public class UserService(IRepository<int, User> repository, IRepository<int, UserLogin> userLoginRepository, IMapper mapper, IConfiguration congiguration) : BaseService<int, User, UserDto, CreateUserDto, UpdateUserDto>(repository, mapper), IUserService
{
    private readonly IRepository<int, User> _repository = repository;
    private readonly IMapper _mapper = mapper;
    public override async Task<UserDto> CreateAsync(CreateUserDto createDto, CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<User>(createDto);
        user.IsActive = true;

        await _repository.AddAsync(user, cancellationToken);

        string salt = CryptoUtility.GenerateSalt();
        string passwordHash = CryptoUtility.CreateHash(createDto.Password, salt);

        var userLogin = new UserLogin
        {
            UserId = user.Id,
            Username = createDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = salt,
            FailedLoginAttempts = 0,
            User = user
        };

        await userLoginRepository.AddAsync(userLogin, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<string> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var userLogin = await userLoginRepository.GetSingleOrDefaultAsync(u => u.Username == dto.Username, cancellationToken) ?? throw new UnauthorizedAccessException("Invalid username or password.");
        var user = await _repository.GetByIdAsync(userLogin.UserId, cancellationToken);
        if (!user!.IsActive)
        {
            throw new UnauthorizedAccessException("User account is inactive.");
        }

        if (!CryptoUtility.VerifyHash(dto.Password, userLogin.PasswordHash))
        {
            userLogin.FailedLoginAttempts += 1;
            if (userLogin.FailedLoginAttempts > 3)
            {
                user.IsActive = false;
            }

            await userLoginRepository.UpdateAsync(userLogin, cancellationToken);
            await _repository.UpdateAsync(user, cancellationToken);
            await _repository.SaveAsync(cancellationToken);
            throw new UnauthorizedAccessException("Invalid username or password.");
        }

        userLogin.FailedLoginAttempts = 0;
        userLogin.LastLoginDate = DateTime.UtcNow;
        await _repository.SaveAsync(cancellationToken);

        return GenerateJwtToken(user);
    }

    public async Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _repository.CountAsync(u => u.Email == email, cancellationToken) > 0;
    }

    public async Task<bool> UsernameExistAsync(string username, CancellationToken cancellationToken = default)
    {
        return await userLoginRepository.CountAsync(u => u.Username == username, cancellationToken) > 0;
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSecret = congiguration["Jwt:Key"];
        var jwtIssuer = congiguration["Jwt:Issuer"];
        var jwtAudience = congiguration["Jwt:Audience"];
        var jwtExpiryInMinutes = int.Parse(congiguration["ExpiryInMinutes"] ?? "60");

        if (string.IsNullOrEmpty(jwtSecret) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
        {
            throw new InvalidOperationException("JWT configuration is missing.");
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("id", user.Id.ToString()),
                new Claim("username", user.UserLogin!.Username),
                new Claim("email", user.Email),
                new Claim("fullName", user.Name + " " + user.Surname)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(jwtExpiryInMinutes),
            Issuer = jwtIssuer,
            Audience = jwtAudience,
            SigningCredentials = credentials
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
