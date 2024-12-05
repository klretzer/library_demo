namespace LibraryDemo.Models.Identity;

public record RegistrationRequest : MessageBase
{
    public required string UserName { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string Role { get; init; }
}