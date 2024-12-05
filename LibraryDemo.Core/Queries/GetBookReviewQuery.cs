namespace LibraryDemo.Core.Queries;

public record GetBookReviewQuery(Expression<Func<BookReview, bool>>? Condition) : IRequest<List<BookReviewDTO>>;

public class GetBookReviewQueryHandler(IRepository<BookReview> repository, IMapper mapper) : IRequestHandler<GetBookReviewQuery, List<BookReviewDTO>>
{
    private readonly IRepository<BookReview> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<List<BookReviewDTO>> Handle(GetBookReviewQuery query, CancellationToken cancellationToken)
    {
        var result = await _repository.RetrieveAsync(query.Condition);

        var dtos = result.Select(_mapper.Map<BookReviewDTO>).ToList();

        return dtos;
    }
}