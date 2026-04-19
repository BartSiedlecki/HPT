using FluentAssertions;
using HPT.SharedKernel.Constants;
using HTP.App.Auth.CreateByAdmin;
using HTP.App.Users.Errors;
using HTP.Domain.Users;
using HTP.Domain.ValueObjects;
using HTP.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace HTP.IntegrationTests.Auth;

public class CreateUserByAdminEndpointsTests : BaseIntegrationTest
{
    public CreateUserByAdminEndpointsTests(IntegrationTestWebAppFactory factory) : base(factory)
    {}

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_ShouldReturnBadRequest_WhenEmailIsEmpty(string email)
    {
        // arrange
        var command = new CreateUserByAdminCommand("jan", "kowalski", Email: email, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ErrorCodes.Email.Validation.Empty.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.Email.Validation.Empty.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenEmailHasInvalidFormat()
    {
        // arrange
        var command = new CreateUserByAdminCommand("jan", "kowalski", Email: "mail@@gmail.com", Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ErrorCodes.Email.Validation.InvalidFormat.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.Email.Validation.InvalidFormat.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenEmailExceedsMaximumLength()
    {
        // arrange
        var tooLongEmail = $"{new string('a', FieldLengths.Email.MaxLength + 1)}@mail.com";
        var command = new CreateUserByAdminCommand("jan", "kowalski", Email: tooLongEmail, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ErrorCodes.Email.Validation.MaximumLengthExceeded.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.Email.Validation.MaximumLengthExceeded.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_ShouldReturnBadRequest_WhenFirstNameIsEmpty(string firstName)
    {
        // arrange
        var command = new CreateUserByAdminCommand(firstName, "kowalski", Email: RandomData.UniqueEmail.Value, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ErrorCodes.FirstName.Validation.Empty.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.FirstName.Validation.Empty.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenFirstNameIsTooShort()
    {
        // arrange
        var command = new CreateUserByAdminCommand("A", "kowalski", Email: RandomData.UniqueEmail.Value, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Title.Should().Be(ErrorCodes.FirstName.Validation.MinLengthNotMet.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.FirstName.Validation.MinLengthNotMet.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenFirstNameHasInvalidFormat()
    {
        // arrange
        var command = new CreateUserByAdminCommand("j@n", "kowalski", Email: RandomData.UniqueEmail.Value, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Title.Should().Be(ErrorCodes.FirstName.Validation.InvalidFormat.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.FirstName.Validation.InvalidFormat.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public async Task Create_ShouldReturnBadRequest_WhenLastNameIsEmpty(string lastName)
    {
        // arrange
        var command = new CreateUserByAdminCommand("jan", lastName, Email: RandomData.UniqueEmail.Value, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.1");
        problemDetails.Status.Should().Be(400);
        problemDetails.Title.Should().Be(ErrorCodes.LastName.Validation.Empty.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.LastName.Validation.Empty.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenLastNameHasInvalidFormat()
    {
        // arrange
        var command = new CreateUserByAdminCommand("jan", "kowalski!", Email: RandomData.UniqueEmail.Value, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var problemDetails = await httpResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Title.Should().Be(ErrorCodes.LastName.Validation.InvalidFormat.Code);
        problemDetails.Detail.Should().Be(ErrorCodes.LastName.Validation.InvalidFormat.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenEmailAlreadyExists()
    {
        // arrange
        var email = RandomData.UniqueEmail.Value;
        var command = new CreateUserByAdminCommand("jan", "kowalski", Email: email, Roles: []);

        // act
        HttpResponseMessage firstResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);
        HttpResponseMessage secondResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.Conflict);

        var problemDetails = await secondResponse.Content.ReadFromJsonAsync<ProblemDetails>(JsonOptions);
        problemDetails.Should().NotBeNull();

        problemDetails!.Type.Should().Be("https://tools.ietf.org/html/rfc7231#section-6.5.8");
        problemDetails.Status.Should().Be(409);
        problemDetails.Title.Should().Be(UserApplicationErrors.EmailAlreadyExists.Code);
        problemDetails.Detail.Should().Be(UserApplicationErrors.EmailAlreadyExists.Message);
    }

    [Fact]
    public async Task Create_ShouldReturnOk_AndSetPasswordUrl_WhenRequestIsValid()
    {
        // arrange
        var email = RandomData.UniqueEmail.Value;
        var command = new CreateUserByAdminCommand("jan", "kowalski", Email: email, Roles: []);

        // act
        HttpResponseMessage httpResponse = await HttpClient.PostAsJsonAsync("api/auth/create-user-by-admin", command);

        // assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var response = await httpResponse.Content.ReadFromJsonAsync<CreateUserByAdminResponse>(JsonOptions);
        response.Should().NotBeNull();
        response!.SetPasswordUrl.Should().NotBeNullOrWhiteSpace();

        var uri = new Uri(response.SetPasswordUrl);
        var query = QueryHelpers.ParseQuery(uri.Query);
        query.Should().ContainKey("token");
        query.Should().ContainKey("userId");

        var userRepository = GetRequiredService<IUserRepository>();
        var userInDb = await userRepository.GetByEmailAsync(Email.Create(email).Value);
        userInDb.Should().NotBeNull();
    }

    [Fact(Skip = "TODO: endpoint is not protected with [Authorize] yet")]
    public async Task Create_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
    }
}
