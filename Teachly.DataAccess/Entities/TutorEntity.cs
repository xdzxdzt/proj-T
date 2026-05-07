namespace Teachly.DataAccess.Entities
{
    public class TutorEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public UserEntity User { get; set; } = null!;
        public string? Description { get; set; }
        public decimal AverageRating { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
        public ICollection<TutorSubjectEntity> TutorSubjects { get; set; } = new List<TutorSubjectEntity>();
    }
}
