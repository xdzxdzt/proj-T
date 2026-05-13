using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<SubjectEntity>
    {
        public void Configure(EntityTypeBuilder<SubjectEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(Subject.MAX_NAME_LENGTH)
                .IsRequired();

            builder.HasIndex(x => x.Name)
                .IsUnique();

            builder.HasData(
                new SubjectEntity
                {
                    Id = Guid.Parse("4e3359ab-542a-4486-99e1-69cf6651f3a1"),
                    Name = "Математика"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("e7b2ff76-a47d-4da2-8755-ae617de01f94"),
                    Name = "Русский язык"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("334a4427-66eb-411c-903a-f7c2a62b9f7c"),
                    Name = "Английский язык"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("f27ce41c-d725-417c-b49f-f668918c34db"),
                    Name = "Информатика"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("a3901178-731e-4691-80b7-62d322735f18"),
                    Name = "Физика"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("f54acc99-b9b0-4bcf-8d98-8e5842959be3"),
                    Name = "Химия"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("3ba55ab4-9ba4-491e-a3cc-752067db3b91"),
                    Name = "Биология"
                },
                new SubjectEntity
                {
                    Id = Guid.Parse("b7a24854-3d03-4c31-bc00-7e4ce5d07aaa"),
                    Name = "История"
                });
        }
    }
}
