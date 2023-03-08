using System.Text;
using AddressBook.Application.Common.Interfaces.Authentication;
using AddressBook.Application.Common.Interfaces.Persistence;
using AddressBook.Application.Common.Interfaces.Services;
using AddressBook.Infrastructure.Authentication;
using AddressBook.Infrastructure.Persistence;
using AddressBook.Infrastructure.Persistence.Repositories;
using AddressBook.Infrastructure.Services;
using AddressBook.Infrastructure.Services.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AddressBook.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services
            .AddAuth(configuration)
            .AddPersistence(configuration);

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
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IContactRepository, ContactRepository>();
        // if connection to database is successful, call the AddDefaultCategoryAsync method to add default categories if they don't exist
        using var scope = services.BuildServiceProvider().CreateScope();
        var scopedServices = scope.ServiceProvider;
        var dbContext = scopedServices.GetRequiredService<AddressBookDbContext>();
        dbContext.Database.Migrate();
        var categoryRepository = scopedServices.GetRequiredService<ICategoryRepository>();
        categoryRepository.AddDefaultCategoryAsync().Wait();


        return services;
    }

    public static IServiceCollection AddAuth(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.Bind(JwtSettings.SectionName, jwtSettings);

        services.AddSingleton(Options.Create(jwtSettings));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IHashHelper, HashHelper>();

        services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings.Secret))
            });

        return services;
    }
}