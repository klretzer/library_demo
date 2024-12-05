namespace LibraryDemo.Core.Commands;

public record CreateBookReviewCommand : IRequest<Guid>
{
    public required Guid UserId { get; init; }

    public required Guid BookId { get; init; }

    public required int Rating { get; init; }

    public string? Text { get; set; }
}

public class CreateBookReviewCommandHandler(IRepository<BookReview> repository, IMapper mapper) : IRequestHandler<CreateBookReviewCommand, Guid>
{
    private readonly IRepository<BookReview> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateBookReviewCommand command, CancellationToken cancellationToken)
    {
        var newReview = _mapper.Map<BookReview>(command);

        var result = await _repository.CreateAsync(newReview);

        return result.Id;
    }
}