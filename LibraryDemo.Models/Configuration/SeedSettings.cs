namespace LibraryDemo.Models.Configuration;

public sealed record SeedSettings
{
    public string DefaultPassword { get; init; } = string.Empty;

    public int NumberOfFakeBooks { get; init; }

    public int NumberOfFakeUsers { get; init; }

    public int NumberOfFakeReviews { get; init; }

    public int NumberOfFakeRentals { get; init; }
}
