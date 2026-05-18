using Teachly.Core.Enums;
using Teachly.Core.Models;

namespace Teachly.UnitTests.Models
{
    public class InstitutionTests
    {
        private static IEnumerable<TestCaseData> InvalidInstitutionData()
        {
            yield return new TestCaseData(
                Guid.Empty,
                InstitutionType.School,
                "Школа",
                "Самара",
                "id");

            yield return new TestCaseData(
                Guid.NewGuid(),
                InstitutionType.School,
                "",
                "Самара",
                "name");

            yield return new TestCaseData(
                Guid.NewGuid(),
                InstitutionType.School,
                "Школа",
                "",
                "city");

            yield return new TestCaseData(
                Guid.NewGuid(),
                InstitutionType.School,
                new string('a', Institution.MAX_LENGTH_NAME + 1),
                "Самара",
                "name");

            yield return new TestCaseData(
                Guid.NewGuid(),
                InstitutionType.School,
                "Школа",
                new string('a', Institution.MAX_LENGTH_CITY + 1),
                "city");
        }

        [Test]
        public void CreateInstitutionWithValidData()
        {
            var id = Guid.NewGuid();
            var type = InstitutionType.School;
            var name = "Школа";
            var city = "Самара";

            var result = Institution.Create(id, type, name, city);

            Assert.That(result.IsSuccess, Is.True);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(id));
                Assert.That(result.Value.Type, Is.EqualTo(type));
                Assert.That(result.Value.Name, Is.EqualTo(name));
                Assert.That(result.Value.City, Is.EqualTo(city));
            });
        }

        [TestCaseSource(nameof(InvalidInstitutionData))]
        public void CreateInstitutionWithInvalidData(
            Guid id,
            InstitutionType type,
            string name,
            string city,
            string expectedError)
        {
            var result = Institution.Create(id, type, name, city);

            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Does.Contain(expectedError));
        }
    }
}
