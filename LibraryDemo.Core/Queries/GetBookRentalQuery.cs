namespace LibraryDemo.Core.Queries;

public record GetBookRentalQuery(Expression<Func<BookRental, bool>>? Condition) : IRequest<List<BookRentalDTO>>;

public class GetBookRentalQueryHandler(IRepository<BookRental> repository, IMapper mapper) : IRequestHandler<GetBookRentalQuery, List<BookRentalDTO>>
{
    private readonly IRepository<BookRental> _repository = repository;

    private readonly IMapper _mapper = mapper;

    public async Task<List<BookRentalDTO>> Handle(GetBookRentalQuery query, CancellationToken cancellationToken)
    {
        var result = await _repository.RetrieveAsync(query.Condition);

        var dtos = result.Select(_mapper.Map<BookRentalDTO>).ToList();

        return dtos;
    }
}