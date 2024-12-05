namespace LibraryDemo.Core.Commands;

public record UpdateBookRentalCommand : IRequest<bool>
{
    public required Guid Id { get; init; }

    public required DateTime DueDate { get; init; }
}

public class UpdateBookRentalCommandHandler(IRepository<BookRental> repository) : IRequestHandler<UpdateBookRentalCommand, bool>
{
    private readonly IRepository<BookRental> _repository = repository;

    public async Task<bool> Handle(UpdateBookRentalCommand command, CancellationToken cancellationToken)
    {
        if ((await _repository.RetrieveAsync(r => r.Id == command.Id)).SingleOrDefault() is not BookRental current)
        {
            return false;
        }

        var result = await _repository.UpdateAsync(current, command);

        return result > 0;
    }
}