using AddressBook.Api;
using AddressBook.Application;
using AddressBook.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddPresentation()
        .AddApplication()
        .AddInfrastructure(builder.Configuration);
}

var app = builder.Build();
{
    app.UseCors("AllowLocalhost");
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseAuthentication();
    app.MapControllers();
    app.Run();
}