using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class ReviewTests
    {
        private static IEnumerable<TestCaseData> InvalidReviewData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Отзыв",
                (short)5,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                "Отзыв",
                (short)5,
                "studentId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                "Отзыв",
                (short)5,
                "tutorId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                (short)5,
                "reviewText");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new string('a', Review.MAX_LENGTH_REVIEWTEXT + 1),
                (short)5,
                "reviewText");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Отзыв",
                (short)0,
                "rating");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Отзыв",
                (short)6,
                "rating");
        }

        [Test]
        public void CreateReviewWithValidData()
        {
            var id = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var tutorId = Guid.NewGuid();
            var reviewText = "Отзыв";
            var rating = (short)5;
            var createdAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = Review.Create(
                id,
                studentId,
                tutorId,
                reviewText,
                rating,
                createdAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.StudentId, Is.EqualTo(studentId));
                Assert.That(result.Value.TutorId, Is.EqualTo(tutorId));
                Assert.That(result.Value.ReviewText, Is.EqualTo(reviewText));
                Assert.That(result.Value.Rating, Is.EqualTo(rating));
                Assert.That(result.Value.CreatedAt, Is.EqualTo(createdAt));
            });
        }

        [TestCaseSource(nameof(InvalidReviewData))]
        public void CreateReviewWithInvalidData(
            Guid id,
            Guid studentId,
            Guid tutorId,
            string reviewText,
            short rating,
            string expectedError)
        {
            var result = Review.Create(
                id,
                studentId,
                tutorId,
                reviewText,
                rating);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
