namespace Teachly.DataAccess.Entities
{
    public class TutorSubjectEntity
    {
        public Guid Id { get; set; }
        public Guid TutorId { get; set; }
        public TutorEntity Tutor { get; set; } = null!;
        public Guid SubjectId { get; set; }
        public SubjectEntity Subject { get; set; } = null!;
        public decimal PricePerHour { get; set; }
        public ICollection<LessonPackageEntity> LessonPackages { get; set; } = new List<LessonPackageEntity>();
    }
}
