using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Teachly.Core.Enums;
using Teachly.Core.Models;
using Teachly.DataAccess.Entities;

namespace Teachly.DataAccess.Configurations
{
    public class InstitutionConfiguration : IEntityTypeConfiguration<InstitutionEntity>
    {
        public void Configure(EntityTypeBuilder<InstitutionEntity> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(Institution.MAX_LENGTH_NAME)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(Institution.MAX_LENGTH_CITY)
                .IsRequired();

            builder.Property(x => x.Type)
                .HasConversion<string>()
                .IsRequired();

            builder.HasData(
                new InstitutionEntity
                {
                    Id = Guid.Parse("1b2d6d50-c1d6-4f63-8a5a-ffbb7f4f6e5a"),
                    Type = InstitutionType.School,
                    Name = "Средняя общеобразовательная школа N 1",
                    City = "Самара"
                },
                new InstitutionEntity
                {
                    Id = Guid.Parse("80b2760a-77d4-4bda-a501-9f42688bc8d4"),
                    Type = InstitutionType.School,
                    Name = "Лицей информационных технологий",
                    City = "Самара"
                },
                new InstitutionEntity
                {
                    Id = Guid.Parse("0deaa567-50ad-46c5-926f-e8d35190236a"),
                    Type = InstitutionType.College,
                    Name = "Самарский колледж сервиса производственного оборудования",
                    City = "Самара"
                },
                new InstitutionEntity
                {
                    Id = Guid.Parse("87090f4c-40c6-4fb5-8e3e-bd7530362f50"),
                    Type = InstitutionType.University,
                    Name = "Самарский университет",
                    City = "Самара"
                });
        }
    }
}
