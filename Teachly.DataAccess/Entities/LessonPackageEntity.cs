namespace Teachly.DataAccess.Entities
{
    public class LessonPackageEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public StudentEntity Student { get; set; } = null!;
        public Guid TutorSubjectId { get; set; }
        public TutorSubjectEntity TutorSubject { get; set; } = null!;
        public int TotalLessons { get; set; }
        public int RemainingLessons { get; set; }
        public decimal PricePerLesson { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PurchasedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public ICollection<TutorTaskEntity> TutorTasks { get; set; } = new List<TutorTaskEntity>();
    }
}