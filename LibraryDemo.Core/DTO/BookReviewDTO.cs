namespace LibraryDemo.Core.DTO;

public record BookReviewDTO
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; init; }

    public required Guid BookId { get; init; }

    public required int Rating { get; init; }

    public string? Text { get; set; }
}