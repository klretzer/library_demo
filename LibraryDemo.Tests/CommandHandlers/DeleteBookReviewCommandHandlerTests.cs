namespace LibraryDemo.Tests.CommandHandlers;

public class DeleteBookReviewCommandHandlerTests
{
    [Fact]
    public async Task HandlerDeleteBookReview_Success()
    {
        var mockRepo = new Mock<IRepository<BookReview>>();
        var command = new DeleteBookReviewCommmand(Guid.NewGuid());

        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()).Result).Returns(1);

        var handler = new DeleteBookReviewCommmandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }
}