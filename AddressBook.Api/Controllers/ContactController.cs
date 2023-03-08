using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Application.Services.Contacts;
using AddressBook.Contracts.Contact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AddressBook.Api.Controllers;

[ApiController]
[Authorize]
[Route("contacts")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IContactRepository _contactRepository;

    public ContactController(IContactService contactService, IContactRepository contactRepository)
    {
        _contactService = contactService;
        _contactRepository = contactRepository;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateContact(CreateContactRequest request)
    {
        var result = await _contactService.CreateContact(request.FirstName,
            request.LastName,
            request.Email,
            request.Password,
            request.Category,
            request.SubCategory,
            request.Phone,
            request.DateOfBirth);

        var response = new OneContactResponse(
            result.Contact.Id,
            result.Contact.FirstName,
            result.Contact.LastName,
            result.Contact.Email,
            result.Contact.Category,
            result.Contact.SubCategory,
            result.Contact.Phone,
            result.Contact.DateOfBirth
        );

        return Ok(response);
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllContacts()
    {
        var result = await _contactRepository.GetContactsAsync();

        var response = new ContactsResponse(result.Select(contact => new ContactResponse(
            contact.Id,
            contact.FirstName,
            contact.LastName,
            contact.Email
        )));

        return Ok(response);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetContactById(string id)
    {
        var result = await _contactRepository.GetContactByIdAsync(Guid.Parse(id));

        var response = new OneContactResponse(
            result.Id,
            result.FirstName,
            result.LastName,
            result.Email,
            result.Category,
            result.SubCategory,
            result.Phone,
            result.DateOfBirth
        );

        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteContact(string id)
    {
        var result = await _contactRepository.GetContactByIdAsync(Guid.Parse(id));

        await _contactRepository.DeleteContactAsync(result);

        return Ok();
    }
}