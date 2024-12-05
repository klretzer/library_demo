namespace LibraryDemo.Core.Commands;

public record CreateBookRentalCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }

    public required Guid BookId { get; init; }

    public required DateTime DueDate { get; init; }
}

public class CreateBookRentalCommandHandler(IRepository<BookRental> repository, IMapper mapper) : IRequestHandler<CreateBookRentalCommand, Guid>
{
    private readonly IRepository<BookRental> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateBookRentalCommand command, CancellationToken cancellationToken)
    {
        var newRental = _mapper.Map<BookRental>(command);

        var result = await _repository.CreateAsync(newRental);

        return result.Id;
    }
}