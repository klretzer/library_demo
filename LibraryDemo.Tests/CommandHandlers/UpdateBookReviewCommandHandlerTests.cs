namespace LibraryDemo.Tests.CommandHandlers;

public class UpdateBookReviewCommandHandlerTests
{
    [Theory]
    [MemberData(nameof(TheoryData))]
    public async Task HandlerUpdateBookReview_Found_Success(UpdateBookReviewCommand command, IEnumerable<BookReview> currentReviews, bool expected)
    {
        var mockRepo = new Mock<IRepository<BookReview>>();

        mockRepo.Setup(r => r.RetrieveAsync(It.IsAny<Expression<Func<BookReview, bool>>>()).Result).Returns(currentReviews);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<BookReview>(), It.IsAny<object>()).Result).Returns(1);

        var handler = new UpdateBookReviewCommandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        var numUpdateCalls = expected ? 1 : 0;

        mockRepo.Verify(r => r.RetrieveAsync(It.IsAny<Expression<Func<BookReview, bool>>>()), Times.Once);
        mockRepo.Verify(r => r.UpdateAsync(It.IsAny<BookReview>(), command), Times.Exactly(numUpdateCalls));

        Assert.IsType<bool>(result);
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> TheoryData =>
        [
            [
                new UpdateBookReviewCommand
                {
                    Id = Guid.NewGuid(),
                    Rating = 1
                },
                new BookReview[]
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        BookId = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        User = new Mock<User>().Object,
                        Book = new Mock<Book>().Object,
                        Text = string.Empty,
                        Rating = 3,
                        Version = new byte[1]
                    }
                },
                true
            ],
            [
                new UpdateBookReviewCommand
                {
                    Id = Guid.NewGuid(),
                    Rating = 1
                },
                Array.Empty<BookReview>(),
                false
            ]
        ];
}