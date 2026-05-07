using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class TutorSubjectConfiguration : IEntityTypeConfiguration<TutorSubjectEntity>
    {
        public void Configure(EntityTypeBuilder<TutorSubjectEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PricePerHour)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.HasIndex(x => new { x.TutorId, x.SubjectId })
                .IsUnique();

            builder.HasOne(x => x.Tutor)
                .WithMany(x => x.TutorSubjects)
                .HasForeignKey(x => x.TutorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Subject)
                .WithMany(x => x.TutorSubjects)
                .HasForeignKey(x => x.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_TutorSubjects_PricePerHour", "\"PricePerHour\" > 0"));
        }
    }
}
