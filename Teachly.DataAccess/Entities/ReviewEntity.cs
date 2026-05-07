namespace Teachly.DataAccess.Entities
{
    public class ReviewEntity
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public StudentEntity Student { get; set; } = null!;
        public Guid TutorId { get; set; }
        public TutorEntity Tutor { get; set; } = null!;
        public string ReviewText { get; set; } = string.Empty;
        public short Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
