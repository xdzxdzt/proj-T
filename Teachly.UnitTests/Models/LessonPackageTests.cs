using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class LessonPackageTests
    {
        private static IEnumerable<TestCaseData> InvalidLessonPackageData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m,
                4,
                2000m,
                null,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                4,
                500m,
                4,
                2000m,
                null,
                "studentId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                4,
                500m,
                4,
                2000m,
                null,
                "tutorSubjectId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                0,
                500m,
                null,
                null,
                null,
                "totalLessons");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                0m,
                null,
                null,
                null,
                "pricePerLesson");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m,
                -1,
                null,
                null,
                "remainingLessons");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m,
                5,
                null,
                null,
                "remainingLessons");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m,
                4,
                100m,
                null,
                "totalPrice");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m,
                1,
                null,
                new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc),
                "completedAt");
        }

        [Test]
        public void CreateLessonPackageWithValidData()
        {
            var id = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var tutorSubjectId = Guid.NewGuid();
            var totalLessons = 4;
            var pricePerLesson = 500m;
            var purchasedAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = LessonPackage.Create(
                id,
                studentId,
                tutorSubjectId,
                totalLessons,
                pricePerLesson,
                purchasedAt: purchasedAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.StudentId, Is.EqualTo(studentId));
                Assert.That(result.Value.TutorSubjectId, Is.EqualTo(tutorSubjectId));
                Assert.That(result.Value.TotalLessons, Is.EqualTo(totalLessons));
                Assert.That(result.Value.RemainingLessons, Is.EqualTo(totalLessons));
                Assert.That(result.Value.PricePerLesson, Is.EqualTo(pricePerLesson));
                Assert.That(result.Value.TotalPrice, Is.EqualTo(2000m));
                Assert.That(result.Value.PurchasedAt, Is.EqualTo(purchasedAt));
                Assert.That(result.Value.CompletedAt, Is.Null);
            });
        }

        [TestCaseSource(nameof(InvalidLessonPackageData))]
        public void CreateLessonPackageWithInvalidData(
            Guid id,
            Guid studentId,
            Guid tutorSubjectId,
            int totalLessons,
            decimal pricePerLesson,
            int? remainingLessons,
            decimal? totalPrice,
            DateTime? completedAt,
            string expectedError)
        {
            var result = LessonPackage.Create(
                id,
                studentId,
                tutorSubjectId,
                totalLessons,
                pricePerLesson,
                remainingLessons,
                totalPrice,
                completedAt: completedAt);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }

        [Test]
        public void UseLessonDecreasesRemainingLessons()
        {
            var lessonPackage = LessonPackage.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                4,
                500m).Value;

            var result = lessonPackage.UseLesson();

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(lessonPackage.RemainingLessons, Is.EqualTo(3));
                Assert.That(lessonPackage.CompletedAt, Is.Null);
            });
        }

        [Test]
        public void UseLastLessonSetsCompletedAt()
        {
            var lessonPackage = LessonPackage.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                1,
                500m).Value;

            var result = lessonPackage.UseLesson();

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(lessonPackage.RemainingLessons, Is.EqualTo(0));
                Assert.That(lessonPackage.CompletedAt, Is.Not.Null);
            });
        }

        [Test]
        public void UseLessonWithoutRemainingLessonsReturnsFailure()
        {
            var lessonPackage = LessonPackage.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                1,
                500m).Value;
            lessonPackage.UseLesson();
            var completedAt = lessonPackage.CompletedAt;

            var result = lessonPackage.UseLesson();

            Assert.That(result.IsFailure, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Error, Does.Contain("RemainingLessons"));
                Assert.That(lessonPackage.RemainingLessons, Is.EqualTo(0));
                Assert.That(lessonPackage.CompletedAt, Is.EqualTo(completedAt));
            });
        }
    }
}
