namespace LibraryDemo.Models.Common;

public record MessageBase
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
}