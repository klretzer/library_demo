namespace LibraryDemo.Core.DTO;

public record BookDTO
{
    public required Guid Id { get; set; }

    public required string Title { get; init; }

    public required string Author { get; init; }

    public required string Description { get; init; }

    public required string Category { get; init; }

    public required string Publisher { get; init; }

    public required string CoverImageUrl { get; init; }

    public required int PageCount { get; init; }

    public required Guid ISBN { get; init; }

    public required DateOnly PublicationDate { get; init; }
}