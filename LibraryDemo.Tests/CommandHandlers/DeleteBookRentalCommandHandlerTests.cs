namespace LibraryDemo.Tests.CommandHandlers;

public class DeleteBookRentalCommandHandlerTests
{
    [Fact]
    public async Task HandlerDeleteBookRental_Success()
    {
        var mockRepo = new Mock<IRepository<BookRental>>();
        var command = new DeleteBookRentalCommmand(Guid.NewGuid());

        mockRepo.Setup(r => r.DeleteAsync(It.IsAny<Guid>()).Result).Returns(1);

        var handler = new DeleteBookRentalCommmandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Once);

        Assert.IsType<bool>(result);
        Assert.True(result);
    }
}