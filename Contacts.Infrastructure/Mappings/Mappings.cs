using Contacts.Application.Dtos;
using Contacts.Domain.Entities;
using Contacts.Infrastructure.Repositories.Postgres.Models;
using Mapster;

namespace Contacts.Infrastructure.Mappings;

/// <summary>
/// Contains methods for setting up mappings between different types in the application.
/// These mappings are used for converting objects of one type to another, typically from 
/// domain models to database models and vice versa, or to DTOs (Data Transfer Objects).
/// This class uses the Mapster library for object-to-object mapping.
/// </summary>
public static class Mappings
{
    /// <summary>
    /// Sets up mappings between UsersDb and User types.
    /// </summary>
    public static void UsersMappings()
    {
        TypeAdapterConfig<UsersDb, User>.NewConfig();
        TypeAdapterConfig<User, UsersDb>.NewConfig();
    }
    
    /// <summary>
    /// Sets up mappings between ContactsDb, Contact, and ContactSimpleDto types.
    /// </summary>
    public static void ContactsMappings()
    {
        TypeAdapterConfig<ContactsDb, Contact>.NewConfig();
        TypeAdapterConfig<Contact, ContactsDb>.NewConfig();
        TypeAdapterConfig<Contact, ContactSimpleDto>.NewConfig();
    }
    
    /// <summary>
    /// Sets up mappings between CategoriesDb and Category types, including their Subcategories.
    /// </summary>
    public static void CategoriesMappings()
    {
        TypeAdapterConfig<CategoriesDb, Category>.NewConfig()
            .Map(dest => dest.Subcategories, src => src.Subcategories.Adapt<ICollection<Subcategory>>());

        TypeAdapterConfig<Category, CategoriesDb>.NewConfig()
            .Map(dest => dest.Subcategories, src => src.Subcategories.Adapt<ICollection<SubcategoriesDb>>());
    }
}