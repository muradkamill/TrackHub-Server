using Domain.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ProductConfiguration:IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.ProductRate).HasColumnType("decimal(18,1)").IsRequired();

        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        builder.Property(x => x.ImageUrls).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
        builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Weight).HasColumnType("decimal(18,2)");
        // builder.Property(p => p.ImageUrls)
        //     .HasConversion(
        //         v => string.Join(";", v),
        //         v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
        //     );

        
    }
}