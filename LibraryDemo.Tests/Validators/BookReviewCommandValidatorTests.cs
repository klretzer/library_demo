namespace LibraryDemo.Tests.Validators;

public class BookReviewCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(PassingData))]
    public void Validate_ShouldPass(CreateBookReviewCommand command)
    {
        var result = new CreateBookReviewCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [MemberData(nameof(FailingData))]
    public void Validate_ShouldFail(CreateBookReviewCommand command, string invalidProperty)
    {
        var result = new CreateBookReviewCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(result.Errors[0].PropertyName, invalidProperty);
    }

    public static IEnumerable<object[]> PassingData =>
        [
            [
                new CreateBookReviewCommand
                {
                    BookId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Rating = 1,
                    Text = "Test"
                }
            ]
        ];

    public static IEnumerable<object[]> FailingData =>
        [
            [
                new CreateBookReviewCommand
                {
                    BookId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Rating = 7,
                    Text = "Test"
                }, "Rating"
            ],
            [
                new CreateBookReviewCommand
                {
                    BookId = default,
                    UserId = Guid.NewGuid(),
                    Rating = 3,
                    Text = "Test"
                }, "BookId"
            ],
            [
                new CreateBookReviewCommand
                {
                    BookId = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    Rating = 3,
                    Text = new StringBuilder("Test", 513).Append('0', 509).ToString()
                }, "Text"
            ]
        ];
}