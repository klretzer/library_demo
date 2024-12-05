namespace LibraryDemo.Core.Commands;

public record DeleteBookReviewCommmand(Guid Id) : IRequest<bool>;

public class DeleteBookReviewCommmandHandler(IRepository<BookReview> repository) : IRequestHandler<DeleteBookReviewCommmand, bool>
{
    private readonly IRepository<BookReview> _repository = repository;

    public async Task<bool> Handle(DeleteBookReviewCommmand command, CancellationToken cancellationToken)
    {
        var result = await _repository.DeleteAsync(command.Id);

        return result > 0;
    }
}