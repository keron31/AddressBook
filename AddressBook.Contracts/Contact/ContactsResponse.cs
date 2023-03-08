namespace AddressBook.Contracts.Contact;

public record ContactsResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);