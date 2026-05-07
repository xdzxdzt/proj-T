namespace Teachly.DataAccess.Entities
{
    public class SubjectEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<TutorSubjectEntity> TutorSubjects { get; set; } = new List<TutorSubjectEntity>();
    }
}
