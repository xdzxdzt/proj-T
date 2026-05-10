using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class Tutor
    {
        public const int MAX_DESCRIPTION_LENGTH = 5000;
        private Tutor(Guid id, Guid userId, string? description, decimal averageRating, int ratingCount)
        {
            Id = id;
            UserId = userId;
            Description = description;
            AverageRating = averageRating;
            RatingCount = ratingCount;
        }

        public Guid Id { get; private set; }
        public Guid UserId { get; private set; }
        public string? Description { get; private set; }
        public decimal AverageRating { get; private set; }
        public int RatingCount { get; private set; }

        public static Result<Tutor> Create(
            Guid id,
            Guid userId,
            string? description,
            decimal averageRating = 0,
            int ratingCount = 0)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<Tutor>($"'{nameof(id)}' не может быть пустым");
            }

            if (userId == Guid.Empty)
            {
                return Result.Failure<Tutor>($"'{nameof(userId)}' не может быть пустым");
            }

            if (description?.Length > MAX_DESCRIPTION_LENGTH)
            {
                return Result.Failure<Tutor>($"'{nameof(description)}' не может быть длиннее {MAX_DESCRIPTION_LENGTH} символов");
            }

            if (averageRating < 0 || averageRating > 5)
            {
                return Result.Failure<Tutor>($"'{nameof(averageRating)}' должен быть в диапазоне от 0 до 5");
            }

            if (ratingCount < 0)
            {
                return Result.Failure<Tutor>($"'{nameof(ratingCount)}' не может быть отрицательным");
            }

            var tutor = new Tutor(id, userId, description, averageRating, ratingCount);

            return Result.Success(tutor);
        }

        public Result AddRating(short newRating)
        {
            if (newRating < 1 || newRating > 5)
            {
                return Result.Failure($"'{nameof(newRating)}' должен быть в диапазоне от 1 до 5");
            }

            var total = AverageRating * RatingCount;

            RatingCount++;

            AverageRating = Math.Round((total + newRating) / RatingCount, 2);

            return Result.Success();
        }
    }
}
