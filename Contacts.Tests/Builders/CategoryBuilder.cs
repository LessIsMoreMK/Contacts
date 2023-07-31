using BuilderGenerator;
using Contacts.Domain.Entities;

namespace Contacts.Tests.Builders;

//NOTE: For more information see https://www.nuget.org/packages/BuilderGenerator/2.0.2
[BuilderFor(typeof(Category))]
public partial class CategoryBuilder
{
    public static CategoryBuilder Simple()
    {
        return new CategoryBuilder()
                .WithId(Guid.NewGuid)
                .WithName("Private")
            ;
    }
}