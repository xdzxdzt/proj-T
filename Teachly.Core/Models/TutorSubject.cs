using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class TutorSubject
    {
        private TutorSubject(Guid id, Guid tutorId, Guid subjectId, decimal pricePerLesson)
        {
            Id = id;
            TutorId = tutorId;
            SubjectId = subjectId;
            PricePerLesson = pricePerLesson;
        }

        public Guid Id { get; private set; }
        public Guid TutorId { get; private set; }
        public Guid SubjectId { get; private set; }
        public decimal PricePerLesson { get; private set; }

        public static Result<TutorSubject> Create(Guid id, Guid tutorId, Guid subjectId, decimal pricePerLesson)
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
            if (pricePerLesson <= 0)
            {
                return Result.Failure<TutorSubject>($"'{nameof(pricePerLesson)}' должна быть больше 0");
            }

            var tutorSubject = new TutorSubject(id, tutorId, subjectId, pricePerLesson);

            return Result.Success(tutorSubject);
        }
    }
}
