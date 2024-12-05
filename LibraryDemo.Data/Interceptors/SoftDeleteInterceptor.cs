namespace LibraryDemo.Data.Interceptors;

public class SoftDeleteInterceptor : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData data,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (data.Context is null) return new ValueTask<InterceptionResult<int>>(result);

        foreach (var entry in data.Context.ChangeTracker.Entries())
        {
            if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable entity }) continue;

            entry.State = EntityState.Modified;

            entity.IsDeleted = true;
        }

        return new ValueTask<InterceptionResult<int>>(result);
    }
}