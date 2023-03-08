using AddressBook.Application.Services.Authentication;
using AddressBook.Application.Services.Contacts;
using Microsoft.Extensions.DependencyInjection;

namespace AddressBook.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IContactService, ContactService>();
        return services;
    }
}