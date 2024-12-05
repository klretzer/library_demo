namespace LibraryDemo.Tests.CommandHandlers;

public class UpdateBookRentalCommandHandlerTests
{
    [Theory]
    [MemberData(nameof(TheoryData))]
    public async Task HandlerUpdateBookRental_Success(UpdateBookRentalCommand command, IEnumerable<BookRental> currentRentals, bool expected)
    {
        var mockRepo = new Mock<IRepository<BookRental>>();

        mockRepo.Setup(r => r.RetrieveAsync(It.IsAny<Expression<Func<BookRental, bool>>>()).Result).Returns(currentRentals);
        mockRepo.Setup(r => r.UpdateAsync(It.IsAny<BookRental>(), command).Result).Returns(1);

        var handler = new UpdateBookRentalCommandHandler(mockRepo.Object);
        var result = await handler.Handle(command, CancellationToken.None);
        var numUpdateCalls = expected ? 1 : 0;

        mockRepo.Verify(r => r.RetrieveAsync(It.IsAny<Expression<Func<BookRental, bool>>>()), Times.Once);
        mockRepo.Verify(r => r.UpdateAsync(It.IsAny<BookRental>(), command), Times.Exactly(numUpdateCalls));

        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> TheoryData =>
        [
            [
                new UpdateBookRentalCommand
                {
                    Id = Guid.NewGuid(),
                    DueDate = DateTime.Now
                },
                new BookRental[]
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        BookId = Guid.NewGuid(),
                        UserId = Guid.NewGuid(),
                        User = new Mock<User>().Object,
                        Book = new Mock<Book>().Object,
                        DueDate = DateTime.Now,
                        Version = new byte[1]
                    }
                },
                true
            ],
            [
                new UpdateBookRentalCommand
                {
                    Id = Guid.NewGuid(),
                    DueDate = DateTime.Now
                },
                Array.Empty<BookRental>(),
                false
            ]
        ];
}