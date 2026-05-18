using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class SubjectTests
    {
        private static IEnumerable<TestCaseData> InvalidSubjectData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                "Математика",
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                "",
                "name");

            yield return new TestCaseData(
                Guid.NewGuid(),
                new string('a', Subject.MAX_NAME_LENGTH + 1),
                "name");
        }

        [Test]
        public void CreateSubjectWithValidData()
        {
            var id = Guid.NewGuid();
            var name = "Математика";

            var result = Subject.Create(id, name);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.Name, Is.EqualTo(name));
            });
        }

        [TestCaseSource(nameof(InvalidSubjectData))]
        public void CreateSubjectWithInvalidData(
            Guid id,
            string name,
            string expectedError)
        {
            var result = Subject.Create(id, name);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
