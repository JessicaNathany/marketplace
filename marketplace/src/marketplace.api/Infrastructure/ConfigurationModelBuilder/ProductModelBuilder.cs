using marketplace.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace marketplace.api.Infrastructure.ConfigurationModelBuilder; 

public class ProductModelBuilder : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasColumnType("varchar(50)");

        builder.Property(p => p.Category)
            .HasColumnType("varchar(50)");

        builder.Property(p => p.Brand)
            .HasColumnType("varchar(50)");

        builder.ToTable("Product");
    }
}
