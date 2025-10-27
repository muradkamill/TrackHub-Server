using Domain.Category;
using Domain.Product;
using Domain.SubCategory;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SubCategoryConfiguration:IEntityTypeConfiguration<SubCategoryEntity>
{
    public void Configure(EntityTypeBuilder<SubCategoryEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SubCategoryName).HasMaxLength(30).IsRequired();
    }
}