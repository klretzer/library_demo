namespace LibraryDemo.Data.Interceptors;

public class SaveChangesInterceptor(
    IHttpContextAccessor contextAccessor,
    IOptions<ConnectionStrings> strings) : ISaveChangesInterceptor
{
    private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

    private SaveChangesAudit _audit = null!;

    public async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData data,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (data.Context == null) return result;

        _audit = CreateAudit(data.Context);

        using var auditContext = new AuditContext(strings);

        await auditContext.Set<SaveChangesAudit>().AddAsync(_audit, cancellationToken);
        await auditContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        using var auditContext = new AuditContext(strings);

        auditContext.Attach(_audit);

        _audit.Succeeded = true;
        _audit.EndTime = DateTimeOffset.Now;

        await auditContext.SaveChangesAsync(cancellationToken);

        return result;
    }

    public async Task SavedChangesFailedAsync(
        DbContextErrorEventData eventData,
        CancellationToken cancellationToken = default)
    {
        using var auditContext = new AuditContext(strings);

        auditContext.Attach(_audit);

        _audit.Succeeded = false;
        _audit.EndTime = DateTimeOffset.Now;
        _audit.ErrorMessage = (eventData.Exception.InnerException?.Message) ?? eventData.Exception.Message;

        await auditContext.SaveChangesAsync(cancellationToken);
    }

    private SaveChangesAudit CreateAudit(DbContext context)
    {
        context.ChangeTracker.DetectChanges();

        var audit = new SaveChangesAudit
        {
            Id = Guid.NewGuid(),
            StartTime = DateTimeOffset.Now,
            AuditEntries = CreateAuditEntries(context.ChangeTracker.Entries())
        };
        var contextUserId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (contextUserId != null)
        {
            _ = Guid.TryParse(contextUserId, out Guid userId);

            audit.UserId = userId;
        }

        return audit;
    }

    private static ICollection<EntityAudit> CreateAuditEntries(IEnumerable<EntityEntry> entries)
    {
        ICollection<EntityAudit> audits = [];

        foreach (var entity in entries)
        {
            if (entity.State is not (EntityState.Modified or EntityState.Added or EntityState.Deleted)) continue;

            var entityAudit = new EntityAudit
            {
                State = entity.State.ToString(),
                Type = entity.Metadata.DisplayName(),
                Message = entity.State is EntityState.Modified ? CreateModifiedMessage(entity) : string.Empty
            };

            var idProp = entity.Properties.SingleOrDefault(p => p.Metadata.Name == "Id");

            if (idProp?.CurrentValue != null)
            {
                entityAudit.EntityId = (Guid) idProp.CurrentValue;
            }

            audits.Add(entityAudit);
        }

        return audits;
    }

    private static string CreateModifiedMessage(EntityEntry entity)
    {
        var message = entity.Properties
            .Where(p => p.IsModified)
            .Aggregate("Updated: { ", (msg, p) => msg + $"'{p.Metadata.Name}': {p.CurrentValue} ") + " }";

        return message;
    }
}
