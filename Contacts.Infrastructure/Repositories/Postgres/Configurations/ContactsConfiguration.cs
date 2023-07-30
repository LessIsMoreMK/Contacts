using Contacts.Infrastructure.Helpers;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Contacts.Infrastructure.Repositories.Postgres.Configurations;

public class ContactsConfiguration : IEntityTypeConfiguration<ContactsDb>
{
    private static readonly DateTimeKindValueConverter DateTimeKindValueConverter = new(DateTimeKind.Utc);
    
    public void Configure(EntityTypeBuilder<ContactsDb> builder)
    {
        builder.HasKey(a => a.Id);
       
        builder.Property(a => a.BirthDate).HasConversion(DateTimeKindValueConverter);
    }
}