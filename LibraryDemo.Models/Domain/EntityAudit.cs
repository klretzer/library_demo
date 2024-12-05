namespace LibraryDemo.Models.Domain;

public record EntityAudit
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("entity_id")]
    public Guid? EntityId { get; set; }

    [Column("state")]
    public required string State { get; set; } = string.Empty;

    [Column("type")]
    public required string Type { get; set; } = string.Empty;

    [Column("message")]
    public string? Message { get; set; }

    [Column("save_changes_audit_id")]
    public Guid? SaveChangesAuditId { get; set; }
}

public record SaveChangesAudit
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("user_id")]
    public Guid? UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Column("start_time")]
    public DateTimeOffset StartTime { get; set; }

    [Column("end_time")]
    public DateTimeOffset EndTime { get; set; }

    [Column("succeeded")]
    public bool Succeeded { get; set; }

    [Column("error_message")]
    public string? ErrorMessage { get; set; }

    public ICollection<EntityAudit> AuditEntries { get; set; } = [];
}