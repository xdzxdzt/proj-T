using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        }
    }
}
