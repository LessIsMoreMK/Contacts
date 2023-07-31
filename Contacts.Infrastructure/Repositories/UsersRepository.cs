using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

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

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        var usersDb = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserName == userName);
        
        return usersDb?.Adapt<User>();
    }

    public async Task AddUserAsync(User user)
    {
        if (user == null) 
            throw new ArgumentNullException(nameof(user));
        
        var usersDb = user.Adapt<UsersDb>();
        
        await _dbContext.Users.AddAsync(usersDb);
        await _dbContext.SaveChangesAsync();
    }
}