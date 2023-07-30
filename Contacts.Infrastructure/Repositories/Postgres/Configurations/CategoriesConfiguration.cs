using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Repositories.Postgres.Configurations;

public class CategoriesConfiguration : IEntityTypeConfiguration<CategoriesDb>
{
    public void Configure(EntityTypeBuilder<CategoriesDb> builder)
    {
        builder.HasKey(a => a.Id);
       
        builder.HasData(Seed.GetCategories());
    }
}