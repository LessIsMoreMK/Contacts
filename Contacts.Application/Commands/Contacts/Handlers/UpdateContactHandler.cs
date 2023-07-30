using System.Net;
using Contacts.Application.Helpers;
using Contacts.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Contacts.Application.Commands.Contacts.Handlers;

public class UpdateContactHandler : ICommandHandler<UpdateContactRequest>
{
    #region Fileds
    
    private readonly IContactsRepository _contactsRepository;
    private readonly ICategoryRepository _categoryRepository;
    
    #endregion
    
    #region Constructor

    public UpdateContactHandler(IContactsRepository contactsRepository, ICategoryRepository categoryRepository)
    {
        _contactsRepository = contactsRepository;
        _categoryRepository = categoryRepository;
    }

    #endregion
    
    #region Methods
    
    public async Task HandleAsync(UpdateContactRequest command, HttpContext context)
    {
        if (await _categoryRepository.GetCategoryByIdAsync(command.CategoryId) == null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "Category id invalid");
            return;
        }
        
        if (command.Email is not null)
        {
            var cont = await _contactsRepository.GetContactByEmailAsync(command.Email);
            if (cont is not null && cont.Id != command.Id)
            {
                await HttpHelpers.SetResponseAsync(context, HttpStatusCode.BadRequest, "Email already taken");
                return;
            }
        }

        var contact = await _contactsRepository.GetContactByIdAsync(command.Id);
        if (contact is null)
        {
            await HttpHelpers.SetResponseAsync(context, HttpStatusCode.NotFound, "Contact not found");
            return;
        }
        
        contact.Update(command.FirstName, command.LastName, command.Email, command.Phone, command.BirthDate, command.CategoryId);
        await _contactsRepository.UpdateContactAsync(contact);
        
        await HttpHelpers.SetResponseAsync(context, HttpStatusCode.Created, "Contact updated successfully");
    }
    
    #endregion
}