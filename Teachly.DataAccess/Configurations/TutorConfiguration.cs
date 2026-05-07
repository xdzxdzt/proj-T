using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class TutorConfiguration : IEntityTypeConfiguration<TutorEntity>
    {
        public void Configure(EntityTypeBuilder<TutorEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                .HasMaxLength(Tutor.MAX_DESCRIPTION_LENGTH);

            builder.Property(x => x.AverageRating)
                .HasPrecision(3, 2)
                .IsRequired();

            builder.Property(x => x.RatingCount)
                .IsRequired();

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Tutor)
                .HasForeignKey<TutorEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_Tutors_AverageRating", "\"AverageRating\" BETWEEN 0 AND 5");
                t.HasCheckConstraint("CK_Tutors_RatingCount", "\"RatingCount\" >= 0");
            });
        }
    }
}
