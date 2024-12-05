namespace LibraryDemo.Tests.CommandHandlers;

public class CreateBookReviewCommandHandlerTests
{
    [Fact]
    public async Task HandlerCreateBookReview_Success()
    {
        var mockRepo = new Mock<IRepository<BookReview>>();
        var mockUser = new Mock<User>();
        var mockBook = new Mock<Book>();
        var testBookReview = new BookReview
        {
            Id = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            User = mockUser.Object,
            Book = mockBook.Object,
            Text = string.Empty,
            Rating = 3,
            Version = new byte[1]
        };
        var command = new CreateBookReviewCommand
        {
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Rating = 3,
            Text = string.Empty
        };

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<BookReview>()).Result).Returns(testBookReview);

        var handler = new CreateBookReviewCommandHandler(mockRepo.Object, new Mock<IMapper>().Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.CreateAsync(It.IsAny<BookReview>()), Times.Once);

        Assert.IsType<Guid>(result);
        Assert.Equal(testBookReview.Id, result);
    }
}