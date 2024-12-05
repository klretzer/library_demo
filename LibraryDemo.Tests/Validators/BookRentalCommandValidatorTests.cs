namespace LibraryDemo.Tests.Validators;

public class BookRentalCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(PassingData))]
    public void Validate_ShouldPass(CreateBookRentalCommand command)
    {
        var result = new CreateBookRentalCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [MemberData(nameof(FailingData))]
    public void Validate_ShouldFail(CreateBookRentalCommand command, string invalidProperty)
    {
        var result = new CreateBookRentalCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(result.Errors[0].PropertyName, invalidProperty);
    }

    public static IEnumerable<object[]> PassingData =>
        [
            [
                new CreateBookRentalCommand
                {
                    BookId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    DueDate = DateTime.Now
                }
            ]
        ];

    public static IEnumerable<object[]> FailingData =>
        [
            [
                new CreateBookRentalCommand
                {
                    BookId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    DueDate = default
                }, "DueDate"
            ],
            [
                new CreateBookRentalCommand
                {
                    BookId = default,
                    UserId = Guid.NewGuid(),
                    DueDate = DateTime.Now
                }, "BookId"
            ]
        ];
}