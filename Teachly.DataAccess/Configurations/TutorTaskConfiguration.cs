using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class TutorTaskConfiguration : IEntityTypeConfiguration<TutorTaskEntity>
    {
        public void Configure(EntityTypeBuilder<TutorTaskEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .HasMaxLength(TutorTask.MAX_TITLE_LENGTH)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(TutorTask.MAX_DESCRIPTION_LENGTH)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.LessonPackage)
                .WithMany(x => x.TutorTasks)
                .HasForeignKey(x => x.LessonPackageId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
