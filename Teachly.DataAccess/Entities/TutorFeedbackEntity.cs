namespace Teachly.DataAccess.Entities
{
    public class TutorFeedbackEntity
    {
        public Guid Id {  get; set; }
        public Guid SolutionId { get; set; }
        public SolutionEntity Solution { get; set; } = null!;
        public string TutorComment { get; set; } = string.Empty;
        public short Grade { get; set; }
        public DateTime GivenAt { get; set; }
    }
}
