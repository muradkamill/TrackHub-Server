using Domain;
using Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PersonConfiguration:IEntityTypeConfiguration<PersonEntity>
{
    public void Configure(EntityTypeBuilder<PersonEntity> builder)
    {
        builder.HasKey(x => x.Fin);
        builder.Property(x => x.Fin).HasMaxLength(7).IsRequired();
        builder.Property(x => x.CourierRate).HasColumnType("decimal(18,2)");
        builder.Property(x => x.Balance).HasColumnType("money");

    }
}