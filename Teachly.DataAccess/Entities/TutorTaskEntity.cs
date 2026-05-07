namespace Teachly.DataAccess.Entities
{
    public class TutorTaskEntity
    {
        public Guid Id { get; set; }
        public Guid LessonPackageId { get; set; }
        public LessonPackageEntity LessonPackage { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public ICollection<SolutionEntity> Solutions { get; set; } = new List<SolutionEntity>();
    }
}
