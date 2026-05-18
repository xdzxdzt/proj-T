using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class TutorTaskTests
    {
        private static IEnumerable<TestCaseData> InvalidTutorTaskData()
        {
            var createdAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                "Задание",
                "Описание задания",
                createdAt,
                null,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                "Задание",
                "Описание задания",
                createdAt,
                null,
                "lessonPackageId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                "Описание задания",
                createdAt,
                null,
                "title");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Задание",
                "",
                createdAt,
                null,
                "description");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Задание",
                "Описание задания",
                createdAt,
                createdAt.AddMinutes(-1),
                "closedAt");
        }

        [Test]
        public void CreateTutorTaskWithValidData()
        {
            var id = Guid.NewGuid();
            var lessonPackageId = Guid.NewGuid();
            var title = "Задание";
            var description = "Описание задания";
            var createdAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = TutorTask.Create(
                id,
                lessonPackageId,
                title,
                description,
                createdAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.LessonPackageId, Is.EqualTo(lessonPackageId));
                Assert.That(result.Value.Title, Is.EqualTo(title));
                Assert.That(result.Value.Description, Is.EqualTo(description));
                Assert.That(result.Value.CreatedAt, Is.EqualTo(createdAt));
                Assert.That(result.Value.ClosedAt, Is.Null);
            });
        }

        [TestCaseSource(nameof(InvalidTutorTaskData))]
        public void CreateTutorTaskWithInvalidData(
            Guid id,
            Guid lessonPackageId,
            string title,
            string description,
            DateTime createdAt,
            DateTime? closedAt,
            string expectedError)
        {
            var result = TutorTask.Create(
                id,
                lessonPackageId,
                title,
                description,
                createdAt,
                closedAt);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }

        [Test]
        public void CloseTutorTaskSetsClosedAt()
        {
            var task = TutorTask.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Задание",
                "Описание задания").Value;

            var result = task.Close();

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(task.ClosedAt, Is.Not.Null);
        }

        [Test]
        public void CloseAlreadyClosedTutorTaskReturnsFailure()
        {
            var task = TutorTask.Create(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Задание",
                "Описание задания").Value;
            task.Close();
            var closedAt = task.ClosedAt;

            var result = task.Close();

            Assert.That(result.IsFailure, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Error, Does.Contain("ClosedAt"));
                Assert.That(task.ClosedAt, Is.EqualTo(closedAt));
            });
        }
    }
}
