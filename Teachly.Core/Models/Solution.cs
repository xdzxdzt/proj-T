using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class Solution
    {
        public const int MAX_LENGTH_ANSWERTEXT = 2000;
        private Solution(Guid id, Guid tutorTaskId, Guid studentId, string answerText, DateTime submittedAt)
        {
            Id = id;
            TutorTaskId = tutorTaskId;
            StudentId = studentId;
            AnswerText = answerText;
            SubmittedAt = submittedAt;
        }

        public Guid Id { get; private set; }
        public Guid TutorTaskId { get; private set; }
        public Guid StudentId { get; private set; }
        public string AnswerText { get; private set; }
        public DateTime SubmittedAt { get; private set; }

        public static Result<Solution> Create(Guid id, Guid tutorTaskId, Guid studentId, string answerText, DateTime? submittedAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Solution>($"'{nameof(id)}' не может быть пустым");
            }
            if (tutorTaskId == Guid.Empty)
            {
                return Result.Failure<Solution>($"'{nameof(tutorTaskId)}' не может быть пустым");
            }
            if (studentId == Guid.Empty)
            {
                return Result.Failure<Solution>($"'{nameof(studentId)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(answerText) || answerText.Length > MAX_LENGTH_ANSWERTEXT)
            {
                return Result.Failure<Solution>($"'{nameof(answerText)}' не может быть пустым или длиннее {MAX_LENGTH_ANSWERTEXT} символов");
            }

            var actualSubmittedAt = submittedAt ?? DateTime.UtcNow;

            var solution = new Solution(id, tutorTaskId, studentId, answerText, actualSubmittedAt);

            return Result.Success(solution);
        }
    }
}
