using BIT.Application.CQRS;
using BIT.Application.Users.Dtos;
using FluentValidation;

namespace BIT.Application.Users.Commands;

public class UserLoginCommand : ICommand<string>
{
    public required LoginDto Input { get; set; }
}

public class UserLoginCommandHandler(IUserService userService) : ICommandHandler<UserLoginCommand, string>
{
    public async Task<string> HandleAsync(UserLoginCommand command, CancellationToken cancellationToken)
    {
        return await userService.LoginAsync(command.Input, cancellationToken);
    }
}

public class UserLoginCommandValidator : AbstractValidator<UserLoginCommand>
{
    public UserLoginCommandValidator()
    {
        RuleFor(x => x.Input.Username)
            .NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.Input.Password)
            .NotEmpty().WithMessage("Password is required.");
    }
}
