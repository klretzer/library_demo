namespace LibraryDemo.Core.Commands;

public record DeleteBookRentalCommmand(Guid Id) : IRequest<bool>;

public class DeleteBookRentalCommmandHandler(IRepository<BookRental> repository) : IRequestHandler<DeleteBookRentalCommmand, bool>
{
    private readonly IRepository<BookRental> _repository = repository;

    public async Task<bool> Handle(DeleteBookRentalCommmand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(command.Id);

        return result > 0;
    }
}