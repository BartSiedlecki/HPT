
using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Permissions;
using HTP.Infrastructure.Persistence.Permissions.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTP.Infrastructure.Persistence.Permissions;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(new PermissionIdConvereter())
            .IsRequired()
            .ValueGeneratedNever()
            .HasMaxLength(FieldLengths.PermissionId.MaxLength);

        builder.Property(p => p.Description)
            .HasConversion(new PermissionDescriptionConverter())
            .HasMaxLength(FieldLengths.Permission.DescriptionMaxLength);
    }
}
