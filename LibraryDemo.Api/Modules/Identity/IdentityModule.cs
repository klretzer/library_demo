namespace LibraryDemo.Api.Modules.Identity;

public class IdentityModule : IModule
{
    public IServiceCollection AddServices(IServiceCollection services)
    {
        services.AddScoped<TokenService, TokenService>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder builder)
    {
        var identityEndpoints = builder.MapGroup("/identity");

        identityEndpoints.MapPost("/register", RegisterAsync)
            .AllowAnonymous()
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Register user." });

        identityEndpoints.MapPost("/login", LoginAsync)
            .AllowAnonymous()
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Authenticate user." });

        identityEndpoints.MapPost("/logout", LogoutAsync)
            .RequireAuthorization("RequireRegisteredUser")
            .WithOpenApi(e => new OpenApiOperation(e) { Summary = "Logout user." });

        return builder;
    }

    public static async Task<IResult> RegisterAsync(IMediator mediator, [FromBody] RegisterUserCommand command)
    {
        await mediator.Send(command);

        return TypedResults.Created();
    }

    //public static async Task<IResult> GetTokenAsync(
    //    TokenService tokenService,
    //    UserManager<User> userManager,
    //    [FromBody] AuthenticateRequest request)
    //{
    //    var user = await userManager.FindByNameAsync(request.UserName!);

    //    if (user == null)
    //    {
    //        return TypedResults.Unauthorized();
    //    }

    //    var result = await userManager.CheckPasswordAsync(user, request.Password!);

    //    if (!result)
    //    {
    //        return TypedResults.Unauthorized();
    //    }

    //    var token = await tokenService.GenerateTokenAsync(request.UserName!);
    //    var response = new AuthenticateResponse
    //    {
    //        UserName = request.UserName,
    //        Token = token
    //    };

    //    return TypedResults.Ok(response);
    //}

    public static async Task<IResult> LoginAsync(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        [FromBody] AuthenticateRequest request)
    {
        var result = await signInManager.PasswordSignInAsync(request.UserName!, request.Password!, false, false);

        if (!result.Succeeded)
        {
            return TypedResults.Unauthorized();
        }

        var response = new AuthenticateResponse
        {
            UserName = request.UserName,
            Success = result.Succeeded,
            IsLockedOut = result.IsLockedOut,
            IsNotAllowed = result.IsNotAllowed,
            RequiresTwoFactor = result.RequiresTwoFactor
        };

        return TypedResults.Ok(response);
    }

    public static async Task<IResult> LogoutAsync(SignInManager<User> signInManager, [FromBody] object empty)
    {
        if (empty == null)
        {
            return TypedResults.Unauthorized();
        }

        await signInManager.SignOutAsync();

        return TypedResults.Ok();
    }
}