using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class Review
    {
        public const int MAX_LENGTH_REVIEWTEXT = 5000;

        private Review(Guid id, Guid studentId, Guid tutorId, string reviewText, short rating, DateTime createdAt)
        {
            Id = id;
            StudentId = studentId;
            TutorId = tutorId;
            ReviewText = reviewText;
            Rating = rating;
            CreatedAt = createdAt;
        }

        public Guid Id { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid TutorId { get; private set; }
        public string ReviewText { get; private set; }
        public short Rating { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public static Result<Review> Create(Guid id, Guid studentId, Guid tutorId, string reviewText, short rating, DateTime? createdAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Review>($"'{nameof(id)}' не может быть пустым");
            }
            if (studentId == Guid.Empty)
            {
                return Result.Failure<Review>($"'{nameof(studentId)}' не может быть пустым");
            }
            if (tutorId == Guid.Empty)
            {
                return Result.Failure<Review>($"'{nameof(tutorId)}' не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(reviewText) || reviewText.Length > MAX_LENGTH_REVIEWTEXT)
            {
                return Result.Failure<Review>($"'{nameof(reviewText)}' не может быть пустым или длиннее {MAX_LENGTH_REVIEWTEXT} символов");
            }
            if (rating < 1 || rating > 5)
            {
                return Result.Failure<Review>($"'{nameof(rating)}' должен быть в диапазоне от 1 до 5");
            }

            var actualCreatedAt = createdAt ?? DateTime.UtcNow;
            var review = new Review(id, studentId, tutorId, reviewText, rating, actualCreatedAt);

            return Result.Success(review);
        }
    }
}
