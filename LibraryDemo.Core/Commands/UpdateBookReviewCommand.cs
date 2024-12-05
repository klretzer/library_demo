namespace LibraryDemo.Core.Commands;

public record UpdateBookReviewCommand : IRequest<bool>
{
    public required Guid Id { get; init; }

    public int? Rating { get; init; }

    public string? Text { get; init; }
}

public class UpdateBookReviewCommandHandler(IRepository<BookReview> repository) : IRequestHandler<UpdateBookReviewCommand, bool>
{
    private readonly IRepository<BookReview> _repository = repository;

    public async Task<bool> Handle(UpdateBookReviewCommand command, CancellationToken cancellationToken)
    {
        if ((await _repository.RetrieveAsync(r => r.Id == command.Id)).SingleOrDefault() is not BookReview current)
        {
            return false;
        }

        var result = await _repository.UpdateAsync(current, command);

        return result > 0;
    }
}