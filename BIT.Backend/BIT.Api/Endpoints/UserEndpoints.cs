using BIT.Application.CQRS;
using BIT.Application.Users;
using BIT.Application.Users.Commands;
using BIT.Application.Users.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BIT.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var users = app.MapGroup("/api/users").WithTags("Users");

        users.MapPost("/", CreateUser)
            .RequireAuthorization()
            .WithName("CreateUser")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Creates a new user.");

        users.MapPost("/login", UserLogin)
            .WithName("UserLogin")
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .WithDescription("Authenticates a user and returns a JWT token.");
    }

    private static async Task<IResult> CreateUser([FromServices] ICommandDispatcher dispatcher, [FromServices] IUserService service, [FromBody] CreateUserDto input)
    {
        var command = new CreateUserCommand { Input = input };

        var validator = new CreateUserCommandValidator(service);
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        var result = await dispatcher.DispatchAsync(command);
        return Results.Ok(result);
    }

    private static async Task<IResult> UserLogin([FromServices] ICommandDispatcher dispatcher, [FromBody] LoginDto input)
    {
        var command = new UserLoginCommand { Input = input };
        var validator = new UserLoginCommandValidator();
        var validationResult = await validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            return Results.ValidationProblem(errors);
        }

        try
        {
            var result = await dispatcher.DispatchAsync(command);
            return Results.Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Results.Problem(ex.Message, null, StatusCodes.Status401Unauthorized);
        }
    }
}
