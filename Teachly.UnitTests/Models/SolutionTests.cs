using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class SolutionTests
    {
        private static IEnumerable<TestCaseData> InvalidSolutionData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Ответ",
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                "Ответ",
                "tutorTaskId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                "Ответ",
                "studentId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                "answerText");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                new string('a', Solution.MAX_LENGTH_ANSWERTEXT + 1),
                "answerText");
        }

        [Test]
        public void CreateSolutionWithValidData()
        {
            var id = Guid.NewGuid();
            var tutorTaskId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var answerText = "Ответ";
            var submittedAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = Solution.Create(
                id,
                tutorTaskId,
                studentId,
                answerText,
                submittedAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.TutorTaskId, Is.EqualTo(tutorTaskId));
                Assert.That(result.Value.StudentId, Is.EqualTo(studentId));
                Assert.That(result.Value.AnswerText, Is.EqualTo(answerText));
                Assert.That(result.Value.SubmittedAt, Is.EqualTo(submittedAt));
            });
        }

        [TestCaseSource(nameof(InvalidSolutionData))]
        public void CreateSolutionWithInvalidData(
            Guid id,
            Guid tutorTaskId,
            Guid studentId,
            string answerText,
            string expectedError)
        {
            var result = Solution.Create(
                id,
                tutorTaskId,
                studentId,
                answerText);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
