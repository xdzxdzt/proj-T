using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class StudentTests
    {
        private static IEnumerable<TestCaseData> InvalidStudentData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                Guid.NewGuid(),
                Guid.NewGuid(),
                9,
                "+79990000000",
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.Empty,
                Guid.NewGuid(),
                9,
                "+79990000000",
                "userId");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                0,
                "+79990000000",
                "educationLevel");

            yield return new TestCaseData(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                9,
                new string('1', Student.MAX_LENGTH_PARENTPHONE + 1),
                "parentPhone");
        }

        [Test]
        public void CreateStudentWithValidData()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var institutionId = Guid.NewGuid();
            var educationLevel = 9;
            var parentPhone = "+79990000000";

            var result = Student.Create(
                id,
                userId,
                institutionId,
                educationLevel,
                parentPhone);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.UserId, Is.EqualTo(userId));
                Assert.That(result.Value.InstitutionId, Is.EqualTo(institutionId));
                Assert.That(result.Value.EducationLevel, Is.EqualTo(educationLevel));
                Assert.That(result.Value.ParentPhone, Is.EqualTo(parentPhone));
            });
        }

        [TestCaseSource(nameof(InvalidStudentData))]
        public void CreateStudentWithInvalidData(
            Guid id,
            Guid userId,
            Guid? institutionId,
            int educationLevel,
            string? parentPhone,
            string expectedError)
        {
            var result = Student.Create(
                id,
                userId,
                institutionId,
                educationLevel,
                parentPhone);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
