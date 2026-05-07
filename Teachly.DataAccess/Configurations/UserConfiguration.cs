using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.UserName)
                .HasMaxLength(User.MAX_USERNAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.FirstName)
                .HasMaxLength(User.MAX_FIRSTNAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.LastName)
                .HasMaxLength(User.MAX_LASTNAME_LENGTH)
                .IsRequired();

            builder.Property(x => x.Age)
                .IsRequired();

            builder.Property(x => x.Email)
                .HasMaxLength(User.MAX_EMAIL_LENGTH)
                .IsRequired();

            builder.HasIndex(x => x.Email)
                .IsUnique();

            builder.Property(x => x.PasswordHash)
                .HasMaxLength(User.MAX_PASSWORD_HASH_LENGTH)
                .IsRequired();

            builder.Property(x => x.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(x => x.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(x => x.RegisteredAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder
                .ToTable(t => t.HasCheckConstraint("CK_Users_Age","\"Age\" > 0"));
        }
    }
}
