using marketplace.api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace marketplace.api.Infrastructure.ConfigurationModelBuilder
{
    public class SellerProductModelBuilder : IEntityTypeConfiguration<SellerProduct>
    {
        public void Configure(EntityTypeBuilder<SellerProduct> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.SellerName)
           .IsRequired()
           .HasColumnType("varchar(50)");

            builder.Property(s => s.ProductId)
           .IsRequired();

            builder.Property(s => s.SellerProductId)
             .IsRequired()
             .HasColumnType("varchar(50)");

            builder.ToTable("SellerProduct");
        }
    }
}
