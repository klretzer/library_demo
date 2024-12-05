namespace LibraryDemo.Core.Commands;

public record CreateBookCommand : IRequest<Guid>
{
    public required string Title { get; init; }

    public required string Author { get; init; }

    public required string Description { get; init; }

    public required string Category { get; init; }

    public required string Publisher { get; init; }

    public required string CoverImageUrl { get; init; }

    public required int PageCount { get; init; }

    public required Guid ISBN { get; init; }

    public required DateOnly PublicationDate { get; init; }
}

public class CreateBookCommandHandler(IRepository<Book> repository, IMapper mapper) : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IRepository<Book> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateBookCommand command, CancellationToken cancellationToken)
    {
        var newBook = _mapper.Map<Book>(command);

        var result = await _repository.CreateAsync(newBook);

        return result.Id;
    }
}