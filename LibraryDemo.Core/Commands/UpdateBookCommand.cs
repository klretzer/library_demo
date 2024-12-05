namespace LibraryDemo.Core.Commands;

public record UpdateBookCommand : IRequest<bool>
{
    public required Guid Id { get; set; }

    public string? Title { get; init; }

    public string? Author { get; init; }

    public string? Description { get; init; }

    public string? Category { get; init; }

    public string? Publisher { get; init; }

    public string? CoverImageUrl { get; init; }

    public int PageCount { get; init; }

    public Guid ISBN { get; init; }

    public DateOnly PublicationDate { get; init; }
}

public class UpdateBookCommandHandler(IRepository<Book> repository) : IRequestHandler<UpdateBookCommand, bool>
{
    private readonly IRepository<Book> _repository = repository;

    public async Task<bool> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
    {
        if ((await _repository.RetrieveAsync(r => r.Id == command.Id)).SingleOrDefault() is not Book current)
        {
            return false;
        }

        var result = await _repository.UpdateAsync(current, command);

        return result > 0;
    }
}