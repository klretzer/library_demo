namespace LibraryDemo.Tests.CommandHandlers;

public class DeleteBookCommandHandlerTests
{
    [Fact]
    public async Task HandlerDeleteBook_Success()
    {
        var mockRepo = new Mock<IRepository<Book>>();
        var command = new DeleteBookCommmand(Guid.NewGuid());

        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()).Result).Returns(1);

        var handler = new DeleteBookCommmandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }
}