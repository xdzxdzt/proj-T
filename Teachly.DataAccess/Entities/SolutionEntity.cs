namespace Teachly.DataAccess.Entities
{
    public class SolutionEntity
    {
        public Guid Id { get; set; }
        public Guid TutorTaskId { get; set; }
        public TutorTaskEntity TutorTask { get; set; } = null!;
        public Guid StudentId { get; set; }
        public StudentEntity Student { get; set; } = null!;
        public string AnswerText { get; set; } = string.Empty;
        public DateTime SubmittedAt { get; set; }
        public TutorFeedbackEntity? TutorFeedback { get; set; }
    }
}
