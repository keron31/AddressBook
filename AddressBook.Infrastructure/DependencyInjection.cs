using AddressBook.Application.Common.Interfaces.Authentication;
using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Application.Common.Interfaces.Services;
using AddressBook.Infrastructure.Authentication;
using AddressBook.Infrastructure.Persistence;
using AddressBook.Infrastructure.Persistence.Repositories;
using AddressBook.Infrastructure.Services;
using AddressBook.Infrastructure.Services.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AddressBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services
            .AddPersistence(configuration);

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }

    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var databaseConnectSettings = new DatabaseConnectSettings();
        configuration.Bind(DatabaseConnectSettings.SectionName, databaseConnectSettings);

        services.AddSingleton(Options.Create(databaseConnectSettings));
        var connectionString = "Server="
            + databaseConnectSettings.Server
            + ";Database="
            + databaseConnectSettings.Database
            + ";user id="
            + databaseConnectSettings.User
            + ";password="
            + databaseConnectSettings.Password;
        services.AddDbContext<AddressBookDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MySqlServerVersion(new Version(10, 4, 26))));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}