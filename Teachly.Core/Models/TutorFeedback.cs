using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class TutorFeedback
    {
        public const int MAX_TUTORCOMMENT_LENGTH = 5000;

        private TutorFeedback(Guid id, Guid solutionId, string tutorComment, short grade, DateTime givenAt)
        {
            Id = id;
            SolutionId = solutionId;
            TutorComment = tutorComment;
            Grade = grade;
            GivenAt = givenAt;
        }

        public Guid Id { get; private set; }
        public Guid SolutionId { get; private set; }
        public string TutorComment { get; private set; }
        public short Grade { get; private set; }
        public DateTime GivenAt { get; private set; }

        public static Result<TutorFeedback> Create(Guid id, Guid solutionId, string tutorComment, short grade, DateTime? givenAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<TutorFeedback>($"'{nameof(id)}' не может быть пустым");
            }
            if (solutionId == Guid.Empty)
            {
                return Result.Failure<TutorFeedback>($"'{nameof(solutionId)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(tutorComment) || tutorComment.Length > MAX_TUTORCOMMENT_LENGTH)
            {
                return Result.Failure<TutorFeedback>($"'{nameof(tutorComment)}' не может быть пустым или длиннее {MAX_TUTORCOMMENT_LENGTH} символов");
            }
            if (grade < 2 || grade > 5)
            {
                return Result.Failure<TutorFeedback>($"'{nameof(grade)}' должен находится в диапазоне от 2 до 5");
            }

            var actualGivenAt = givenAt ?? DateTime.UtcNow;

            var tutorFeedback = new TutorFeedback(id, solutionId, tutorComment, grade, actualGivenAt);

            return Result.Success(tutorFeedback);
        }
    }
}
