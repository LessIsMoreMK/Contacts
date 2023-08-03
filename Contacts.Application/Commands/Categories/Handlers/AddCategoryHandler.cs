using System.Net;
using Contacts.Application.Helpers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Contacts.Application.Commands.Categories.Handlers;

public class AddCategoryHandler : ICommandHandler<AddCategoryRequest>
{
    #region Fileds
    
    private readonly ICategoryRepository _categoryRepository;
    
    #endregion
    
    #region Constructor

    public AddCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    #endregion
    
    #region Methods
    
    public async Task HandleAsync(AddCategoryRequest command, HttpContext context)
    {
        if (await _categoryRepository.GetCategoryByNameAsync(command.Name) != null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "Category with same name already exists");
            return;
        }

        var category = Category.Create(command.Name);
        var categoryId = await _categoryRepository.AddCategoryAsync(category);
        
        var jsonResponse = JsonConvert.SerializeObject(new { Message = "Category added successfully", Id = categoryId });
        await HttpHelpers.SetResponseAsync(context, HttpStatusCode.Created, jsonResponse);
    }
    
    #endregion
}