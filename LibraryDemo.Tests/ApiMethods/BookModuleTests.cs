namespace LibraryDemo.Tests.ApiMethods;

public class BookModuleTests
{
    [Fact]
    public async Task CreateAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
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
        var testGuid = Guid.NewGuid();

        mediator.Setup(m => m.Send(It.IsAny<CreateBookCommand>(), It.IsAny<CancellationToken>()).Result).Returns(testGuid);

        var response = await BookModule.CreateAsync(mediator.Object, command);

        Assert.IsType<Created>(response);
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal($"/books/{testGuid}", response.Location);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var testId = Guid.NewGuid();
        var testDtos = new List<BookDTO>
        {
            new()
            {
                Id = testId,
                Author = "TestTwo",
                Category = "TestTwo",
                CoverImageUrl = "TestTwo",
                Description = "TestTwo",
                ISBN = Guid.NewGuid(),
                PageCount = 10,
                PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-10)),
                Publisher = "TestTwo",
                Title = "TestTwo"
            }
        };

        mediator.Setup(m => m.Send(It.IsAny<GetBookQuery>(), It.IsAny<CancellationToken>()).Result).Returns(testDtos);

        var response = await BookModule.GetByIdAsync(mediator.Object, testId);

        Assert.IsType<Ok<BookDTO>>(response.Result);
        Assert.Equal(testId, (response.Result as Ok<BookDTO>)!.Value!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<GetBookQuery>(), It.IsAny<CancellationToken>()).Result).Returns([]);

        var response = await BookModule.GetByIdAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookCommand>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookCommand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookCommand>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }
}