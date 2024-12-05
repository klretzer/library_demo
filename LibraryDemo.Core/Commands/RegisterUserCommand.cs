namespace LibraryDemo.Core.Commands;

public record RegisterUserCommand : IRequest
{
    public required string Email { get; set; }

    public required string UserName { get; set; }

    public required string Password { get; set; }

    public required string Role { get; set; }
}

public class RegisterUserCommandHandler(
    RoleManager<IdentityRole<Guid>> roleManager,
    UserManager<User> userManager,
    IdentityContext context) : IRequestHandler<RegisterUserCommand>
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;

    private readonly UserManager<User> _userManager = userManager;

    private readonly IdentityContext _context = context;

    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        _ = await CheckRoleAsync(command.Role);

        var strategy = _context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            await RegisterUserAsync(command, cancellationToken);
        });
    }

    private async Task RegisterUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            _ = await CreateUserAsync(command, cancellationToken);
            _ = await AddRoleToNewUserAsync(command, cancellationToken);

            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }

    private async Task<IdentityRole<Guid>> CheckRoleAsync(string role)
    {
        return await _roleManager.FindByNameAsync(role) ?? throw new ValidationException($"Role '{role}' is invalid.");
    }

    private async Task<int> CreateUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _userManager.CreateAsync(new User { UserName = command.UserName, Email = command.Email }, command.Password);

        if (!result.Succeeded)
        {
            throw new Exception($"Error adding user: {result}");
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }

    private async Task<int> AddRoleToNewUserAsync(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var createdUser = await _userManager.FindByNameAsync(command.UserName);
        var roleResult = await _userManager.AddToRoleAsync(createdUser!, command.Role);

        if (!roleResult.Succeeded)
        {
            throw new Exception($"Error adding role: {roleResult}");
        }

        return await _context.SaveChangesAsync(cancellationToken);
    }
}