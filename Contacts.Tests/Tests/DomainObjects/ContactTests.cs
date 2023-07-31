using Contacts.Domain.Entities;

namespace Contacts.Tests.Tests.DomainObjects;

public class ContactTests
{
    [Fact]
    public void Create_WithValidParameters_CreatesNewContact()
    {
        var firstName = "Bob";
        var lastName = "Smith";
        var email = "bob.smith@test.com";
        var phone = "123456789";
        var birthDate = new DateTime(2000, 1, 1);
        var categoryId = Guid.NewGuid();

        var contact = Contact.Create(firstName, lastName, email, phone, birthDate, categoryId);

        contact.ShouldNotBeNull();
        contact.FirstName.ShouldBe(firstName);
        contact.LastName.ShouldBe(lastName);
        contact.Email.ShouldBe(email);
        contact.Phone.ShouldBe(phone);
        contact.BirthDate.ShouldBe(birthDate);
        contact.CategoryId.ShouldBe(categoryId);
    }
    
    [Fact]
    public void Create_WithInvalidEmail_ThrowsArgumentException()
    {
        var firstName = "Bob";
        var lastName = "Smith";
        var email = "invalid email";
        var phone = "123456789";
        var birthDate = new DateTime(2000, 1, 1);
        var categoryId = Guid.NewGuid();

        Should.Throw<ArgumentException>(() => Contact.Create(firstName, lastName, email, phone, birthDate, categoryId));
    }

    [Fact]
    public void Update_WithValidParameters_UpdatesContact()
    {
        var firstName = "Bob";
        var lastName = "Smith";
        var email = "bob.smith@test.com";
        var phone = "123456789";
        var birthDate = new DateTime(2000, 1, 1);
        var categoryId = Guid.NewGuid();

        var contact = Contact.Create(firstName, lastName, email, phone, birthDate, categoryId);

        var newFirstName = "Bob";
        var newLastName = "Smith2";
        var newEmail = "bob.smith2@test.com";
        var newPhone = "987654321";
        var newBirthDate = new DateTime(2001, 1, 1);
        var newCategoryId = Guid.NewGuid();

        contact.Update(newFirstName, newLastName, newEmail, newPhone, newBirthDate, newCategoryId);

        contact.FirstName.ShouldBe(newFirstName);
        contact.LastName.ShouldBe(newLastName);
        contact.Email.ShouldBe(newEmail);
        contact.Phone.ShouldBe(newPhone);
        contact.BirthDate.ShouldBe(newBirthDate);
        contact.CategoryId.ShouldBe(newCategoryId);
    }

    [Fact]
    public void Update_WithInvalidEmail_ThrowsArgumentException()
    {
        var firstName = "Bob";
        var lastName = "Smith";
        var email = "bob.smith@test.com";
        var phone = "123456789";
        var birthDate = new DateTime(2000, 1, 1);
        var categoryId = Guid.NewGuid();

        var contact = Contact.Create(firstName, lastName, email, phone, birthDate, categoryId);
        var newEmail = "invalid email";

        Should.Throw<ArgumentException>(() => contact.Update(firstName, lastName, newEmail, phone, birthDate, categoryId));
    }
}