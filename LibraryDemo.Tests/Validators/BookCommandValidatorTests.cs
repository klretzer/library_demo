namespace LibraryDemo.Tests.Validators;

public class BookCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(PassingData))]
    public void Validate_ShouldPass(CreateBookCommand command)
    {
        var result = new CreateBookCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [MemberData(nameof(FailingData))]
    public void Validate_ShouldFail(CreateBookCommand command, string invalidProperty)
    {
        var result = new CreateBookCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(result.Errors[0].PropertyName, invalidProperty);
    }

    public static IEnumerable<object[]> PassingData =>
        [
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = "Test"
                }
            ],
            [
                new CreateBookCommand
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
                }
            ]
        ];

    public static IEnumerable<object[]> FailingData =>
        [
            [
                new CreateBookCommand
                {
                    Author = new StringBuilder("Test", 65).Append('0', 65).ToString(),
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = "Test"
                }, "Author"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = new StringBuilder("Test", 65).Append('0', 65).ToString(),
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = "Test"
                }, "Category"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = new StringBuilder("Test", 65).Append('0', 65).ToString(),
                    Title = "Test"
                }, "Publisher"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = -1,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = "Test"
                }, "PageCount"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = new StringBuilder("Test", 65).Append('0', 65).ToString()
                }, "Title"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = new StringBuilder("Test", 257).Append('0', 253).ToString(),
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-5)),
                    Publisher = "Test",
                    Title = "Test"
                }, "CoverImageUrl"
            ],
            [
                new CreateBookCommand
                {
                    Author = "Test",
                    Category = "Test",
                    CoverImageUrl = "Test",
                    Description = "Test",
                    ISBN = Guid.NewGuid(),
                    PageCount = 10,
                    PublicationDate = DateOnly.FromDateTime(DateTime.Today.Date.AddDays(1)),
                    Publisher = "Test",
                    Title = "Test"
                }, "PublicationDate"
            ],
        ];
}