using AmpApp.Features.Todo;
using AmpApp.Shared.Models.Todo;

using Xunit; // xUnit attributes and assertions
using Moq; // For mocking dependencies
using FluentAssertions; // For readable assertions
using System.Security.Claims; // For simulating user identity
using Microsoft.AspNetCore.Http;
using Xunit.Abstractions; // For HttpContextAccessor

namespace AmpApp.Tests;

public class CreateServiceTests(ITestOutputHelper output)
{
    [Fact] // This marks the method as a test
    public async Task HandleAsync_SetsCreatedByAndCallsRepo()
    {
        // Arrange: set up everything the test needs
        // (1) Mock the repository so it doesn't hit the database
        var repoMock = new Mock<CreateRepository>(null!); // null! because constructor needs a factory
        repoMock
            .Setup(r => r.CreateAsync(It.IsAny<TodoEntity>()))
            .Callback<TodoEntity>(e => output.WriteLine($"CreatedBy: {e.CreatedBy}, Title: {e.Title}"))
            .ReturnsAsync(Guid.NewGuid()); // Whenever CreateAsync is called, return a new Guid

        // (2) Set up a fake user (simulate someone logged in)
        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(
                new ClaimsIdentity(
                    new[]
                    {
                        new Claim(ClaimTypes.Name, "testuser"),
                        new Claim(ClaimTypes.Email, "testuser@example.com")
                    }, authenticationType: "TestAuthType"
                )
            )
        };
        var contextAccessor = new Mock<IHttpContextAccessor>();
        contextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // (3) Create the service under test
        var service = new CreateService(repoMock.Object, contextAccessor.Object);

        // (4) Create a sample DTO to pass in
        var dto = new CreateTodoDto { Title = "My todo", Description = "desc" };

        // Act: run the code we want to test
        var result = await service.HandleAsync(dto);

        // Assert: check the results
        result.Should().NotBeNull();
        repoMock.Verify(r => r.CreateAsync(It.Is<TodoEntity>(
            e => e.CreatedBy == "testuser" && e.Title == "My todo")), Times.Once);
    }
}