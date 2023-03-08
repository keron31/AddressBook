namespace AddressBook.Contracts.Contact;

public record ContactsResponse(
    IEnumerable<ContactResponse> Contacts
);

public record ContactResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email
);