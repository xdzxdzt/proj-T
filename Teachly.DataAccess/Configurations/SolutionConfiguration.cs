using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class SolutionConfiguration : IEntityTypeConfiguration<SolutionEntity>
    {
        public void Configure(EntityTypeBuilder<SolutionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.AnswerText)
                .HasMaxLength(Solution.MAX_LENGTH_ANSWERTEXT)
                .IsRequired();

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.HasIndex(x => new { x.TutorTaskId, x.StudentId })
                .IsUnique();

            builder.HasOne(x => x.TutorTask)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.TutorTaskId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Student)
                .WithMany(x => x.Solutions)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
