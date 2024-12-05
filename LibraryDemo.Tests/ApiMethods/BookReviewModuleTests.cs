namespace LibraryDemo.Tests.ApiMethods;

public class BookReviewModuleTests
{
    [Fact]
    public async Task CreateAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var command = new CreateBookReviewCommand
        {
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Rating = 3
        };
        var testGuid = Guid.NewGuid();

        mediator.Setup(m => m.Send(It.IsAny<CreateBookReviewCommand>(), It.IsAny<CancellationToken>()).Result).Returns(testGuid);

        var response = await BookReviewModule.CreateAsync(mediator.Object, command);

        Assert.IsType<Created>(response);
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal($"/reviews/{testGuid}", response.Location);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var testId = Guid.NewGuid();
        var testDtos = new List<BookReviewDTO>
        {
            new()
            {
                Id = testId,
                BookId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Rating = 3
            }
        };

        mediator.Setup(m => m.Send(It.IsAny<GetBookReviewQuery>(), It.IsAny<CancellationToken>()).Result).Returns(testDtos);

        var response = await BookReviewModule.GetByIdAsync(mediator.Object, testId);

        Assert.IsType<Ok<BookReviewDTO>>(response.Result);
        Assert.Equal(testId, (response.Result as Ok<BookReviewDTO>)!.Value!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<GetBookReviewQuery>(), It.IsAny<CancellationToken>()).Result).Returns([]);

        var response = await BookReviewModule.GetByIdAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookReviewCommand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookReviewModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookReviewCommand>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookReviewCommand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookReviewModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookReviewCommand>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookReviewCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookReviewModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookReviewCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookReviewModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }
}