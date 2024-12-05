namespace LibraryDemo.Core.Commands;

public record DeleteBookCommmand(Guid Id) : IRequest<bool>;

public class DeleteBookCommmandHandler(IRepository<Book> repository) : IRequestHandler<DeleteBookCommmand, bool>
{
    private readonly IRepository<Book> _repository = repository;

    public async Task<bool> Handle(DeleteBookCommmand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(command.Id);

        return result > 0;
    }
}