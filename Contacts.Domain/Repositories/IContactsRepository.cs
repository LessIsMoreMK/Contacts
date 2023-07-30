namespace Contacts.Domain.Repositories;

public interface IContactsRepository
{
    public Task AddContactAsync();
}