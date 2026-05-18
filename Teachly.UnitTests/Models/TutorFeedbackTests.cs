using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class TutorFeedbackTests
    {
        private static IEnumerable<TestCaseData> InvalidTutorFeedbackData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                "Комментарий",
                (short)5,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                "Комментарий",
                (short)5,
                "solutionId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "",
                (short)5,
                "tutorComment");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                new string('a', TutorFeedback.MAX_TUTORCOMMENT_LENGTH + 1),
                (short)5,
                "tutorComment");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Комментарий",
                (short)1,
                "grade");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                "Комментарий",
                (short)6,
                "grade");
        }

        [Test]
        public void CreateTutorFeedbackWithValidData()
        {
            var id = Guid.NewGuid();
            var solutionId = Guid.NewGuid();
            var tutorComment = "Комментарий";
            var grade = (short)5;
            var givenAt = new DateTime(2026, 05, 18, 14, 23, 0, DateTimeKind.Utc);

            var result = TutorFeedback.Create(
                id,
                solutionId,
                tutorComment,
                grade,
                givenAt);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.SolutionId, Is.EqualTo(solutionId));
                Assert.That(result.Value.TutorComment, Is.EqualTo(tutorComment));
                Assert.That(result.Value.Grade, Is.EqualTo(grade));
                Assert.That(result.Value.GivenAt, Is.EqualTo(givenAt));
            });
        }

        [TestCaseSource(nameof(InvalidTutorFeedbackData))]
        public void CreateTutorFeedbackWithInvalidData(
            Guid id,
            Guid solutionId,
            string tutorComment,
            short grade,
            string expectedError)
        {
            var result = TutorFeedback.Create(
                id,
                solutionId,
                tutorComment,
                grade);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
