using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Roles;
using HTP.Infrastructure.Persistence.SharedValueObjectsConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTP.Infrastructure.Persistence.Roles.Write;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasConversion(new RoleNameConverter())
            .IsRequired()
            .HasMaxLength(FieldLengths.Role.NameMaxLength);

        builder.HasMany(r => r.RolePermissions)
            .WithOne()
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.CreatedByUserId)
            .IsRequired();

        builder.Property(r => r.LastModifiedAt);

        builder.Property(r => r.LastModifiedByUserId);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}
