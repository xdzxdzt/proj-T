using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class LessonPackageConfiguration : IEntityTypeConfiguration<LessonPackageEntity>
    {
        public void Configure(EntityTypeBuilder<LessonPackageEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.LessonPackages)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.TutorSubject)
                .WithMany(x => x.LessonPackages)
                .HasForeignKey(x => x.TutorSubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.TotalLessons)
                .IsRequired();

            builder.Property(x => x.RemainingLessons)
                .IsRequired();

            builder.Property(x => x.PricePerLesson)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(x => x.TotalPrice)
                .HasPrecision(10, 2)
                .IsRequired();

            builder.Property(x => x.PurchasedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.CompletedAt);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_LessonPackages_TotalLessons", "\"TotalLessons\" > 0");
                t.HasCheckConstraint("CK_LessonPackages_RemainingLessons", "\"RemainingLessons\" >= 0");
                t.HasCheckConstraint("CK_LessonPackages_PricePerLesson", "\"PricePerLesson\" > 0");
                t.HasCheckConstraint("CK_LessonPackages_TotalPrice", "\"TotalPrice\" > 0");
                t.HasCheckConstraint("CK_LessonPackages_TotalPrice_Calculated","\"TotalPrice\" = \"TotalLessons\" * \"PricePerLesson\"");
                t.HasCheckConstraint("CK_LessonPackages_RemainingLessons_Max","\"RemainingLessons\" <= \"TotalLessons\"");
            });


        }
    }
}
