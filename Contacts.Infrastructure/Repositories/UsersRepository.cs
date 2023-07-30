using Contacts.Domain.Repositories;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;

namespace Contacts.Infrastructure.Repositories;

public class UsersRepository : IUsersRepository
{
    #region Fields
    
    private readonly ApplicationDbContext _dbContext;

    #endregion
    
    #region Constructor
    
    public UsersRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    #endregion
    
    #region Methods
    
    #endregion
}