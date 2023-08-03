using System.Net;
using Contacts.Application.Commands;
using Contacts.Application.Commands.Categories;
using Contacts.Application.Commands.Contacts;
using Contacts.Application.Commands.Users;
using Contacts.Application.Dtos;
using Contacts.Application.Helpers;
using Contacts.Domain.Repositories;
using Contacts.Domain.Services;
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
        BuildUsersEndpoints(app);
        
        return app;
    }
    
    /// <summary>
    /// Method to configure categories related endpoints.
    /// </summary>
    /// <param name="app">WebApplication instance to configure</param>
    private static void BuildCategoriesEndpoints(WebApplication app)
    {
        // Endpoint to fetch all categories with their subcategories
        app.MapGet("/categories", HandleGetCategories).RequireAuthorization();

        // Endpoint to add a new category
        app.MapPost("/categories", HandleAddCategory).WithName("AddCategory").RequireAuthorization();
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
        app.MapGet("/contacts/{id:guid}", HandleGetContactById).RequireAuthorization();

        // Endpoint to add a new contact
        app.MapPost("/contacts", HandleAddContact).RequireAuthorization();
    
        // Endpoint to update an existing contact
        app.MapPut("/contacts", HandleUpdateContact).RequireAuthorization();

        // Endpoint to delete a contact by id
        app.MapDelete("/contacts/{id:guid}", HandleDeleteContact).RequireAuthorization();
    }
    
    /// <summary>
    /// Method to configure users related endpoints.
    /// </summary>
    /// <param name="app">WebApplication instance to configure</param>
    private static void BuildUsersEndpoints(WebApplication app)
    {
        // Endpoint to add a new user
        app.MapPost("/users", HandleAddUser);
        
        // Endpoint to login
        app.MapPost("/login", HandleLogin);
        
        // Endpoint to logout
        app.MapPost("/logout", HandleLogout);
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
        HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
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
            loggerHelper.LogError(ex, "Error occurred while HandleAddCategory");
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
    
    private static async Task HandleAddContact(HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
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
            loggerHelper.LogError(ex, "Error occurred while HandleAddContact");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    private static async Task HandleUpdateContact(HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
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
            loggerHelper.LogError(ex, "Error occurred while HandleUpdateContact");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }

    private static async Task HandleDeleteContact(
        HttpContext httpContext, Guid id, 
        [FromServices] IContactsRepository repository, 
        [FromServices] LoggerHelpers loggerHelper)
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
            loggerHelper.LogError(ex, "Error occurred while HandleDeleteContact");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    #endregion
    
    #region Users
    
    public static async Task HandleAddUser(
        HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            var addUserRequest = await httpContext.Request.ReadFromJsonAsync<AddUserRequest>();
            if (addUserRequest == null)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new { Error = "Request body is null" });
                return;
            }

            var addUserHandler = httpContext.RequestServices.GetRequiredService<ICommandHandler<AddUserRequest>>();
            await addUserHandler.HandleAsync(addUserRequest, httpContext);
        }
        catch (ArgumentException ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleAddUser");
            await HttpHelpers.SetResponseAsync(httpContext, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleAddUser");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
    
    private static async Task HandleLogin(HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            var loginRequest = await httpContext.Request.ReadFromJsonAsync<LoginRequest>();

            if (loginRequest == null)
            {
                httpContext.Response.StatusCode = (int) HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync(new {Error = "Request body is null"});
                return;
            }

            var usersRepository = httpContext.RequestServices.GetRequiredService<IUsersRepository>();
            var passwordService = httpContext.RequestServices.GetRequiredService<IPasswordService>();

            var user = await usersRepository.GetUserByUserNameAsync(loginRequest.UserName);

            if (user == null)
            {
                await HttpHelpers.SetResponseAsync(httpContext, HttpStatusCode.Unauthorized, "Invalid username or password");
                return;
            }

            if (!passwordService.VerifyPassword(loginRequest.Password, user.Password, user.Salt))
            {
                await HttpHelpers.SetResponseAsync(httpContext, HttpStatusCode.Unauthorized, "Invalid username or password");
                return;
            }

            var token = passwordService.GenerateJwtToken(user);
            await httpContext.Response.WriteAsJsonAsync(new {token});
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleLogin");
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new {Error = ex.Message});
        }
    }
    
    private static async Task HandleLogout(HttpContext httpContext, 
        [FromServices] LoggerHelpers loggerHelper)
    {
        try
        {
            //Just return OK for now
            httpContext.Response.StatusCode = (int) HttpStatusCode.OK;
            await httpContext.Response.WriteAsJsonAsync(new {Message = "Logout successful"});
        }
        catch (Exception ex)
        {
            loggerHelper.LogError(ex, "Error occurred while HandleLogout");
            httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(new {Error = ex.Message});
        }
    }
    
    #endregion
}
