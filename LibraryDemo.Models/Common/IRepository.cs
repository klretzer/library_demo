namespace LibraryDemo.Models.Common;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> RetrieveAsync(Expression<Func<T, bool>>? condition = default);

    Task<T> CreateAsync(T entity);

    Task<int> DeleteAsync(Guid id);

    Task<int> UpdateAsync(T entity, object? updatedValues = default);
}