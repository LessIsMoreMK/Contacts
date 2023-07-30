using System.Net;
using Contacts.Application.Helpers;
using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Microsoft.AspNetCore.Http;

namespace Contacts.Application.Commands.Contacts.Handlers;

public class AddContactHandler : ICommandHandler<AddContactRequest>
{
    #region Fileds
    
    private readonly IContactsRepository _contactsRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    #endregion
    
    #region Constructor

    public AddContactHandler(IContactsRepository contactsRepository, ICategoryRepository categoryRepository)
    {
        _contactsRepository = contactsRepository;
        _categoryRepository = categoryRepository;
    }

    #endregion
    
    #region Methods
    
    public async Task HandleAsync(AddContactRequest command, HttpContext context)
    {
        if (await _categoryRepository.GetCategoryByIdAsync(command.CategoryId) == null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "Category id invalid");
            return;
        }
        
        if (command.Email is not null && await _contactsRepository.GetContactByEmailAsync(command.Email) != null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "Email already taken");
            return;
        }

        var contact = Contact.Create(command.FirstName, command.LastName, command.Email, command.Phone, command.BirthDate, command.CategoryId);
        await _contactsRepository.AddContactAsync(contact);
        
        await HttpHelpers.SetResponseAsync(context, HttpStatusCode.Created, "Contact added successfully");
    }
    
    #endregion
}