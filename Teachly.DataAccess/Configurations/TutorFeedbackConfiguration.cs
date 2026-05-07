using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class TutorFeedbackConfiguration : IEntityTypeConfiguration<TutorFeedbackEntity>
    {
        public void Configure(EntityTypeBuilder<TutorFeedbackEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TutorComment)
                .HasMaxLength(TutorFeedback.MAX_TUTORCOMMENT_LENGTH)
                .IsRequired();

            builder.Property(x => x.Grade)
                .IsRequired();

            builder.Property(x => x.GivenAt)
                .IsRequired();

            builder.HasIndex(x => x.SolutionId)
                .IsUnique();

            builder.HasOne(x => x.Solution)
                .WithOne(x => x.TutorFeedback)
                .HasForeignKey<TutorFeedbackEntity>(x => x.SolutionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_TutorFeedbacks_Grade", "\"Grade\" BETWEEN 2 AND 5"));
        }
    }
}
