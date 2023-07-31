using BuilderGenerator;
using Contacts.Domain.Entities;

namespace Contacts.Tests.Builders;

//NOTE: For more information see https://www.nuget.org/packages/BuilderGenerator/2.0.2
[BuilderFor(typeof(User))]
public partial class UserBuilder
{
    public static UserBuilder Simple()
    {
        return new UserBuilder()
            .WithId(Guid.NewGuid)
            .WithUserName("Bob")
            .WithPassword("Password123!");
    }
}