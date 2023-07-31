using System.Net;
using Contacts.Application.Commands.Categories;
using Contacts.Application.Commands.Categories.Handlers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Tests.Builders;
using Microsoft.AspNetCore.Http;

namespace Contacts.Tests.Tests.Handlers;

public class AddCategoryHandlerTests
{
    #region Setup
    
    private readonly Mock<ICategoryRepository> _categoryRepository;
    private readonly AddCategoryHandler _handler;
    private readonly DefaultHttpContext _httpContext;

    public AddCategoryHandlerTests()
    {
        _categoryRepository = new Mock<ICategoryRepository>();
        _handler = new AddCategoryHandler(_categoryRepository.Object);
        _httpContext = new DefaultHttpContext();
    }
    
    #endregion
    
    #region Tests

    [Fact]
    public async Task AddCategoryHandler_WithExistingCategoryName_ReturnsBadRequest()
    {
        var command = new AddCategoryRequest("TestCategory");

        var existingCategory = CategoryBuilder.Simple().Build();
        _categoryRepository.Setup(x => x.GetCategoryByNameAsync(command.Name)).ReturnsAsync(existingCategory);

        await _handler.HandleAsync(command, _httpContext);

        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task AddCategoryHandler_WithValidCommand_AddsNewCategory()
    {
        var command = new AddCategoryRequest("TestCategory");
        
        _categoryRepository.Setup(x => x.GetCategoryByNameAsync(command.Name)).ReturnsAsync((Category)null);

        await _handler.HandleAsync(command, _httpContext);

        _categoryRepository.Verify(x => x.AddCategoryAsync(It.IsAny<Category>()), Times.Once);
        _httpContext.Response.StatusCode.ShouldBe((int)HttpStatusCode.Created);
    }
    
    #endregion
}