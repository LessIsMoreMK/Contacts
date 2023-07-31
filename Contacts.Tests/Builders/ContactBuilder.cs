using BuilderGenerator;
using Contacts.Domain.Entities;

namespace Contacts.Tests.Builders;

//NOTE: For more information see https://www.nuget.org/packages/BuilderGenerator/2.0.2
[BuilderFor(typeof(Contact))]
public partial class ContactBuilder
{
    public static ContactBuilder Simple()
    {
        return new ContactBuilder()
                .WithId(Guid.NewGuid)
                .WithFirstName("Bob")
                .WithLastName("Smith")
                .WithBirthDate(new DateTime(1980, 1, 1))
                .WithPhone("+38123123123")
                .WithCategoryId(Guid.NewGuid);
    }
}