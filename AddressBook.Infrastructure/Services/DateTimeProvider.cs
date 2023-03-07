using AddressBook.Application.Common.Interfaces.Services;

namespace AddressBook.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}