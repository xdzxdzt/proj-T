using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<StudentEntity>
    {
        public void Configure(EntityTypeBuilder<StudentEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EducationLevel)
                .IsRequired();

            builder.Property(x => x.ParentPhone)
                .HasMaxLength(Student.MAX_LENGTH_PARENTPHONE);

            builder.HasIndex(x => x.UserId)
                .IsUnique();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Student)
                .HasForeignKey<StudentEntity>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Institution)
                .WithMany(x => x.Students)
                .HasForeignKey(x => x.InstitutionId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Students_EducationLevel", "\"EducationLevel\" BETWEEN 1 AND 11"));
        }
    }
}
