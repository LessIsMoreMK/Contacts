using Contacts.Domain.Entities;

namespace Contacts.Tests.Tests.DomainObjects;

public class CategoryTests
{
    [Fact]
    public void Create_WithValidName_CreatesCategory()
    {
        var name = "TestCategory";

        var category = Category.Create(name);

        category.ShouldNotBeNull();
        category.Name.ShouldBe(name);
        category.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithNullName_ThrowsArgumentNullException()
    {
        string name = null!;

        Should.Throw<ArgumentNullException>(() => Category.Create(name));
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsArgumentNullException()
    {
        var name = "";

        Should.Throw<ArgumentNullException>(() => Category.Create(name));
    }
}