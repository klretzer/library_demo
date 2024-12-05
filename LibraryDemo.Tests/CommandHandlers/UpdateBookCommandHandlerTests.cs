namespace LibraryDemo.Tests.CommandHandlers;

public class UpdateBookCommandHandlerTests
{
    [Theory]
    [MemberData(nameof(TheoryData))]
    public async Task HandlerUpdateBook_Success(UpdateBookCommand command, IEnumerable<Book> currentBooks, bool expected)
    {
        var mockRepo = new Mock<IRepository<Book>>();

        mockRepo.Setup(r => r.RetrieveAsync(It.IsAny<Expression<Func<Book, bool>>>()).Result).Returns(currentBooks);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Book>(), command).Result).Returns(1);

        var handler = new UpdateBookCommandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        var numUpdateCalls = expected ? 1 : 0;

        mockRepo.Verify(r => r.RetrieveAsync(It.IsAny<Expression<Func<Book, bool>>>()), Times.Once);
        mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Book>(), command), Times.Exactly(numUpdateCalls));

        Assert.IsType<bool>(result);
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> TheoryData =>
        [
            [
                new UpdateBookCommand
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
                    Title = "TestTwo"
                },
                new Book[]
                {
                    new()
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
                    }
                },
                true
            ],
            [
                new UpdateBookCommand
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
                    Title = "TestTwo"
                },
                Array.Empty<Book>(),
                false
            ]
        ];
}