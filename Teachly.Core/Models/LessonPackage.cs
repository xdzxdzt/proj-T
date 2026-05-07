using CSharpFunctionalExtensions;

namespace Teachly.Core.Models
{
    public class LessonPackage
    {
        private LessonPackage(
            Guid id,
            Guid studentId,
            Guid tutorSubjectId,
            int totalLessons,
            int remainingLessons,
            decimal pricePerLesson,
            decimal totalPrice,
            DateTime purchasedAt,
            DateTime? completedAt)
        {
            Id = id;
            StudentId = studentId;
            TutorSubjectId = tutorSubjectId;
            TotalLessons = totalLessons;
            RemainingLessons = remainingLessons;
            PricePerLesson = pricePerLesson;
            TotalPrice = totalPrice;
            PurchasedAt = purchasedAt;
            CompletedAt = completedAt;
        }

        public Guid Id { get; private set; }
        public Guid StudentId { get; private set; }
        public Guid TutorSubjectId { get; private set; }
        public int TotalLessons { get; private set; }
        public int RemainingLessons { get; private set; }
        public decimal PricePerLesson { get; private set; }
        public decimal TotalPrice { get; private set; }
        public DateTime PurchasedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public static Result<LessonPackage> Create(
            Guid id,
            Guid studentId,
            Guid tutorSubjectId,
            int totalLessons,
            decimal pricePerLesson,
            int? remainingLessons = null,
            decimal? totalPrice = null,
            DateTime? purchasedAt = null,
            DateTime? completedAt = null)
        {
            if (id == Guid.Empty)
            {
                return Result.Failure<LessonPackage>($"'{nameof(id)}' не может быть пустым");
            }
            if (studentId == Guid.Empty)
            {
                return Result.Failure<LessonPackage>($"'{nameof(studentId)}' не может быть пустым");
            }
            if (tutorSubjectId == Guid.Empty)
            {
                return Result.Failure<LessonPackage>($"'{nameof(tutorSubjectId)}' не может быть пустым");
            }
            if (totalLessons <= 0)
            {
                return Result.Failure<LessonPackage>($"'{nameof(totalLessons)}' должно быть больше 0");
            }
            if (pricePerLesson <= 0)
            {
                return Result.Failure<LessonPackage>($"'{nameof(pricePerLesson)}' должно быть больше 0");
            }

            var actualRemainingLessons = remainingLessons ?? totalLessons;

            var actualTotalPrice = totalPrice ?? totalLessons * pricePerLesson;

            var actualPurchasedAt = purchasedAt ?? DateTime.UtcNow;

            if (actualRemainingLessons < 0)
            {
                return Result.Failure<LessonPackage>($"'{nameof(remainingLessons)}' не может быть отрицательным");
            }
            if (actualRemainingLessons > totalLessons)
            {
                return Result.Failure<LessonPackage>($"'{nameof(remainingLessons)}' не может быть больше '{nameof(totalLessons)}'");
            }
            if (actualTotalPrice != totalLessons * pricePerLesson)
            {
                return Result.Failure<LessonPackage>($"'{nameof(totalPrice)}' должен быть равен '{nameof(totalLessons)}' * '{nameof(pricePerLesson)}'");
            }
            if (completedAt is not null && actualRemainingLessons > 0)
            {
                return Result.Failure<LessonPackage>($"'{nameof(completedAt)}' может быть установлено только в том случае, если '{nameof(remainingLessons)}' равно 0");
            }

            var lessonPackage = new LessonPackage(
                id,
                studentId,
                tutorSubjectId,
                totalLessons,
                actualRemainingLessons,
                pricePerLesson,
                actualTotalPrice,
                actualPurchasedAt,
                completedAt);

            return Result.Success(lessonPackage);
        }

        public Result UseLesson()
        {
            if (RemainingLessons <= 0)
            {
                return Result.Failure($"'{nameof(RemainingLessons)}' должно быть больше 0");
            }

            RemainingLessons--;

            if (RemainingLessons == 0)
            {
                CompletedAt = DateTime.UtcNow;
            }

            return Result.Success();
        }
    }
}
