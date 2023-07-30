using Contacts.Infrastructure.Repositories.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Repositories.Postgres.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<UsersDb>
{
    public void Configure(EntityTypeBuilder<UsersDb> builder)
    {
        builder.HasKey(a => a.Id);
    }
}