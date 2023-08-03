using Contacts.Domain.Entities;
using Contacts.Domain.Repositories;
using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Contacts.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    #region Fields
    
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CategoryRepository> _logger;

    #endregion
    
    #region Constructor
    
    public CategoryRepository(ApplicationDbContext dbContext, ILogger<CategoryRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    #endregion
    
    #region Methods
    
    public async Task<IEnumerable<Category>> GetAllCategoriesWithSubcategoriesAsync()
    {
        var categoriesDb =  await _dbContext.Categories
            .Include(c => c.Subcategories)
            .AsNoTracking()
            .ToListAsync();
    
        return categoriesDb.Adapt<List<Category>>();
    }

    public async Task<Category?> GetCategoryByIdAsync(Guid categoryId)
    {
        var categoryDb = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == categoryId);
                
        return categoryDb?.Adapt<Category>();
    }
    
    public async Task<Category?> GetCategoryByNameAsync(string categoryName)
    {
        var contactDb = await _dbContext.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == categoryName);
                
        return contactDb?.Adapt<Category>();
    }

    public async Task<Guid> AddCategoryAsync(Category category)
    {
        if (category == null) 
            throw new ArgumentNullException(nameof(category));
        
        var cryptoDataDb = category.Adapt<CategoriesDb>();
        
        await _dbContext.Categories.AddAsync(cryptoDataDb);
        await _dbContext.SaveChangesAsync();
        
        return cryptoDataDb.Id;
    }

    #endregion
}