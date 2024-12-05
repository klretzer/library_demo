namespace LibraryDemo.Core.Queries;

public record GetBookQuery(Expression<Func<Book, bool>>? Condition) : IRequest<List<BookDTO>>;

public class GetBookQueryHandler(IRepository<Book> repository, IMapper mapper) : IRequestHandler<GetBookQuery, List<BookDTO>>
{
    private readonly IRepository<Book> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<List<BookDTO>> Handle(GetBookQuery query, CancellationToken cancellationToken)
    {
        var result = await _repository.RetrieveAsync(query.Condition);

        var dtos = result.Select(_mapper.Map<BookDTO>).ToList();

        return dtos;
    }
}