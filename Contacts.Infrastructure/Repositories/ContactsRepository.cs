using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Infrastructure.Repositories;

public class ContactsRepository : IContactsRepository
{
    #region Fields
    
    private readonly ApplicationDbContext _dbContext;

    #endregion
    
    #region Constructor
    
    public ContactsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    #endregion
    
    #region Methods

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        var contactsDb =  await _dbContext.Contacts
            .AsNoTracking()
            .ToListAsync();
        
        return contactsDb.Adapt<IEnumerable<Contact>>();
    }

    public async Task<Contact?> GetContactByIdAsync(Guid contactId)
    {
        var contactDb = await _dbContext.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == contactId);
        
        return contactDb?.Adapt<Contact>();
    }

    public async Task<Contact?> GetContactByEmailAsync(string contactEmail)
    {
        var contactDb = await _dbContext.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == contactEmail);
        
        return contactDb?.Adapt<Contact>();
    }

    public async Task AddContactAsync(Contact contact)
    {
        if (contact == null) 
            throw new ArgumentNullException(nameof(contact));
        
        var contactDb = contact.Adapt<ContactsDb>();
        
        await _dbContext.Contacts.AddAsync(contactDb);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        if (contact == null) 
            throw new ArgumentNullException(nameof(contact));
        
        var contactToDb = contact.Adapt<ContactsDb>();
        
        var contactDb = (ContactsDb) (await _dbContext.FindAsync(typeof(ContactsDb), contactToDb.Id))!;
            
        _dbContext.Entry(contactDb).CurrentValues.SetValues(contactToDb);
    }

    public async Task DeleteContactAsync(Guid contactId)
    {
        if (contactId == Guid.Empty)
            throw new ArgumentNullException(nameof(contactId));
            
        var contactDb = await _dbContext.Contacts.FindAsync(contactId);
        if (contactDb != null)
            _dbContext.Contacts.Remove(contactDb);
    }
    
    #endregion
}