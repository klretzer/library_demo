namespace LibraryDemo.Models.Common;

public class BaseEntity : ISoftDeletable
{
    [Column("id")]
    public Guid Id { get; init; }

    [Timestamp]
    [Column("version")]
    public required byte[] Version { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; } = false;
}