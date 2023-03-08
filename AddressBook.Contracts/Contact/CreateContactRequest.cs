namespace AddressBook.Contracts.Contact;

public record CreateContactRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string Category,
    string? SubCategory,
    string Phone,
    DateTime DateOfBirth
);