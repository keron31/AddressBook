using AddressBook.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBook.Infrastructure.Persistence;

public class AddressBookDbContext : DbContext
{
    public AddressBookDbContext(DbContextOptions<AddressBookDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Contact> Contacts { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AddressBookDbContext).Assembly);
    }
}