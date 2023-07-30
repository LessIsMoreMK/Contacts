using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Contacts.Infrastructure.Helpers;

/// <summary>
/// Value converter that uses DateTime.SpecifyKind method to ensure that
/// all DateTime instances have the specified kind of DateTimeKind.
/// It's useful in scenarios where you need to ensure that DateTime objects
/// have a specific DateTimeKind when reading from or writing to the database.
/// </summary>
public class DateTimeKindValueConverter : ValueConverter<DateTime, DateTime>
{
    public DateTimeKindValueConverter(DateTimeKind kind, ConverterMappingHints? mappingHints = null) 
        : base(v => DateTime.SpecifyKind(v, kind),
            v => DateTime.SpecifyKind(v, kind),
            mappingHints)
    {
    }
}