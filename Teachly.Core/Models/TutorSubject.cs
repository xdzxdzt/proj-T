using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class TutorSubject
    {
        private TutorSubject(Guid id, Guid tutorId, Guid subjectId, decimal pricePerHour)
        {
            Id = id;
            TutorId = tutorId;
            SubjectId = subjectId;
            PricePerHour = pricePerHour;
        }

        public Guid Id { get; private set; }
        public Guid TutorId { get; private set; }
        public Guid SubjectId { get; private set; }
        public decimal PricePerHour { get; private set; }

        public static Result<TutorSubject> Create(Guid id, Guid tutorId, Guid subjectId, decimal pricePerHour)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<TutorSubject>($"'{nameof(id)}' не может быть пустым");
            }
            if (tutorId == Guid.Empty)
            {
                return Result.Failure<TutorSubject>($"'{nameof(tutorId)}' не может быть пустым");
            }
            if (subjectId == Guid.Empty)
            {
                return Result.Failure<TutorSubject>($"'{nameof(subjectId)}' не может быть пустым");
            }
            if (pricePerHour <= 0)
            {
                return Result.Failure<TutorSubject>($"'{nameof(pricePerHour)}' должна быть больше 0");
            }

            var tutorSubject = new TutorSubject(id, tutorId, subjectId, pricePerHour);

            return Result.Success(tutorSubject);
        }
    }
}
