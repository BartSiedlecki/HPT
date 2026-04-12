using HTP.Domain.Entities.RefreshTokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTP.Infrastructure.Persistence.RefreshTokens.Write;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Token)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(t => t.ExpiredOnUtc)
            .IsRequired();

        builder.Property(t => t.UserId)
            .IsRequired();
    }
}