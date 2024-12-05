namespace LibraryDemo.Models.Domain;

public class Book : BaseEntity
{
    [Column("title")]
    public required string Title { get; init; }

    [Column("author")]
    public required string Author { get; init; }

    [Column("category")]
    public required string Category { get; init; }

    [Column("publisher")]
    public required string Publisher { get; init; }

    [Column("isbn")]
    public required Guid ISBN { get; init; }

    [Column("page_count")]
    public required int PageCount { get; init; }

    [Column("pub_date")]
    public required DateOnly PublicationDate { get; init; }

    [Column("img_url")]
    public string? CoverImageUrl { get; set; }

    [Column("description")]
    public string? Description { get; init; }
}