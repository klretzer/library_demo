namespace LibraryDemo.Models.Configuration;

public sealed record IdentitySettings
{
    public PasswordSettings? PasswordSettings { get; init; }

    public JwtSettings? JwtSettings { get; init; }

    public string DefaultAuthScheme { get; set; } = string.Empty;
}
