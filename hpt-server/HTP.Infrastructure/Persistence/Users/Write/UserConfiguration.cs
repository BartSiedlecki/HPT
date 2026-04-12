using HPT.SharedKernel.Constants;
using HTP.Domain.Entities.Users;
using HTP.Infrastructure.Persistence.SharedValueObjectsConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HTP.Infrastructure.Persistence.Users.Write;

using Lengths = FieldLengths.User;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(Lengths.FirstNameMaxLength);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(Lengths.LastNameMaxLength);

        builder.Property(u => u.Email)
            .HasConversion(new EmailConverter())
            .IsRequired()
            .HasMaxLength(FieldLengths.Email.MaxLength);

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.IsBlocked)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.ActivatedAt);

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
