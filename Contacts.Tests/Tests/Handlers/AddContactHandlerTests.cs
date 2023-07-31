using System.Net;
using Contacts.Application.Commands.Contacts;
using Contacts.Application.Commands.Contacts.Handlers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Tests.Builders;
using Microsoft.AspNetCore.Http;

namespace Contacts.Tests.Tests.Handlers;

public class AddContactHandlerTests
{
    #region Setup
    
    private readonly Mock<IContactsRepository> _contactsRepository;
    private readonly Mock<ICategoryRepository> _categoryRepository;
    private readonly AddContactHandler _handler;
    private readonly DefaultHttpContext _httpContext;

    public AddContactHandlerTests()
    {
        _contactsRepository = new Mock<IContactsRepository>();
        _categoryRepository = new Mock<ICategoryRepository>();
        _handler = new AddContactHandler(_contactsRepository.Object, _categoryRepository.Object);
        _httpContext = new DefaultHttpContext();
    }
    
    #endregion
    
    #region Tests

    [Fact]
    public async Task HandleAsync_WithInvalidCategoryId_ReturnsBadRequest()
    {
        var command = new AddContactRequest("Bob", "Smith", "bob@smith.com", "1234567890", new DateTime(2000, 1, 1), Guid.NewGuid());

        _categoryRepository.Setup(x => x.GetCategoryByIdAsync(command.CategoryId)).ReturnsAsync((Category)null!);

        await _handler.HandleAsync(command, _httpContext);

        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_WithExistingEmail_ReturnsBadRequest()
    {
        var command = new AddContactRequest("Bob", "Smith", "bob@smith.com", "1234567890", new DateTime(2000, 1, 1), Guid.NewGuid());

        var existingContact = ContactBuilder.Simple().Build();
        var existingCategory = CategoryBuilder.Simple().Build();
        _contactsRepository.Setup(x => x.GetContactByEmailAsync(command.Email!)).ReturnsAsync(existingContact);
        _categoryRepository.Setup(x => x.GetCategoryByIdAsync(command.CategoryId)).ReturnsAsync(existingCategory);  

        await _handler.HandleAsync(command, _httpContext);

        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task HandleAsync_WithValidCommand_AddsNewContact()
    {
        var command = new AddContactRequest("Bob", "Smith", "bob@smith.com", "1234567890", new DateTime(2000, 1, 1), Guid.NewGuid());

        var existingCategory = CategoryBuilder.Simple().Build();
        _contactsRepository.Setup(x => x.GetContactByEmailAsync(command.Email!)).ReturnsAsync((Contact)null!);
        _categoryRepository.Setup(x => x.GetCategoryByIdAsync(command.CategoryId)).ReturnsAsync(existingCategory);  

        await _handler.HandleAsync(command, _httpContext);

        _contactsRepository.Verify(x => x.AddContactAsync(It.IsAny<Contact>()), Times.Once);
        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
    }
    
    #endregion
}