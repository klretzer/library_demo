namespace LibraryDemo.Tests.Validators;

public class RegistrationRequestCommandValidatorTests
{
    [Theory]
    [MemberData(nameof(PassingData))]
    public void Validate_ShouldPass(RegisterUserCommand command)
    {
        var result = new RegisterUserCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [MemberData(nameof(FailingData))]
    public void Validate_ShouldFail(RegisterUserCommand command, string invalidProperty)
    {
        var result = new RegisterUserCommandValidator().Validate(command);

        Assert.NotNull(result);
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
        Assert.Equal(result.Errors[0].PropertyName, invalidProperty);
    }

    public static IEnumerable<object[]> PassingData =>
        [
            [
                new RegisterUserCommand
                {
                    UserName = "test",
                    Email = "test@test.com",
                    Password = "testpassword",
                    Role = "Admin"
                }
            ]
        ];

    public static IEnumerable<object[]> FailingData =>
        [
            [
                new RegisterUserCommand
                {
                    UserName = "test",
                    Email = "test",
                    Password = "testpassword",
                    Role = "Admin"
                }, "Email"
            ],
            [
                new RegisterUserCommand
                {
                    UserName = "test",
                    Email = new StringBuilder("test@test.com", 257).Append('0', 244).ToString(),
                    Password = "testpassword",
                    Role = "Admin"
                }, "Email"
            ],
            [
                new RegisterUserCommand
                {
                    UserName = "test",
                    Email = "test@test.com",
                    Password = "testpassword",
                    Role = new StringBuilder("Admin", 257).Append('0', 252).ToString()
                }, "Role"
            ]
        ];
}