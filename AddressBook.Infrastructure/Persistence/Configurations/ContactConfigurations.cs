using AddressBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.Infrastructure.Persistence.Configurations;

public class ContactConfigurations : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        ConfigureContactsTable(builder);
    }

    private void ConfigureContactsTable(EntityTypeBuilder<Contact> builder)
    {
        builder.ToTable("Contacts");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Category)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.SubCategory)
            .HasMaxLength(50);

        builder.Property(x => x.Phone)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DateOfBirth)
            .IsRequired();
    }
}