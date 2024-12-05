namespace LibraryDemo.Models.Configuration;

public sealed record LibrarySettings
{
    public IdentitySettings? Identity { get; init; }

    public SeedSettings? Seed { get; init; }
}