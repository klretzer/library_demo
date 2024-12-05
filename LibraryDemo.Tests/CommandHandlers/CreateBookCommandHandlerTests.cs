namespace LibraryDemo.Tests.CommandHandlers;

public class CreateBookCommandHandlerTests
{
    [Fact]
    public async Task HandlerCreateBook_Success()
    {
        var mockRepo = new Mock<IRepository<Book>>();
        var testBook = new Book
        {
            Id = Guid.NewGuid(),
            Author = "TestTwo",
            Category = "TestTwo",
            CoverImageUrl = "TestTwo",
            Description = "TestTwo",
            ISBN = Guid.NewGuid(),
            PageCount = 10,
            PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-10)),
            Publisher = "TestTwo",
            Title = "TestTwo",
            Version = new byte[1]
        };
        var command = new CreateBookCommand
        {
            Author = "TestTwo",
            Category = "TestTwo",
            CoverImageUrl = "TestTwo",
            Description = "TestTwo",
            ISBN = Guid.NewGuid(),
            PageCount = 10,
            PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-10)),
            Publisher = "TestTwo",
            Title = "TestTwo"
        };

        mockRepo.Setup(r => r.CreateAsync(It.IsAny<Book>()).Result).Returns(testBook);

        var handler = new CreateBookCommandHandler(mockRepo.Object, new Mock<IMapper>().Object);
        var result = await handler.Handle(command, CancellationToken.None);

        mockRepo.Verify(r => r.CreateAsync(It.IsAny<Book>()), Times.Once);

        Assert.IsType<Guid>(result);
        Assert.Equal(testBook.Id, result);
    }
}