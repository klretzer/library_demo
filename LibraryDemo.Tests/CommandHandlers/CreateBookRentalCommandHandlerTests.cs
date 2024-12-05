namespace LibraryDemo.Tests.CommandHandlers;

public class CreateBookRentalCommandHandlerTests
{
    [Fact]
    public async Task HandlerCreateBookRental_Success()
    {
        var mockRepo = new Mock<IRepository<BookRental>>();
        var mockUser = new Mock<User>();
        var mockBook = new Mock<Book>();
        var testBookRental = new BookRental
        {
            Id = Guid.NewGuid(),
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            User = mockUser.Object,
            Book = mockBook.Object,
            DueDate = DateTime.Now,
            Version = new byte[1]
        };
        var command = new CreateBookRentalCommand
        {
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            DueDate = DateTime.Now
        };

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<BookRental>()).Result).Returns(testBookRental);

        var handler = new CreateBookRentalCommandHandler(mockRepo.Object, new Mock<IMapper>().Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.CreateAsync(It.IsAny<BookRental>()), Times.Once);

        Assert.IsType<Guid>(result);
        Assert.Equal(testBookRental.Id, result);
    }
}