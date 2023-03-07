namespace AddressBook.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddCors(options =>
            options.AddPolicy("AllowLocalhost", builder =>
                builder.WithOrigins("http://localhost:3000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            )
        );

        return services;
    }
}