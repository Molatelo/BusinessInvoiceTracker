using BIT.Application.CQRS;
using BIT.Application.Users.Dtos;
using FluentValidation;

namespace BIT.Application.Users.Commands;

public class CreateUserCommand : ICommand<UserDto>
{
    public required CreateUserDto Input { get; set; }
}

public class CreateUserCommandHandler(IUserService userService) : ICommandHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return await userService.CreateAsync(command.Input, cancellationToken);
    }
}

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    private readonly IUserService _userService;
    public CreateUserCommandValidator(IUserService userService)
    {
        _userService = userService;

        RuleFor(x => x.Input.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")
            .WithMessage("Email must be a valid email address.")
            .MaximumLength(100)
            .WithMessage("Email must not exceed 100 characters.")
            .CustomAsync(async (email, context, cancellationToken) =>
            {
                if (await BeUniqueEmail(email, cancellationToken))
                {
                    context.AddFailure("Email", "Email is already in use.");
                }
            });

        RuleFor(x => x.Input.Username)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Matches(@"^[a-zA-Z]+$")
            .WithMessage("Username can only contain letters.")
            .MaximumLength(25)
            .WithMessage("Username must not exceed 25 characters.")
            .CustomAsync(async (username, context, cancellationToken) =>
            {
                if (await BeUniqueUsername(username, cancellationToken))
                {
                    context.AddFailure("Username", "Username is already in use.");
                }
            });

        RuleFor(x => x.Input.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .Matches(@"^[a-zA-Z-]+$")
            .WithMessage("Name can only contain letters and hyphens.")
            .MaximumLength(50)
            .WithMessage("Name must not exceed 50 characters.");

        RuleFor(x => x.Input.Surname)
            .NotEmpty()
            .WithMessage("Surname is required.")
            .Matches(@"^[a-zA-Z-]+$")
            .WithMessage("Surname can only contain letters and hyphens.")
            .MaximumLength(50)
            .WithMessage("Surname must not exceed 50 characters.");

        RuleFor(x => x.Input.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password must be at least 8 characters long and include uppercase, lowercase, number, and special character.")
            .MaximumLength(50)
            .WithMessage("Password must not exceed 50 characters.");
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        return await _userService.EmailExistAsync(email, cancellationToken);
    }

    private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
    {
        return await _userService.UsernameExistAsync(username, cancellationToken);
    }
}