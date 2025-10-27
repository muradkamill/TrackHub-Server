using Domain.Suggestion;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class SuggestionConfiguration:IEntityTypeConfiguration<SuggestionEntity>
{
    public void Configure(EntityTypeBuilder<SuggestionEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Suggestion).HasMaxLength(300);
    }
}