using BIT.Common.Utilities;
using BIT.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BIT.Infrastructure.Persistence;

public static class DataSeeder
{
    public static void Seed(DbContext context)
    {
        var user = context.Set<User>().FirstOrDefault(u => u.Email == "mpmabs@gmail.com");
        user ??= context.Set<User>().Add(
            new User
            {
                Name = "Molatelo",
                Surname = "Mabelebele",
                Email = "mpmabs@gmail.com",
                IsActive = true
            }
        ).Entity;

        string salt = CryptoUtility.GenerateSalt();
        string passwordHash = CryptoUtility.CreateHash("P4ssw0rd@26@", salt);
        var userLogin = context.Set<UserLogin>().FirstOrDefault(ul => ul.UserId == user.Id && ul.Username == "MolateloM");
        if (userLogin == null)
        {
            context.Set<UserLogin>().Add(new UserLogin
            {
                UserId = 1,
                Username = "MolateloM",
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                FailedLoginAttempts = 0,
                User = user
            });
        }

        var role = context.Set<Role>().FirstOrDefault(r => r.Code == "SUPER_ADMIN");
        role ??= context.Set<Role>().Add(
            new Role
            {
                Name = "Super Admin"
            }
        ).Entity;

        var userRole = context.Set<UserRole>().FirstOrDefault(ur => ur.UserId == user!.Id && ur.RoleId == role!.Id);
        if (userRole == null)
        {
            context.Set<UserRole>().Add(new UserRole
            {
                UserId = 1,
                RoleId = 1,
                User = user,
                Role = role
            });
        }
    }

    public static async Task SeedAsync(DbContext context, CancellationToken cancellationToken)
    {
        var user = await context.Set<User>().FirstOrDefaultAsync(u => u.Email == "mpmabs@gmail.com", cancellationToken: cancellationToken);
        user ??= (await context.Set<User>().AddAsync(new User
        {
            Name = "Molatelo",
            Surname = "Mabelebele",
            Email = "mpmabs@gmail.com",
            IsActive = true
        },
            cancellationToken
        )).Entity;

        string salt = CryptoUtility.GenerateSalt();
        string passwordHash = CryptoUtility.CreateHash("P4ssw0rd@26@", salt);
        var userLogin = await context.Set<UserLogin>().FirstOrDefaultAsync(ul => ul.UserId == user.Id && ul.Username == "MolateloM", cancellationToken: cancellationToken);
        if (userLogin == null)
        {
            await context.Set<UserLogin>().AddAsync(new UserLogin
            {
                UserId = 1,
                Username = "MolateloM",
                PasswordHash = passwordHash,
                PasswordSalt = salt,
                FailedLoginAttempts = 0,
                User = user
            });
        }

        var role = await context.Set<Role>().FirstOrDefaultAsync(r => r.Code == "SUPER_ADMIN", cancellationToken: cancellationToken);
        role ??= (await context.Set<Role>().AddAsync(new Role
        {
            Name = "Super Admin"
        },
            cancellationToken
        )).Entity;

        var userRole = await context.Set<UserRole>().FirstOrDefaultAsync(ur => ur.UserId == user!.Id && ur.RoleId == role!.Id, cancellationToken: cancellationToken);
        if (userRole == null)
        {
            await context.Set<UserRole>().AddAsync(new UserRole
            {
                UserId = 1,
                RoleId = 1,
                User = user,
                Role = role
            }, cancellationToken);
        }
    }
}
