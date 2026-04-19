using FluentAssertions;
using HTP.App.Abstractions.Repositories.Read;
using HTP.App.Auth.CreateByAdmin;
using HTP.App.Users.Errors;
using HTP.Domain.Users;
using HTP.Domain.ValueObjects;
using HTP.Infrastructure.Identity;
using HTP.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace HTP.IntegrationTests.Auth;

public class CreateUserByAdminCommandTests : BaseIntegrationTest
{

    public CreateUserByAdminCommandTests(IntegrationTestWebAppFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Create_ShouldSuccess_WhenCredentialsAreValid() {
        // arrange
        CreateUserByAdminCommand command = CreateValidCommand();

        // act
        var result = await Dispatcher.Send(command);
        
        // assert
        result.IsSuccess.Should().Be(true);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenEmailAlreadyExists()
    {
        // arrange
        var email = RandomData.UniqueEmail;
        CreateUserByAdminCommand command = CreateValidCommand(email);
        CreateUserByAdminCommand duplicatedEmailCommand = CreateValidCommand(email);

        // act
        var result = await Dispatcher.Send(command);
        var secondResult = await Dispatcher.Send(duplicatedEmailCommand);

        // assert
        result.IsSuccess.Should().Be(true);
        secondResult.IsSuccess.Should().Be(false);
        secondResult.Error.Should().Be(UserApplicationErrors.EmailAlreadyExists);
    }

    [Fact]
    public async Task Create_ShouldReturnURL()
    {
        // arrange
        CreateUserByAdminCommand command = CreateValidCommand();

        // act
        var result = await Dispatcher.Send(command);

        // assert
        var uri = new Uri(result.Value.SetPasswordUrl);
        var query = QueryHelpers.ParseQuery(uri.Query);


        result.Value.SetPasswordUrl.Should().NotBeNullOrWhiteSpace();
        result.IsSuccess.Should().Be(true);
        query.Should().ContainKey("token");
        query.Should().ContainKey("userId");
    }

    [Fact]
    public async Task Create_ShouldFail_WhenValidationFails()
    {
        // arrange
        var command = new CreateUserByAdminCommand("j@n", "kowalski!", "invalid-email", []);

        // act
        var result = await Dispatcher.Send(command);

        // assert
        result.IsFailure.Should().Be(true);
    }

    [Fact]
    public async Task Create_ShouldPersistDomainUser_InDatabase()
    {
        // arrange
        var email = RandomData.UniqueEmail;
        CreateUserByAdminCommand command = CreateValidCommand(email);

        // act
        var result = await Dispatcher.Send(command);

        // assert
        result.IsSuccess.Should().Be(true);

        var userRepository = GetRequiredService<IUserRepository>();

        var emailVo = Email.Create(email).Value;
        var userInDb = await userRepository.GetByEmailAsync(emailVo);

        userInDb.Should().NotBeNull();
        userInDb!.FirstName.Value.Should().Be("Jan");
        userInDb.LastName.Value.Should().Be("Kowalski");
        userInDb.Email.Should().Be(emailVo);
    }

    [Fact]
    public async Task Create_ShouldPersistIdetityUser_InDatabase()
    {
        // arrange
        var email = RandomData.UniqueEmail;
        CreateUserByAdminCommand command = CreateValidCommand(email);

        // act
        var result = await Dispatcher.Send(command);

        // assert
        result.IsSuccess.Should().Be(true);
        
        var userManager = GetRequiredService<UserManager<AppIdentityUser>>();
        var identityUser = await userManager.FindByEmailAsync(email);
        identityUser.Should().NotBeNull();
        identityUser!.Email.Should().Be(email);
    }

    [Fact]
    public async Task Create_ShouldSetEmailAsUnconfirmed_WhenUserIsCreated()
    {
        // arrange
        var email = RandomData.UniqueEmail;
        CreateUserByAdminCommand command = CreateValidCommand(email);

        // act
        var result = await Dispatcher.Send(command);
        
        // assert
        result.IsSuccess.Should().Be(true);

        var userManager = GetRequiredService<UserManager<AppIdentityUser>>();
        var identityUser = await userManager.FindByEmailAsync(email);
        identityUser.Should().NotBeNull();
        identityUser.EmailConfirmed.Should().Be(false);
    }

    [Fact]
    public async Task Create_ShouldSetRoles_WhenUserIsCreated()
    {
        // arrange
        var email = RandomData.UniqueEmail;
        var commmand = new CreateUserByAdminCommand("jan", "kowalski", email, ["moderator", "subscriber"]);

        // act
        var result = await Dispatcher.Send(commmand);

        // assert
        result.IsSuccess.Should().Be(true);

        var userRoleRepository = GetRequiredService<IUserRoleRepository>();
        var emailVo = Email.Create(email).Value;
        var userRepository = GetRequiredService<IUserRepository>();
        var userInDb = await userRepository.GetByEmailAsync(emailVo);

        userInDb.Should().NotBeNull();
        var roles = await userRoleRepository.GetUserRolesAsync(userInDb.Id);
        roles.Should().NotBeNull();
        roles.Should().HaveCount(2);
        roles.Should().Contain("moderator");
        roles.Should().Contain("subscriber");

    }

    private CreateUserByAdminCommand CreateValidCommand(string? email = null)
    {
        return new CreateUserByAdminCommand(
            "jan",
            "kowalski",
            email ?? RandomData.UniqueEmail,
            []
            );
    }
}
