using Domain.Card;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CartConfiguration:IEntityTypeConfiguration<CartEntity>
{
    public void Configure(EntityTypeBuilder<CartEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.PersonFin).HasMaxLength(7).IsRequired();
        builder.Property(x => x.ProductId).IsRequired();
        builder.Property(x => x.CourierFee).HasColumnType("money");
        builder.Property(x => x.Price).HasColumnType("money");
        
    }
}