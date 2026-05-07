using Teachly.Core.Enums;

namespace Teachly.DataAccess.Entities
{
    public class InstitutionEntity
    {
        public Guid Id { get; set; }
        public InstitutionType Type { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();
    }
}
