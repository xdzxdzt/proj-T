namespace Teachly.DataAccess.Entities
{
    public class StudentEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public Guid? InstitutionId { get; set; }
        public InstitutionEntity? Institution { get; set; }
        public int EducationLevel { get; set; }
        public string? ParentPhone { get; set; }
        public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
        public ICollection<LessonPackageEntity> LessonPackages { get; set; } = new List<LessonPackageEntity>();
        public ICollection<SolutionEntity> Solutions { get; set; } = new List<SolutionEntity>();
    }
}
