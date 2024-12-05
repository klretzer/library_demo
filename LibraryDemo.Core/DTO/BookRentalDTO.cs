namespace LibraryDemo.Core.DTO;

public record BookRentalDTO
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; init; }

    public required Guid BookId { get; init; }

    public required DateTime DueDate { get; init; }
}