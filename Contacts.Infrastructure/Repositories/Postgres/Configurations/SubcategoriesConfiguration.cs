using Contacts.Infrastructure.Repositories.Postgres.DbContext;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Repositories.Postgres.Configurations;

public class SubcategoriesConfiguration : IEntityTypeConfiguration<SubcategoriesDb>
{
    public void Configure(EntityTypeBuilder<SubcategoriesDb> builder)
    {
        builder.HasKey(a => a.Id);
       
        builder.HasData(Seed.GetSubcategories());
    }
}