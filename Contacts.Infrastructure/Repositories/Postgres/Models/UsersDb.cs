namespace Contacts.Infrastructure.Repositories.Postgres.Models;

public class UsersDb
{
    #region Properties
    
    public Guid Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Salt { get; set; } = null!;

    #endregion
}