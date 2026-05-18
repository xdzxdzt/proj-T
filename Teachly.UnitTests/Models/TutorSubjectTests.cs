using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class TutorSubjectTests
    {
        private static IEnumerable<TestCaseData> InvalidTutorSubjectData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                500m,
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                500m,
                "tutorId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.Empty,
                500m,
                "subjectId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                0m,
                "pricePerLesson");
        }

        [Test]
        public void CreateTutorSubjectWithValidData()
        {
            var id = Guid.NewGuid();
            var tutorId = Guid.NewGuid();
            var subjectId = Guid.NewGuid();
            var pricePerLesson = 500m;

            var result = TutorSubject.Create(
                id,
                tutorId,
                subjectId,
                pricePerLesson);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.TutorId, Is.EqualTo(tutorId));
                Assert.That(result.Value.SubjectId, Is.EqualTo(subjectId));
                Assert.That(result.Value.PricePerLesson, Is.EqualTo(pricePerLesson));
            });
        }

        [TestCaseSource(nameof(InvalidTutorSubjectData))]
        public void CreateTutorSubjectWithInvalidData(
            Guid id,
            Guid tutorId,
            Guid subjectId,
            decimal pricePerLesson,
            string expectedError)
        {
            var result = TutorSubject.Create(
                id,
                tutorId,
                subjectId,
                pricePerLesson);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
