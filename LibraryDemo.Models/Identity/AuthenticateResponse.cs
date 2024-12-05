namespace LibraryDemo.Models.Identity;

public record AuthenticateResponse : MessageBase
{
    public string? UserName { get; init; } = string.Empty;

    public bool Success { get; init; } = false;

    public bool IsLockedOut { get; init; } = false;

    public bool IsNotAllowed { get; init; } = false;

    public bool RequiresTwoFactor { get; init; } = false;

    public string? Token { get; init; } = string.Empty;
}