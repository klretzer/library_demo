namespace LibraryDemo.Models.Domain;

public class BookRental : BaseEntity
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("book_id")]
    public Guid BookId { get; set; }

    [ForeignKey(nameof(UserId))]
    [Column("user_id")]
    public required User User { get; set; }

    [ForeignKey(nameof(BookId))]
    [Column("book_id")]
    public required Book Book { get; set; }

    [Column("due_date")]
    public required DateTime DueDate { get; init; }
}