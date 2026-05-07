using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class TutorTask
    {
        public const int MAX_TITLE_LENGTH = 200;
        public const int MAX_DESCRIPTION_LENGTH = 5000;

        private TutorTask(Guid id, Guid lessonPackageId, string title, string description, DateTime createdAt, DateTime? closedAt)
        {
            Id = id;
            LessonPackageId = lessonPackageId;
            Title = title;
            Description = description;
            CreatedAt = createdAt;
            ClosedAt = closedAt;
        }

        public Guid Id { get; private set; }
        public Guid LessonPackageId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? ClosedAt { get; private set; }

        public static Result<TutorTask> Create(
            Guid id,
            Guid lessonPackageId,
            string title,
            string description,
            DateTime? createdAt = null,
            DateTime? closedAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<TutorTask>($"'{nameof(id)}' не должен быть пустым");
            }
            if (lessonPackageId == Guid.Empty)
            {
                return Result.Failure<TutorTask>($"'{nameof(lessonPackageId)}' не должен быть пустым");
            }
            if (string.IsNullOrWhiteSpace(title) || title.Length > MAX_TITLE_LENGTH)
            {
                return Result.Failure<TutorTask>($"'{nameof(title)}' не может быть пустым или длиннее {MAX_TITLE_LENGTH} символов");
            }
            if (string.IsNullOrWhiteSpace(description) || description.Length > MAX_DESCRIPTION_LENGTH)
            {
                return Result.Failure<TutorTask>($"'{nameof(description)}' не может быть пустым или длиннее {MAX_DESCRIPTION_LENGTH} символов");
            }

            var actualCreatedAt = createdAt ?? DateTime.UtcNow;

            if (closedAt is not null && closedAt < actualCreatedAt)
            {
                return Result.Failure<TutorTask>($"'{nameof(closedAt)}' не может быть раньше '{nameof(createdAt)}'");
            }

            var tutorTask = new TutorTask(id, lessonPackageId, title, description, actualCreatedAt, closedAt);

            return Result.Success(tutorTask);
        }

        public Result Close()
        {
            if (ClosedAt is not null)
            {
                return Result.Failure($"'{nameof(ClosedAt)}' уже задан");
            }

            ClosedAt = DateTime.UtcNow;

            return Result.Success();
        }
    }
}
