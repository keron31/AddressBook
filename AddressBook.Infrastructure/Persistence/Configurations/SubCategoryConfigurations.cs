using AddressBook.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AddressBook.Infrastructure.Persistence.Configurations;

public class SubCategoryConfigurations : IEntityTypeConfiguration<SubCategory>
{
    public void Configure(EntityTypeBuilder<SubCategory> builder)
    {
        ConfigureSubCategoriesTable(builder);
    }

    private void ConfigureSubCategoriesTable(EntityTypeBuilder<SubCategory> builder)
    {
        builder.ToTable("SubCategories");

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.CategoryId)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.SubCategories)
            .HasForeignKey(x => x.CategoryId);
    }
}