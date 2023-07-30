using System.Net;
using Contacts.Application.Commands;
using Contacts.Application.Commands.Categories;
using Contacts.Application.Commands.Contacts;
using Contacts.Application.Dtos;
using Contacts.Application.Helpers;
using Contacts.Domain.Repositories;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Contacts.Api;

/// <summary>
/// Static class to build and configure application endpoints.
/// </summary>
public static class EndpointBuilder
{
    #region Endpoints
    
    /// <summary>
    /// Method to build all application endpoints.
    /// </summary>
    /// <param name="app">WebApplication instance to configure.</param>
    /// <returns>Configured WebApplication instance.</returns>
    public static WebApplication BuildApplicationEndpoints(WebApplication app)
    {
        app.MapGet("/", () => "Contacts API");
        
        BuildCategoriesEndpoints(app);
        BuildContactsEndpoints(app);
        
        return app;
    }
    
    /// <summary>
    /// Method to configure categories related endpoints.
    /// </summary>
    /// <param name="app">WebApplication instance to configure</param>
    private static void BuildCategoriesEndpoints(WebApplication app)
    {
        // Endpoint to fetch all categories with their subcategories
        app.MapGet("/categories", HandleGetCategories);

        // Endpoint to add a new category
        app.MapPost("/categories", HandleAddCategory).WithName("AddCategory");
    }
    
    /// <summary>
    /// Method to configure contacts related endpoints.
    /// </summary>
    /// <param name="app">WebApplication instance to configure</param>
    private static void BuildContactsEndpoints(WebApplication app)
    {
        // Endpoint to fetch all contacts
        app.MapGet("/contacts", HandleGetAllContacts);
    
        // Endpoint to fetch a contact by id
        app.MapGet("/contacts/{id:guid}", HandleGetContactById);

        // Endpoint to add a new contact
        app.MapPost("/contacts", HandleAddContact);
    
        // Endpoint to update an existing contact
        app.MapPut("/contacts", HandleUpdateContact);

        // Endpoint to delete a contact by id
        app.MapDelete("/contacts/{id:guid}", HandleDeleteContact);
    }
    
    #endregion
    
    #region Categories

    private static async Task<IResult> HandleGetCategories(
        [FromServices] ICategoryRepository repository, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            var categories = await repository.GetAllCategoriesWithSubcategoriesAsync();
            return Results.Ok(categories);
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleGetCategories");
            return Results.Problem("An error occurred while processing the request. Please try again later.", 
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task HandleAddCategory(
        HttpContext httpContext)
    {
        try
        {
            var categoryRequest = await httpContext.Request.ReadFromJsonAsync<AddCategoryRequest>();
            if (categoryRequest == null)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Request body is null" });
                return;
            }

            var addCategoryHandler = httpContext.RequestServices.GetRequiredService<ICommandHandler<AddCategoryRequest>>();
            await addCategoryHandler.HandleAsync(categoryRequest, httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    #endregion
    
    #region Contacts
    
    private static async Task<IResult> HandleGetAllContacts(
        [FromServices] IContactsRepository repository, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            var contacts = await repository.GetAllContactsAsync();
            var contactsSimpleDto = contacts.Adapt<IEnumerable<ContactSimpleDto>>();
            return Results.Ok(contactsSimpleDto);
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleGetAllContacts");
            return Results.Problem("An error occurred while processing the request. Please try again later.", 
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private static async Task<IResult> HandleGetContactById(
        Guid id, 
        [FromServices] IContactsRepository repository, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            var contact = await repository.GetContactByIdAsync(id);
            if (contact == null)
                return Results.NotFound($"Contact with ID '{id}' not found");
            return Results.Ok(contact);
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleGetAllContacts");
            return Results.Problem("An error occurred while processing the request. Please try again later.", 
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
    
    private static async Task HandleAddContact(HttpContext httpContext)
    {
        try
        {
            var contactRequest = await httpContext.Request.ReadFromJsonAsync<AddContactRequest>();
            if (contactRequest == null)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Request body is null" });
                return;
            }

            var addContactRequest = httpContext.RequestServices.GetRequiredService<ICommandHandler<AddContactRequest>>();
            await addContactRequest.HandleAsync(contactRequest, httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    private static async Task HandleUpdateContact(HttpContext httpContext)
    {
        try
        {
            var contactRequest = await httpContext.Request.ReadFromJsonAsync<UpdateContactRequest>();
            if (contactRequest == null)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Request body is null" });
                return;
            }

            var addContactRequest = httpContext.RequestServices.GetRequiredService<ICommandHandler<UpdateContactRequest>>();
            await addContactRequest.HandleAsync(contactRequest, httpContext);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }

    private static async Task HandleDeleteContact(
        HttpContext httpContext, Guid id, 
        [FromServices] IContactsRepository repository)
    {
        try
        {
            var result = await repository.DeleteContactAsync(id);
            if (result)
                httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            else 
                httpContext.Response.StatusCode = (int) HttpStatusCode.NotFound;
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    #endregion
}
