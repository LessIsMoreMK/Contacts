using Contacts.Domain.Entities;

namespace Contacts.Tests.Tests.DomainObjects;

public class UserTests
{
    [Fact]
    public void Create_WithValidUserNameAndPassword_CreatesUser()
    {
        var userName = "TestUser";
        var password = "Password123!";

        var user = User.Create(userName, password);

        user.ShouldNotBeNull();
        user.UserName.ShouldBe(userName);
        user.Password.ShouldBe(password);
        user.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public void Create_WithNullUserName_ThrowsArgumentNullException()
    {
        string userName = null!;
        var password = "TestPassword";

        Should.Throw<ArgumentNullException>(() => User.Create(userName, password));
    }

    [Fact]
    public void Create_WithEmptyUserName_ThrowsArgumentNullException()
    {
        var userName = "";
        var password = "TestPassword";

        Should.Throw<ArgumentNullException>(() => User.Create(userName, password));
    }

    [Fact]
    public void Create_WithNullPassword_ThrowsArgumentException()
    {
        var userName = "TestUser";
        string password = null!;

        Should.Throw<ArgumentException>(() => User.Create(userName, password));
    }

    [Fact]
    public void Create_WithEmptyPassword_ThrowsArgumentException()
    {
        var userName = "TestUser";
        var password = "";

        Should.Throw<ArgumentException>(() => User.Create(userName, password));
    }
}