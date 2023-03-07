namespace AddressBook.Infrastructure.Services.Settings;

public class DatabaseConnectSettings
{
    public const string SectionName = "DatabaseConnectSettings";
    public string Server { get; init; } = null!;
    public string Database { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
}