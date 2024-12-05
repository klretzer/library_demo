namespace LibraryDemo.Data.Repositories;

public class Repository<T>(LibraryContext context) : IRepository<T> where T : class
{
    private readonly LibraryContext _context = context;

    public async Task<T> CreateAsync(T entity)
    {
        _context.Set<T>().Add(entity);

        await _context.SaveChangesAsync();

        return entity;
    }

    public async Task<int> DeleteAsync(Guid id)
    {
        if (await _context.Set<T>().FindAsync(id) is T entity)
        {
            _context.Set<T>().Remove(entity);

            return await _context.SaveChangesAsync();
        }

        return 0;
    }

    public async Task<IEnumerable<T>> RetrieveAsync(Expression<Func<T, bool>>? condition = default)
    {
        IQueryable<T> query = _context.Set<T>();

        if (condition != default)
        {
            query = query.Where(condition);
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<int> UpdateAsync(T entity, object? updatedValues = default)
    {
        if (updatedValues is not null)
        {
            _context.Attach(entity);

            _context.Entry(entity).CurrentValues.SetValues(updatedValues);
        }
        else
        {
            _context.Set<T>().Update(entity);
        }

        return await _context.SaveChangesAsync();
    }
}