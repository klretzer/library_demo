namespace LibraryDemo.Tests.ApiMethods;

public class BookRentalModuleTests
{
    [Fact]
    public async Task CreateAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var command = new CreateBookRentalCommand
        {
            BookId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            DueDate = DateTime.Now
        };
        var testGuid = Guid.NewGuid();

        mediator.Setup(m => m.Send(It.IsAny<CreateBookRentalCommand>(), It.IsAny<CancellationToken>()).Result).Returns(testGuid);

        var response = await BookRentalModule.CreateAsync(mediator.Object, command);

        Assert.IsType<Created>(response);
        Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        Assert.Equal($"/rentals/{testGuid}", response.Location);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOk()
    {
        var mediator = new Mock<IMediator>();
        var testId = Guid.NewGuid();
        var testDtos = new List<BookRentalDTO>
        {
            new()
            {
                Id = testId,
                BookId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                DueDate= DateTime.Now
            }
        };

        mediator.Setup(m => m.Send(It.IsAny<GetBookRentalQuery>(), It.IsAny<CancellationToken>()).Result).Returns(testDtos);

        var response = await BookRentalModule.GetByIdAsync(mediator.Object, testId);

        Assert.IsType<Ok<BookRentalDTO>>(response.Result);
        Assert.Equal(testId, (response.Result as Ok<BookRentalDTO>)!.Value!.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<GetBookRentalQuery>(), It.IsAny<CancellationToken>()).Result).Returns([]);

        var response = await BookRentalModule.GetByIdAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookRentalCommand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookRentalModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookRentalCommand>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<UpdateBookRentalCommand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookRentalModule.UpdateAsync(mediator.Object, It.IsAny<UpdateBookRentalCommand>());

        Assert.IsType<NotFound>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNoContent()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookRentalCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(true);

        var response = await BookRentalModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NoContent>(response.Result);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsNotFound()
    {
        var mediator = new Mock<IMediator>();

        mediator.Setup(m => m.Send(It.IsAny<DeleteBookRentalCommmand>(), It.IsAny<CancellationToken>()).Result).Returns(false);

        var response = await BookRentalModule.DeleteAsync(mediator.Object, It.IsAny<Guid>());

        Assert.IsType<NotFound>(response.Result);
    }
}