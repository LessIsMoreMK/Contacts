using Contacts.Domain.Entities;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Mapster;

namespace Contacts.Infrastructure.Mappings;

public static class Mappings
{
    public static void UsersMappings()
    {
        TypeAdapterConfig<UsersDb, User>.NewConfig();
        TypeAdapterConfig<User, UsersDb>.NewConfig();
    }
    
    public static void ContactsMappings()
    {
        TypeAdapterConfig<ContactsDb, Contact>.NewConfig();
        TypeAdapterConfig<Contact, ContactsDb>.NewConfig();
    }
    
    public static void CategoriesMappings()
    {
        TypeAdapterConfig<CategoriesDb, Category>.NewConfig()
            .Map(dest => dest.Subcategories, src => src.Subcategories.Adapt<ICollection<Subcategory>>());

        TypeAdapterConfig<Category, CategoriesDb>.NewConfig()
            .Map(dest => dest.Subcategories, src => src.Subcategories.Adapt<ICollection<SubcategoriesDb>>());
    }
}