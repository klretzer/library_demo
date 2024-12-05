namespace LibraryDemo.Models.Identity;

public record AuthenticateRequest : MessageBase
{
    public required string UserName { get; init; }

    public required string Password { get; init; }
}