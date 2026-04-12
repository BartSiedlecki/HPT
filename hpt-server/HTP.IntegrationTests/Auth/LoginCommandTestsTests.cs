using HTP.App.Auth.Login;

namespace HTP.IntegrationTests.Auth;

public class LoginCommandTestsTests : BaseIntegrationTest
{
    public LoginCommandTestsTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Theory]
    [InlineData("Jan", "123daskmd213")]
    public async Task Login_ShouldFail_ForInvalidLoginOrPassword(string login, string password)
    {
        // arrange
        var command = new LoginUserCommand(login, password);

        // act
        var result = await Dispatcher.Send(command);

        // assert
        Assert.NotNull(result);
    }
}
