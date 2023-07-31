using System.Net;
using Contacts.Application.Commands.Users;
using Contacts.Application.Commands.Users.Handlers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Domain.Services;
using Contacts.Tests.Builders;
using Microsoft.AspNetCore.Http;

namespace Contacts.Tests.Tests.Handlers;

public class AddUserHandlerTests
{
    #region Setup
    
    private readonly Mock<IUsersRepository> _usersRepository;
    private readonly AddUserHandler _handler;
    private readonly DefaultHttpContext _httpContext;

    public AddUserHandlerTests()
    {
        _usersRepository = new Mock<IUsersRepository>();
        var passwordService = new Mock<IPasswordService>();
        _handler = new AddUserHandler(_usersRepository.Object, passwordService.Object);
        _httpContext = new DefaultHttpContext();
    }
    
    #endregion
    
    #region Tests

    [Fact]
    public async Task AddUserHandler_WithExistingUserName_ReturnsBadRequest()
    {
        var command = new AddUserRequest("TestUser", "Password123!");

        var existingUser = UserBuilder.Simple().Build();
        _usersRepository.Setup(x => x.GetUserByUserNameAsync(command.UserName)).ReturnsAsync(existingUser);

        await _handler.HandleAsync(command, _httpContext);

        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddUserHandler_WithValidCommand_AddsNewUser()
    {
        var command = new AddUserRequest("TestUser", "Password123!");
        
        _usersRepository.Setup(x => x.GetUserByUserNameAsync(command.UserName)).ReturnsAsync((User)null);

        await _handler.HandleAsync(command, _httpContext);

        _usersRepository.Verify(x => x.AddUserAsync(It.IsAny<User>()), Times.Once);
        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
    }
    
    #endregion
}