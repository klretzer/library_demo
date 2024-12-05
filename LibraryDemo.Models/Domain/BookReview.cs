namespace LibraryDemo.Models.Domain;

public class BookReview : BaseEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("book_id")]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

    [ForeignKey(nameof(BookId))]
    public required Book Book { get; set; }

    [Column("rating")]
    public required int Rating { get; set; }

    [Column("text")]
    public string? Text { get; set; }
}